using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnionTypeGenerator
{
    public interface IResult
    {
        public bool IsOk { get; }

        public bool IsError { get; }
    }

    public readonly struct Result : IResult, IEquatable<Result>
    {
        private readonly IError _error;

        public bool IsOk => _error == default;
        public bool IsError => _error != default;

        internal Result(IError error)
        {
            _error = error;
        }

        public override bool Equals(object? obj) => obj is Result result && Equals(result);

        public bool Equals(Result other) =>
            EqualityComparer<IError>.Default.Equals(_error, other._error);

        public override int GetHashCode() => _error?.GetHashCode() ?? 0;

        public static bool operator ==(Result left, Result right) =>
            left.Equals(right);

        public static bool operator !=(Result left, Result right) =>
            !(left == right);

        public static Result Ok() => default;

        public static Result Error(IError error)
        {
            if (error is null)
            {
                ThrowInvalidErrorType();
                return default; // Dead code
            }

            return new Result(error);
        }

        public static Result<T> Ok<T>(T value)
            where T : notnull
        {
            if (value is IError)
                ThrowInvalidTypeToOkConstructor();

            return new Result<T>(value);
        }

        public static Result<T> Error<T>(IError error) where T : notnull =>
            new Result<T>(error);

        public static Result<T, TError> Ok<T, TError>(T value)
            where TError : IError
            where T : notnull
        {
            if (value is IError)
                ThrowInvalidTypeToOkConstructor();

            return new Result<T, TError>(value);
        }

        public static Result<T> Error<T, TError>(TError error)
            where TError : IError
            where T : notnull
        {
            return new Result<T>(error);
        }

        private static void ThrowInvalidErrorType() =>
                throw new ArgumentException("Can't create Error result from null", "error");

        private static void ThrowInvalidTypeToOkConstructor() =>
                throw new ArgumentException("Can't create OK result from error type", "value");
    }

    public readonly struct Result<T, TError> : IResult, IEquatable<Result<T, TError>>
        where TError : IError
        where T : notnull
    {
        private readonly T _result;
        private readonly TError _error;
        private readonly int _tag;

        public bool IsT0 => _tag == 1;
        public bool IsT1 => _tag == 2;
        public bool IsOk => _tag == 1;
        public bool IsError => _tag == 2;

        internal Result(T result)
        {
            Unsafe.SkipInit(out this);
            _result = result;
            _tag = 1;
        }

        internal Result(TError error)
        {
            Unsafe.SkipInit(out this);
            _error = error;
            _tag = 2;
        }

        public override bool Equals(object? obj) =>
            obj is Result<T> result && Equals(result);

        public bool Equals(Result<T, TError> other) =>
            EqualityComparer<T?>.Default.Equals(_result, other._result) &&
            EqualityComparer<IError?>.Default.Equals(_error, other._error);

        public override int GetHashCode() => HashCode.Combine(_result, _error);

        public TResult Match<TResult>(Func<T, TResult> handleValue, Func<TError, TResult> handleError1)
        {
            if (_tag == 1)
                return handleValue(_result);
            else
                return handleError1(_error);
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

    public readonly struct Result<T> : IResult, IEquatable<Result<T>>
        where T : notnull
    {
        private readonly T _result;
        private readonly IError _error;
        private readonly int _tag;

        public bool IsT0 => _tag == 1;
        public bool IsT1 => _tag == 2;
        public bool IsOk => _tag == 1;
        public bool IsError => _tag == 2;

        internal Result(T result)
        {
            Unsafe.SkipInit(out this);
            _result = result;
            _tag = 1;
        }

        internal Result(IError error)
        {
            Unsafe.SkipInit(out this);
            _error = error;
            _tag = 2;
        }

        public override bool Equals(object? obj) =>
            obj is Result<T> result && Equals(result);

        public bool Equals(Result<T> other) =>
            EqualityComparer<T?>.Default.Equals(_result, other._result) &&
            EqualityComparer<IError?>.Default.Equals(_error, other._error);

        public override int GetHashCode() => HashCode.Combine(_result, _error);

        public TResult Match<TResult>(Func<T, TResult> handleValue, Func<IError, TResult> handleError)
        {
            if (_tag == 1)
                return handleValue(_result);
            else
                return handleError(_error);
        }

        public static bool operator ==(Result<T> left, Result<T> right) =>
            left.Equals(right);

        public static bool operator !=(Result<T> left, Result<T> right) =>
            !(left == right);

        public static implicit operator Result<T>(T result) => new Result<T>(result);

        public static Result<T> Error<TError>(TError error) where TError : IError => new Result<T>(error);
    }
}
