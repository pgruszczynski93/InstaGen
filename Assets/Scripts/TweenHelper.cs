using System.Collections;
using UnityEngine;
using System;

namespace InstaGen
{
    public class Vector2x2
    {
        public Vector2 vectorRow1;
        public Vector2 vectorRow2;

        public Vector2x2(Vector2 vectorRow1, Vector2 vectorRow2)
        {
            this.vectorRow1 = vectorRow1;
            this.vectorRow2 = vectorRow2;
        }
    }

    public class TweenParameters
    {
        public Vector2x2 startPos;
        public Vector2x2 endPos;
        public AnimationCurve animationCurve;
        public float durationTime;
    }

    public static class TweenHelper
    {
        public static readonly Vector2 VectorZero = new Vector2(0, 0);

        public static IEnumerator TweenAction2D(Action<Vector2x2> onRectTransformChange = null, TweenParameters parameters = null)
        {
            float currentTime = 0.0f;
            float animationProgress = 0.0f;
            float curveProgress = 0.0f;

            while (currentTime < parameters.durationTime)
            {
                animationProgress = Mathf.Clamp01(currentTime / parameters.durationTime);
                curveProgress = parameters.animationCurve.Evaluate(animationProgress);

                onRectTransformChange(new Vector2x2(
                    Vector2.Lerp(parameters.startPos.vectorRow1, parameters.endPos.vectorRow1, curveProgress),
                    Vector2.Lerp(parameters.startPos.vectorRow2, parameters.endPos.vectorRow2, curveProgress)));

                currentTime += Time.deltaTime;
                yield return null;
            }

            onRectTransformChange(parameters.endPos);
        }
    }
}
