using Xunit;

namespace UnionTypeGenerator.Tests
{
    public sealed class ResultGenericErrorTypeArgumentTests
    {
        [Fact]
        public void Test_Result_Ok_Match()
        {
            var result = Result.Ok<int, ValidationError>(3);

            var v = result.Match(v => v, e => int.MaxValue);

            Assert.Equal(3, v);
        }

        [Fact]
        public void Test_Result_Ok_Implicit_Cast()
        {
            Result<int, ValidationError> result = 3;

            var v = result.Match(v => v, e => int.MaxValue);

            Assert.Equal(3, v);
        }

        [Fact]
        public void Test_Result_Error_Implicit_Cast()
        {
            var appError = new ValidationError("Thing");
            Result<int, ValidationError> result = appError;

            var v = result.Match(v => null!, e => e);

            Assert.Equal(appError, v);
        }

        [Fact]
        public void Test_Result_Error_Match()
        {
            var appError = new ValidationError("Thingy");
            var result = Result.Error<int, ValidationError>(appError);

            var v = result.Match(v => null!, e => e);

            Assert.True(v is ValidationError);
            Assert.Equal(appError, (ValidationError)v);
        }
    }
}
