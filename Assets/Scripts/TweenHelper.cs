using System;
using System.Collections;
using UnityEngine;

namespace InstaGen
{
    [Serializable]
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

        public override string ToString()
        {
            return string.Format("1.(x,y) = {0}, 2.(x,y) = {1}", propertyVector1, propertyVector2);
        }
    }

    [Serializable]
    public class TweenBaseParameters
    {
        public AnimationCurve animationCurve;
        public float durationTime;
    }

    [Serializable]
    public class RectTweenParameters : TweenBaseParameters
    {
        public RectTweenableObject endPos;
        public RectTweenableObject startPos;
        public RectTransform tweenableRectTransform;
    }

    [Serializable]
    public class AlphaTweenParameters : TweenBaseParameters
    {
        public CanvasGroup inGroup;
        public CanvasGroup outGroup;
    }

    public static class TweenHelper
    {
        public static readonly Vector2 VectorZero = new Vector2(0, 0);
        public static readonly float AlphaMax = 1.0f;
        public static readonly float AlphaMin = 0.0f;

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

            if (parameters == null)
            {
                yield break;
            }
            
            SetupInitialReferences();
            
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

        public static IEnumerator AlphaTweenAction(Action<float, float> onAlphaChange = null,
            AlphaTweenParameters parameters = null)
        {
            SetupInitialReferences();

            while (currentTime < parameters.durationTime)
            {
                SetupTweenProperties(parameters.durationTime, parameters.animationCurve);

                if (onAlphaChange != null)
                {
                    onAlphaChange(
                        Mathf.Lerp(parameters.inGroup.alpha, AlphaMin, curveProgress),
                        Mathf.Lerp(parameters.outGroup.alpha, AlphaMax, curveProgress));
                }


                currentTime += Time.deltaTime;
                yield return null;
            }

            if (onAlphaChange != null)
            {
                onAlphaChange(Mathf.Clamp01(parameters.inGroup.alpha), Mathf.Clamp01(parameters.outGroup.alpha));
            }
        }
    }
}