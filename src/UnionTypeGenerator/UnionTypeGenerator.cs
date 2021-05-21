using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnionTypeGenerator
{
    [Generator]
    public sealed class UnionTypeGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var compilation = context.Compilation;

            //System.Diagnostics.Debugger.Launch();

            var resultModels = new List<TemplatingModel>();

            var resultTypes = Enumerable.Range(2, 4).Select(n => compilation.GetTypeByMetadataName($"UnionTypeGenerator.IUnion`{n}")!);

            foreach (var tree in compilation.SyntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(tree);

                foreach (var node in tree.GetRoot().DescendantNodesAndSelf().OfType<StructDeclarationSyntax>())
                {
                    var structSymbol = (INamedTypeSymbol)semanticModel.GetDeclaredSymbol(node)!;

                    var resultInterfaceSymbol = resultTypes.Select(r =>
                    {
                        if (structSymbol.AllInterfaces.Length == 0)
                            return null;

                        var candidateInterface = structSymbol.AllInterfaces.FirstOrDefault(i => SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, r));
                        if (candidateInterface is null)
                            return null;

                        var constructedInterface = r.Construct(candidateInterface.TypeArguments.ToArray());
                        return compilation.HasImplicitConversion(structSymbol, constructedInterface) ? constructedInterface : null;
                    }).SingleOrDefault(i => i is not null);

                    if (resultInterfaceSymbol is null)
                        continue;

                    resultModels.Add(new TemplatingModel(structSymbol, resultInterfaceSymbol, compilation));
                }
            }

            var file = @"resources/Union.sbn-cs";
            var template = Template.Parse(EmbeddedResource.GetContent(file), file);

            foreach (var resultModel in resultModels)
            {
                var output = template.Render(resultModel, member => member.Name);

                context.AddSource($"Union_{resultModel.Name}.g.cs", SourceText.From(output, Encoding.UTF8));
            }
        }

        private sealed class TemplatingModel
        {
            public readonly INamedTypeSymbol Concrete;
            public readonly INamedTypeSymbol Interface;

            public readonly bool AllowUnsafeIsOn;

            public string Namespace =>
                Concrete.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat);

            public string InterfaceName =>
                Interface.GetTypeSymbolFullName();

            public string Name => Concrete.Name;

            public IEnumerable<UnionType> UnionTypes
            {
                get
                {
                    var typeSymbols = Interface.TypeArguments;

                    for (int i = 0; i < typeSymbols.Length; i++)
                    {
                        var typeSymbol = typeSymbols[i];

                        var typeName = typeSymbol.GetTypeSymbolFullName();
                        var tag = i + 1;
                        var fieldName = $"_t{tag - 1}Value";
                        var comma = i < typeSymbols.Length - 1 ? "," : "";
                        yield return new UnionType(typeName, fieldName, comma, tag);
                    }
                }
            }

            public TemplatingModel(INamedTypeSymbol concrete, INamedTypeSymbol @interface, Compilation compilation)
            {
                Concrete = concrete;
                Interface = @interface;
                AllowUnsafeIsOn = compilation.Options is CSharpCompilationOptions csharpOptions &&
                    csharpOptions.AllowUnsafe;
            }

            public sealed class UnionType
            {
                public readonly string TypeName;
                public readonly string FieldName;
                public readonly string Comma;
                public readonly int Tag;

                public int TypeArgumentIndex => Tag - 1;

                public UnionType(string typeName, string fieldName, string comma, int tag)
                {
                    TypeName = typeName;
                    FieldName = fieldName;
                    Comma = comma;
                    Tag = tag;
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
