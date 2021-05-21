using Microsoft.CodeAnalysis;

namespace UnionTypeGenerator
{
    public static class RoslynExtensions
    {
        public static string GetTypeSymbolFullName(this ITypeSymbol symbol, bool withGlobalPrefix = true, bool includeTypeParameters = true)
        {
            return symbol.ToDisplayString(new SymbolDisplayFormat(
                withGlobalPrefix ? SymbolDisplayGlobalNamespaceStyle.Included : SymbolDisplayGlobalNamespaceStyle.Omitted,
                SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                includeTypeParameters ? SymbolDisplayGenericsOptions.IncludeTypeParameters : SymbolDisplayGenericsOptions.None,
                miscellaneousOptions: SymbolDisplayMiscellaneousOptions.ExpandNullable
            ));
        }
    }
}
