using System;
using System.Collections.Generic;

namespace UnionTypeGenerator
{
    //public readonly struct Result
    //{
    //    private readonly IError _error;
    //}

    public readonly struct Result<T, TError> : IUnion<T, TError>, IEquatable<Result<T, TError>>
        where TError : IError
    {
        private readonly T? _result;
        private readonly TError? _error;

        public bool IsT => _result is not null;

        public Result(T result)
        {
            _result = result;
            _error = default;
        }
        public Result(TError error)
        {
            _result = default;
            _error = error;
        }

        public override bool Equals(object? obj) =>
            obj is Result<T> result && Equals(result);

        public bool Equals(Result<T, TError> other) =>
            EqualityComparer<T?>.Default.Equals(_result, other._result) &&
            EqualityComparer<IError?>.Default.Equals(_error, other._error);

        public override int GetHashCode() => HashCode.Combine(_result, _error);

        public TResult Match<TResult>(Func<T, TResult> handleValue, Func<TError, TResult> handleError1)
        {
            if (_result is not null)
                return handleValue(_result);
            else
                return handleError1(_error!);
        }

        public static bool operator ==(Result<T, TError> left, Result<T, TError> right) =>
            left.Equals(right);

        public static bool operator !=(Result<T, TError> left, Result<T, TError> right) =>
            !(left == right);

        public static implicit operator Result<T, TError>(T result) => new Result<T, TError>(result);

        public static implicit operator Result<T, TError>(TError error) => new Result<T, TError>(error);


        public static implicit operator Result<T>(Result<T, TError> result) =>
            result.Match(r => new Result<T>(r), e => new Result<T>(e));
    }

    public readonly struct Result<T> : IUnion<T, IError>, IEquatable<Result<T>>
    {
        private readonly T? _result;
        private readonly IError? _error;

        public bool IsT => _result is not null;

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

        public override bool Equals(object? obj) =>
            obj is Result<T> result && Equals(result);

        public bool Equals(Result<T> other) =>
            EqualityComparer<T?>.Default.Equals(_result, other._result) &&
            EqualityComparer<IError?>.Default.Equals(_error, other._error);

        public override int GetHashCode() => HashCode.Combine(_result, _error);

        public TResult Match<TResult>(Func<T, TResult> handleValue, Func<IError, TResult> handleError1)
        {
            if (_result is not null)
                return handleValue(_result);
            else
                return handleError1(_error!);
        }

        public static bool operator ==(Result<T> left, Result<T> right) =>
            left.Equals(right);

        public static bool operator !=(Result<T> left, Result<T> right) =>
            !(left == right);

        public static implicit operator Result<T>(T result) => new Result<T>(result);

        public static Result<T> Error<TError>(TError error) where TError : IError => new Result<T>(error);
    }
}
