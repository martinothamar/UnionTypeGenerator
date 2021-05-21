using Xunit;

namespace UnionTypeGenerator.Tests
{
    public record ApplicationError : IError;

    public sealed record ValidationError(string Field) : ApplicationError;

    public sealed class ResultGenericOkTypeArgumentTests
    {
        [Fact]
        public void Test_Result_Ok_Match()
        {
            var result = Result.Ok(3);

            var v = result.Match(v => v, e => int.MaxValue);

            Assert.Equal(3, v);
        }

        [Fact]
        public void Test_Result_Ok_Implicit_Cast()
        {
            Result<int> result = 3;

            var v = result.Match(v => v, e => int.MaxValue);

            Assert.Equal(3, v);
        }

        [Fact]
        public void Test_Result_Error_Match()
        {
            var appError = new ValidationError("Thingy");
            var result = Result.Error<int>(appError);

            var v = result.Match(v => null!, e => e);

            Assert.True(v is ValidationError);
            Assert.Equal(appError, (ValidationError)v);
        }
    }
}
