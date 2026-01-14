using System;
using UnityEngine;

namespace ActionCode.AwaitableSystem
{
    /// <summary>
    /// Awaitable extension class for <see cref="MonoBehaviour"/>.
    /// </summary>
    public static class MonoBehaviourExtension
    {
        /// <summary>
        /// Resumes execution after the specified number of seconds, even if the MonoBehaviour is destroyed.
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="time">Seconds to wait for.</param>
        /// <returns>An asynchronous operation.</returns>
        public static async Awaitable WaitForSecondsAsync(this MonoBehaviour mono, float time)
        {
            try
            {
                await Awaitable.WaitForSecondsAsync(time, mono.destroyCancellationToken);
            }
            catch (OperationCanceledException)
            {
                // GameObject may be destroyed by gameplay. Don't show Exception.
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}