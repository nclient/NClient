using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace NClient.Core.Castle
{
    // TODO: It's fixed copy (see InterceptSynchronousResult). Fix it in https://github.com/JSkimming/Castle.Core.AsyncInterceptor

    /// <summary>
    /// A base type for an <see cref="IAsyncInterceptor"/> to provided a simplified solution of method
    /// <see cref="IInvocation"/> by enforcing only two types of interception, both asynchronous.
    /// </summary>
    [SuppressMessage(
        "Design",
        "CA1031:Do not catch general exception types",
        Justification = "Must propagate the same exceptions.")]
    internal abstract class AsyncInterceptorBase : IAsyncInterceptor
    {
#if !NETSTANDARD2_0 && !NET5_0
        /// <summary>
        /// A completed <see cref="Task"/>.
        /// </summary>
        private static readonly Task CompletedTask = Task.FromResult(0);
#endif

        private static readonly MethodInfo InterceptSynchronousMethodInfo =
            typeof(AsyncInterceptorBase).GetMethod(
                nameof(InterceptSynchronousResult), BindingFlags.Static | BindingFlags.NonPublic)!;

        private static readonly ConcurrentDictionary<Type, GenericSynchronousHandler> GenericSynchronousHandlers =
            new ConcurrentDictionary<Type, GenericSynchronousHandler>
            {
                [typeof(void)] = InterceptSynchronousVoid,
            };

        private delegate void GenericSynchronousHandler(AsyncInterceptorBase me, IInvocation invocation);

        /// <summary>
        /// Intercepts a synchronous method <paramref name="invocation"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptSynchronous(IInvocation invocation)
        {
            Type returnType = invocation.Method.ReturnType;
            GenericSynchronousHandler handler = GenericSynchronousHandlers.GetOrAdd(returnType, CreateHandler);
            handler(this, invocation);
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task"/>.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InterceptAsync(invocation, invocation.CaptureProceedInfo(), ProceedAsynchronous);
        }

        /// <summary>
        /// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue =
                InterceptAsync(invocation, invocation.CaptureProceedInfo(), ProceedAsynchronous<TResult>);
        }

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="proceedInfo">The <see cref="IInvocationProceedInfo"/>.</param>
        /// <param name="proceed">The function to proceed the <paramref name="proceedInfo"/>.</param>
        /// <returns>A <see cref="Task" /> object that represents the asynchronous operation.</returns>
        protected abstract Task InterceptAsync(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, Task> proceed);

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="proceedInfo">The <see cref="IInvocationProceedInfo"/>.</param>
        /// <param name="proceed">The function to proceed the <paramref name="proceedInfo"/>.</param>
        /// <returns>A <see cref="Task" /> object that represents the asynchronous operation.</returns>
        protected abstract Task<TResult> InterceptAsync<TResult>(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed);

        private static GenericSynchronousHandler CreateHandler(Type returnType)
        {
            MethodInfo method = InterceptSynchronousMethodInfo.MakeGenericMethod(returnType);
            return (GenericSynchronousHandler)method.CreateDelegate(typeof(GenericSynchronousHandler));
        }

        private static void InterceptSynchronousVoid(AsyncInterceptorBase me, IInvocation invocation)
        {
            Task task = me.InterceptAsync(invocation, invocation.CaptureProceedInfo(), ProceedSynchronous);

            // If the intercept task has yet to complete, wait for it.
            if (!task.IsCompleted)
            {
                // Need to use Task.Run() to prevent deadlock in .NET Framework ASP.NET requests.
                // GetAwaiter().GetResult() prevents a thrown exception being wrapped in a AggregateException.
                // See https://stackoverflow.com/a/17284612
                Task.Run(() => task).GetAwaiter().GetResult();
            }

            task.RethrowIfFaulted();
        }

        private static void InterceptSynchronousResult<TResult>(AsyncInterceptorBase me, IInvocation invocation)
        {
            Task<TResult> task = me.InterceptAsync(invocation, invocation.CaptureProceedInfo(), ProceedSynchronous<TResult>);

            // If the intercept task has yet to complete, wait for it.
            if (!task.IsCompleted)
            {
                // Need to use Task.Run() to prevent deadlock in .NET Framework ASP.NET requests.
                // GetAwaiter().GetResult() prevents a thrown exception being wrapped in a AggregateException.
                // See https://stackoverflow.com/a/17284612
                Task.Run(() => task).GetAwaiter().GetResult();
            }

            task.RethrowIfFaulted();

            //NOTE: My fix
            invocation.ReturnValue = task.Result;
        }

        private static Task ProceedSynchronous(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            try
            {
                proceedInfo.Invoke();
#if NETSTANDARD2_0 || NET5_0
                return Task.CompletedTask;
#else
                return CompletedTask;
#endif
            }
            catch (Exception e)
            {
#if NETSTANDARD2_0 || NET5_0
                return Task.FromException(e);
#else
                var tcs = new TaskCompletionSource<int>();
                tcs.SetException(e);
                return tcs.Task;
#endif
            }
        }

        private static Task<TResult> ProceedSynchronous<TResult>(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo)
        {
            try
            {
                proceedInfo.Invoke();
                return Task.FromResult((TResult)invocation.ReturnValue);
            }
            catch (Exception e)
            {
#if NETSTANDARD2_0 || NET5_0
                return Task.FromException<TResult>(e);
#else
                var tcs = new TaskCompletionSource<TResult>();
                tcs.SetException(e);
                return tcs.Task;
#endif
            }
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "The name explicitly says Asynchronous.")]
        private static async Task ProceedAsynchronous(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            proceedInfo.Invoke();

            // Get the task to await.
            var originalReturnValue = (Task)invocation.ReturnValue;

            await originalReturnValue.ConfigureAwait(false);
        }

        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "The name explicitly says Asynchronous.")]
        private static async Task<TResult> ProceedAsynchronous<TResult>(
            IInvocation invocation,
            IInvocationProceedInfo proceedInfo)
        {
            proceedInfo.Invoke();

            // Get the task to await.
            var originalReturnValue = (Task<TResult>)invocation.ReturnValue;

            TResult result = await originalReturnValue.ConfigureAwait(false);
            return result;
        }
    }

    /// <summary>
    /// A helper class to re-throw exceptions and retain the stack trace.
    /// </summary>
    internal static class RethrowHelper
    {
        /// <summary>
        /// Re-throws the supplied exception without losing its stack trace.
        /// Prefer <c>throw;</c> where possible, this method is useful for re-throwing
        /// <see cref="Exception.InnerException" /> which cannot be done with the <c>throw;</c> semantics.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void Rethrow(this Exception? exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            ExceptionDispatchInfo.Capture(exception).Throw();
        }

        /// <summary>
        /// If the <paramref name="exception"/> is an <see cref="AggregateException"/> the
        /// <paramref name="exception"/>.<see cref="Exception.InnerException"/> is re-thrown; otherwise the
        /// <paramref name="exception"/> is re-thrown.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void RethrowInnerIfAggregate(this Exception? exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            switch (exception)
            {
                case AggregateException aggregate:
                    Rethrow(aggregate.InnerException);
                    break;
                default:
                    Rethrow(exception);
                    break;
            }
        }

        /// <summary>
        /// If the <paramref name="task"/> <see cref="Task.IsFaulted"/> the inner exception is re-thrown; otherwise the
        /// method is a no-op.
        /// </summary>
        /// <param name="task">The task.</param>
        public static void RethrowIfFaulted(this Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (task.IsFaulted)
                RethrowInnerIfAggregate(task.Exception);
        }
    }
}
