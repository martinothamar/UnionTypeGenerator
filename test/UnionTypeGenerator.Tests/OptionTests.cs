using System;
using UnionTypeGenerator.Abstractions;
using Xunit;

namespace UnionTypeGenerator.Tests
{
    public sealed class OptionTests
    {
        [Fact]
        public void Test_Some()
        {
            var opt = Option.Some(1);

            Assert.True(opt.IsSome);
            Assert.False(opt.IsNone);
            var v = opt.Match(n => n, () => 0);

            Assert.Equal(1, v);
        }

        [Fact]
        public void Test_None()
        {
            var opt = Option.None<int>();

            Assert.False(opt.IsSome);
            Assert.True(opt.IsNone);
            var v = opt.Match(_ => throw new Exception("Should not arrive here"), () => 1);

            Assert.Equal(1, v);
        }

        [Fact]
        public void Test_Some2()
        {
            var opt = Option<int>.Some(1);

            Assert.True(opt.IsSome);
            Assert.False(opt.IsNone);
            var v = opt.Match(n => n, () => 0);

            Assert.Equal(1, v);
        }

        [Fact]
        public void Test_None2()
        {
            var opt = Option<int>.None;

            Assert.False(opt.IsSome);
            Assert.True(opt.IsNone);
            var v = opt.Match(_ => throw new Exception("Should not arrive here"), () => 1);

            Assert.Equal(1, v);
        }
    }
}
