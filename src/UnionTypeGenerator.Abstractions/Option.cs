using System;
using System.Runtime.CompilerServices;

namespace UnionTypeGenerator.Abstractions
{
    public interface IOption
    {

    }

    public static class Option
    {
        public static Option<T> Some<T>(T value)
            where T : notnull
        {
            return new Option<T>(value);
        }

        public static Option<T> None<T>()
            where T : notnull
        {
            return new Option<T>(default(int?));
        }
    }

    public readonly struct Option<T>
        where T : notnull
    {
        private const int NONE = 0;
        private const int SOME = 1;

        private readonly T _value;
        private readonly int _state;

        public static readonly Option<T> None = new Option<T>(default(int?));

        public static Option<T> Some(T value) => new Option<T>(value);

        public bool IsNone => _state == NONE;

        public bool IsSome => _state == SOME;

        internal Option(T value)
        {
            _value = value;
            _state = 1;
        }

        internal Option(int? _)
        {
            Unsafe.SkipInit(out this);
            _state = 0;
        }

        public TResult Match<TResult>(Func<T, TResult> handleSome, Func<TResult> handleNone)
        {
            return _state switch
            {
                1 => handleSome(_value),
                0 => handleNone(),
                _ => ThrowInvalidState<TResult>(),
            };
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static TResult ThrowInvalidState<TResult>() =>
            throw new InvalidProgramException("Reached an invalid state");
    }
}
