using ObjectLayoutInspector;
using Xunit;

namespace UnionTypeGenerator.Tests
{
    public enum SomeError { Generic, NotFound }

    public readonly partial struct SomeResult : IUnion<float, SomeError>
    {
    }

    public sealed class BasicTests
    {
        [Fact]
        public void Test_Basic_Union()
        {
            var result = new SomeResult(2.0f);
            var value = result.Match(v => v, _ => 0f);
            Assert.Equal(2.0f, value);

            result = new SomeResult(SomeError.NotFound);
            var value2 = result.Match(_ => (SomeError)int.MaxValue, e => e);
            Assert.Equal(SomeError.NotFound, value2);
        }

        [Fact]
        public void Test_Memory_Size()
        {
            var layout = TypeLayout.GetLayout<SomeResult>();

            Assert.Equal(8, layout.FullSize);
        }
    }
}
