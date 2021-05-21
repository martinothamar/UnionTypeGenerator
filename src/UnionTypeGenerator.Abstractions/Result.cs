using System;

namespace UnionTypeGenerator
{
    public readonly struct Result<T> : IUnion<T, IError>
    {
        private readonly T? _result;
        private readonly IError? _error;

        public Result(T result)
        {
            _result = result;
            _error = null;
        }
        public Result(IError error)
        {
            _result = default;
            _error = error;
        }

        public TResult Match<TResult>(Func<T, TResult> handleValue, Func<IError, TResult> handleError1)
        {
            if (_result is not null)
                return handleValue(_result);
            else
                return handleError1(_error!);
        }
    }
}
