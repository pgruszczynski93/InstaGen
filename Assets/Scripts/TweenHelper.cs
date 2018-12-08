using System.Collections;
using UnityEngine;
using System;

namespace InstaGen
{
    public struct TweenParameters
    {
        public Vector2 startPos;
        public Vector2 endPos;
        public AnimationCurve animationCurve;
        public float durationTime;
    }

    public static class TweenHelper
    {
        public static IEnumerator TweenAction(Action<Vector2> onRectTransformChange, TweenParameters parameters)
        {
            float currentTime = 0.0f;
            float animationProgress = 0.0f;
            float curveProgress = 0.0f;

            while (currentTime < parameters.durationTime)
            {
                animationProgress = Mathf.Clamp01(currentTime / parameters.durationTime);
                curveProgress = parameters.animationCurve.Evaluate(animationProgress);

                onRectTransformChange(Vector2.Lerp(parameters.startPos, parameters.endPos, curveProgress));

                currentTime += Time.deltaTime;
                yield return null;
            }

            onRectTransformChange(parameters.endPos);
        }
    }
}
