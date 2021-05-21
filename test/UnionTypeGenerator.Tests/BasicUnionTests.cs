using ObjectLayoutInspector;
using System;
using Xunit;

namespace UnionTypeGenerator.Tests
{
    public enum SomeError { Generic, NotFound }

    public readonly partial struct SomeResult : IUnion<float, SomeError>
    {
    }

    public readonly partial struct SomeResult2 : IUnion<Guid, SomeError>
    {
    }

    public sealed class BasicUnionTests
    {
        [Fact]
        public void Test_Basic_Union()
        {
            var result = new SomeResult(2.0f);
            var value = result.Match(v => v, _ => 0f);
            Assert.Equal(2.0f, value);

            Assert.True(result.IsT0);
            Assert.False(result.IsT1);

            result = new SomeResult(SomeError.NotFound);
            var value2 = result.Match(_ => (SomeError)int.MaxValue, e => e);
            Assert.Equal(SomeError.NotFound, value2);

            Assert.False(result.IsT0);
            Assert.True(result.IsT1);
        }

        [Fact]
        public void Test_Memory_Size()
        {
            var layout = TypeLayout.GetLayout<SomeResult>();

            Assert.Equal(8, layout.FullSize);
        }

        [Fact]
        public void Test_Memory_Size_Larger()
        {
            var layout = TypeLayout.GetLayout<SomeResult2>();

            Assert.Equal(20, layout.FullSize);
        }

        [Fact]
        public void Test_Implicit_Conversion_From_T()
        {
            var f = 2.0f;

            SomeResult result = f;

            Assert.True(result.IsT0);

            var v = result.Match(v => v, e => float.MaxValue);

            Assert.Equal(f, v);
        }

        [Fact]
        public void Test_Implicit_Conversion_From_Error()
        {
            var err = SomeError.NotFound;

            SomeResult result = err;

            Assert.True(result.IsT1);

            var v = result.Match(v => (SomeError)int.MaxValue, e => e);

            Assert.Equal(err, v);
        }
    }
}
