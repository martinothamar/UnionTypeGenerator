using System;

namespace UnionTypeGenerator
{
    public interface IBaseUnion { }

    public interface IUnion<T> : IBaseUnion
        where T : unmanaged
    {
        public bool IsT0 { get; }
    }

    public interface IUnion<T0, T1> : IUnion<T0>
        where T0 : unmanaged
        where T1 : unmanaged
    {
        public bool IsT1 { get; }

        /// <summary>
        /// Handle the contents of the Result, map it into a TResult value.
        /// </summary>
        /// <typeparam name="TResult">Return value type</typeparam>
        /// <param name="handleValue0">Function for handling the T0 value</param>
        /// <param name="handleValue1">Function for handling the T1 value</param>
        /// <returns>The TResult value</returns>
        TResult Match<TResult>(Func<T0, TResult> handleValue0, Func<T1, TResult> handleValue1);
    }

    public interface IUnion<T0, T1, T2>
        where T0 : unmanaged
        where T1 : unmanaged
        where T2 : unmanaged
    {
        public bool IsT1 { get; }

        public bool IsT2 { get; }

        /// <summary>
        /// Handle the contents of the Result, map it into a TResult value.
        /// </summary>
        /// <typeparam name="TResult">Return value type</typeparam>
        /// <param name="handleValue0">Function for handling the T0 value</param>
        /// <param name="handleValue1">Function for handling the T1 value</param>
        /// <param name="handleValue2">Function for handling the T2 value</param>
        /// <returns>The TResult value</returns>
        TResult Match<TResult>(Func<T0, TResult> handleValue0, Func<T1, TResult> handleValue1, Func<T2, TResult> handleValue2);
    }

    public interface IUnion<T0, T1, T2, T3>
        where T0 : unmanaged
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
    {
        public bool IsT1 { get; }

        public bool IsT2 { get; }

        public bool IsT3 { get; }

        /// <summary>
        /// Handle the contents of the Result, map it into a TResult value.
        /// </summary>
        /// <typeparam name="TResult">Return value type</typeparam>
        /// <param name="handleValue0">Function for handling the T0 value</param>
        /// <param name="handleValue1">Function for handling the T1 value</param>
        /// <param name="handleValue2">Function for handling the T2 value</param>
        /// <param name="handleValue3">Function for handling the T3 value</param>
        /// <returns>The TResult value</returns>
        TResult Match<TResult>(
            Func<T0, TResult> handleValue0,
            Func<T1, TResult> handleValue1,
            Func<T2, TResult> handleValue2,
            Func<T3, TResult> handleValue3
        );
    }
}
