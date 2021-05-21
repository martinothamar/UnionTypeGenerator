using System;

namespace UnionTypeGenerator
{
    public interface IBaseUnion { }

    public interface IUnion<T> : IBaseUnion
    {
    }

    public interface IUnion<T, TError> : IUnion<T>
    {
        /// <summary>
        /// Handle the contents of the Result, map it into a TResult value.
        /// </summary>
        /// <typeparam name="TResult">Return value type</typeparam>
        /// <param name="handleValue">Function for handling the T value</param>
        /// <param name="handleError1">Function for handling the error</param>
        /// <returns>The TResult value</returns>
        TResult Match<TResult>(Func<T, TResult> handleValue, Func<TError, TResult> handleError1);
    }
}
