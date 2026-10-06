using System;
using System.Threading;
using UnityEngine;

namespace ActionCode.AwaitableSystem
{
    /// <summary>
    /// Represents an asynchronous Awaitable operation that can be started and stopped.
    /// </summary>
    public sealed class AwaitableOperation
    {
        private CancellationTokenSource source;

        /// <summary>
        /// Starts the Awaitable operation.
        /// </summary>
        /// <param name="operation">The operation to start.</param>
        /// <param name="token">The cancellation token.</param>
        public void Start(Func<CancellationToken, Awaitable> operation, CancellationToken token) =>
            _ = StartAsync(operation, token);

        /// <summary>
        /// Starts the Awaitable operation asynchronously.
        /// </summary>
        /// <param name="operation">The operation to start.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>An asynchronously started Awaitable operation.</returns>
        public async Awaitable StartAsync(Func<CancellationToken, Awaitable> operation, CancellationToken token)
        {
            source = CancellationTokenSource.CreateLinkedTokenSource(token);

            try { await operation(source.Token); }
            catch (OperationCanceledException) { }
            catch (Exception e) { Debug.LogException(e); }
        }

        /// <summary>
        /// Stops the Awaitable operation.
        /// </summary>
        public void Stop()
        {
            if (source == null) return;

            source.Cancel();
            source.Dispose();
            source = null;
        }
    }
}