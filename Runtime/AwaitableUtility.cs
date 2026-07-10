using System;
using System.Threading;
using UnityEngine;

namespace ActionCode.AwaitableSystem
{
    /// <summary>
    /// Utility class for <see cref="Awaitable"/>.
    /// </summary>
    public static class AwaitableUtility
    {
        /// <summary>
        /// Waits asynchronously until the given condition is true.
        /// </summary>
        /// <param name="condition">A boolean condition.</param>
        /// <returns>An asynchronously operation.</returns>
        public static async Awaitable WaitUntilAsync(Func<bool> condition)
        {
            while (!condition()) await Awaitable.NextFrameAsync();
        }

        /// <summary>
        /// Waits asynchronously until the given condition is false.
        /// </summary>
        /// <param name="condition">A boolean condition.</param>
        /// <returns>An asynchronously operation.</returns>
        public static async Awaitable WaitWhileAsync(Func<bool> condition)
        {
            while (condition()) await Awaitable.NextFrameAsync();
        }

        /// <summary>
        /// Resumes execution after the specified number of seconds in real time, independent from <see cref="Time.timeScale"/>.
        /// </summary>
        /// <param name="time">Seconds to wait for.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns>An asynchronously operation.</returns>
        public static async Awaitable WaitForSecondsRealtimeAsync(float time, CancellationToken token = default)
        {
            var currentTime = 0f;
            while (currentTime < time)
            {
                if (token.IsCancellationRequested) return;

                await Awaitable.NextFrameAsync();
                currentTime += Time.unscaledDeltaTime;
            }
        }

        /// <summary>
        /// Waits asynchronously until the given animation is playing.
        /// </summary>
        /// <param name="animation">The playing animation.</param>
        /// <returns>An asynchronously operation.</returns>
        public static async Awaitable WaitWhilePlayingAsync(Animation animation) =>
            await WaitWhileAsync(() => animation && animation.isPlaying);

        /// <summary>
        /// Waits asynchronously for the given number of <paramref name="frames"/>
        /// </summary>
        /// <param name="frames">The frames to wait for.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns>An asynchronously operation.</returns>
        public static async Awaitable WaitForFramesAsync(uint frames, CancellationToken token = default)
        {
            uint current = 0;
            while (current++ < frames)
            {
                if (token.IsCancellationRequested) return;
                await Awaitable.NextFrameAsync();
            }
        }

        /// <summary>
        /// Linearly interpolates between <paramref name="start"/> and <paramref name="final"/> by <paramref name="duration"/>.
        /// </summary>
        /// <param name="start">The start value.</param>
        /// <param name="final">The final value.</param>
        /// <param name="duration">The entire interpolation duration.</param>
        /// <param name="getValue">The update function. Use it to get the interpolation value.</param>
        /// <param name="speed">The interpolation speed.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns>An asynchronously lerp operation.</returns>
        public static async Awaitable LerpAsync(float start, float final, float duration, Action<float> getValue, float speed = 1F, CancellationToken token = default) =>
            await InterpolateAsync(duration, getValue, value => Mathf.Lerp(start, final, value), speed, token);

        /// <summary>
        /// Linearly interpolates between <paramref name="start"/> and <paramref name="final"/> colors by <paramref name="duration"/>.
        /// </summary>
        /// <param name="start">The start color.</param>
        /// <param name="final">The final color.</param>
        /// <param name="duration">The entire color interpolation duration.</param>
        /// <param name="getValue">The update function. Use it to get the interpolation color.</param>
        /// <param name="speed">The color interpolation speed.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns>An asynchronously color lerp operation.</returns>
        public static async Awaitable LerpAsync(Color start, Color final, float duration, Action<Color> getValue, float speed = 1F, CancellationToken token = default) =>
            await InterpolateAsync(duration, getValue, value => Color.Lerp(start, final, value), speed, token);

        /// <summary>
        /// Linearly interpolates using <paramref name="setValue"/> method by <paramref name="duration"/>.
        /// </summary>
        /// <typeparam name="T">The interpolation type.</typeparam>
        /// <param name="duration">The entire interpolation duration.</param>
        /// <param name="getValue">The get value function. Use it to get the interpolation value.</param>
        /// <param name="setValue">The set value function. Use it to set the interpolation value.</param>
        /// <param name="speed">The interpolation speed.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns>An asynchronously interpolation operation.</returns>
        public static async Awaitable InterpolateAsync<T>(
            float duration,
            Action<T> getValue,
            Func<float, T> setValue,
            float speed,
            CancellationToken token = default
        )
        {
            var currentTime = 0F;
            while (currentTime < duration)
            {
                if (token.IsCancellationRequested) return;

                var step = currentTime / duration;
                var value = setValue(step);

                getValue?.Invoke(value);
                currentTime += Time.unscaledDeltaTime * speed;

                await Awaitable.NextFrameAsync();
            }
            getValue?.Invoke(setValue(1F));
        }

        /// <summary>
        /// Plays the given animation curve.
        /// </summary>
        /// <param name="curve">The animation curve to play.</param>
        /// <param name="getValue">The get value function. Use it to get the Animation Curve value.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns>An asynchronously operation.</returns>
        public static async Awaitable PlayAnimationCurveAsync(AnimationCurve curve, Action<float> getValue, CancellationToken token = default)
        {
            var elapsed = 0f;
            var totalTime = curve[curve.length - 1].time;

            while (elapsed < totalTime)
            {
                if (token.IsCancellationRequested) return;

                elapsed += Time.deltaTime;

                var curveValue = curve.Evaluate(elapsed);
                getValue?.Invoke(curveValue);

                await Awaitable.NextFrameAsync(token);
            }
        }
    }
}