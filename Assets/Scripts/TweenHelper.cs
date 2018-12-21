using System;
using System.Collections;
using UnityEngine;

namespace InstaGen
{
    public class RectTweenableObject
    {
        public Vector2 propertyVector1;
        public Vector2 propertyVector2;
        public RectTransform rectTransform;

        public RectTweenableObject(RectTransform rectTransform, Vector2 propertyVector1, Vector2 propertyVector2)
        {
            this.rectTransform = rectTransform;
            this.propertyVector1 = propertyVector1;
            this.propertyVector2 = propertyVector2;
        }
    }

    public class TweenBaseParameters
    {
        public AnimationCurve animationCurve;
        public float durationTime;
    }

    public class RectTweenParameters : TweenBaseParameters
    {
        public RectTweenableObject endPos;
        public RectTweenableObject startPos;
        public RectTransform tweenableRectTransform;
    }

    public class AlphaTweenParameters : TweenBaseParameters
    {
        public CanvasGroup inGroup;
        public CanvasGroup outGroup;
    }

    public static class TweenHelper
    {
        public static readonly Vector2 VectorZero = new Vector2(0, 0);

        private static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        private static float currentTime;
        private static float animationProgress;
        private static float curveProgress;

        public static IEnumerator SkipFrames(int frames)
        {
            for (int i = 0; i < frames; i++) yield return WaitForEndOfFrame;
            yield return null;
        }

        private static void SetupInitialReferences()
        {
            currentTime = 0.0f;
            animationProgress = 0.0f;
            curveProgress = 0.0f;
        }

        private static void SetupTweenProperties(float tweenDuration, AnimationCurve animCurve)
        {
            animationProgress = Mathf.Clamp01(currentTime / tweenDuration);
            curveProgress =  animCurve.Evaluate(animationProgress);
        }

        public static IEnumerator RectTweenAction(Action<RectTweenableObject> onRectTransformChange = null,
            RectTweenParameters parameters = null)
        {
            SetupInitialReferences();

            if (parameters == null) yield break;

            while (currentTime < parameters.durationTime)
            {
                SetupTweenProperties(parameters.durationTime, parameters.animationCurve);

                if (onRectTransformChange != null)
                {
                    onRectTransformChange(new RectTweenableObject(
                        parameters.tweenableRectTransform,
                        Vector2.Lerp(parameters.startPos.propertyVector1, parameters.endPos.propertyVector1,
                            curveProgress),
                        Vector2.Lerp(parameters.startPos.propertyVector2, parameters.endPos.propertyVector2,
                            curveProgress)));

                }

                currentTime += Time.deltaTime;
                yield return null;
            }

            if (onRectTransformChange != null)
            {
                onRectTransformChange(parameters.endPos);
            }
        }

        public static IEnumerator AlphaTweenAction(Action<float> onAlphaChange = null,
            AlphaTweenParameters parameters = null)
        {
            SetupInitialReferences();

            while (currentTime < parameters.durationTime)
            {
                SetupTweenProperties(parameters.durationTime, parameters.animationCurve);

                if (onAlphaChange != null)
                {
                    //onAlphaChange();
                }


                currentTime += Time.deltaTime;
                yield return null;
            }

            if (onAlphaChange != null)
            {
                //onAlphaChange();
            }
        }
    }
}