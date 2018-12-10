using System.Collections;
using UnityEngine;
using System;

namespace InstaGen
{
    public class RectTweenableObject
    { 
        public RectTransform rectTransform;
        public Vector2 propertyVector1;
        public Vector2 propertyVector2;

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
        public RectTransform tweenableRectTransform;
        public RectTweenableObject startPos;
        public RectTweenableObject endPos;
    }

    public class AlphaTweenParameters : TweenBaseParameters
    {
        public CanvasGroup inGroup;
        public CanvasGroup outGroup;
    }

    public static class TweenHelper
    {
        public static readonly Vector2 VectorZero = new Vector2(0, 0);

        static WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        static float currentTime;
        static float animationProgress;
        static float curveProgress;

        public static IEnumerator SkipFrames(int frames)
        {
            for(int i=0; i<frames; i++)
            {
                yield return WaitForEndOfFrame;
            }
            yield return null;
        }

        public static IEnumerator RectTweenAction(Action<RectTweenableObject> onRectTransformChange = null, RectTweenParameters parameters = null)
        {
            currentTime = 0.0f;
            animationProgress = 0.0f;
            curveProgress = 0.0f;

            while (currentTime < parameters.durationTime)
            {
                animationProgress = Mathf.Clamp01(currentTime / parameters.durationTime);
                curveProgress = parameters.animationCurve.Evaluate(animationProgress);

                if(onRectTransformChange != null)
                {
                    onRectTransformChange(new RectTweenableObject(
                        parameters.tweenableRectTransform,
                        Vector2.Lerp(parameters.startPos.propertyVector1, parameters.endPos.propertyVector1, curveProgress),
                        Vector2.Lerp(parameters.startPos.propertyVector2, parameters.endPos.propertyVector2, curveProgress)));
                }


                currentTime += Time.deltaTime;
                yield return null;
            }

            if(onRectTransformChange != null)
            {
                onRectTransformChange(parameters.endPos);
            }
        }

        public static IEnumerator AlphaTweenAction(Action<float> onAlphaChange = null, AlphaTweenParameters parameters = null)
        {
            currentTime = 0.0f;
            animationProgress = 0.0f;
            curveProgress = 0.0f;

            while (currentTime < parameters.durationTime)
            {
                animationProgress = Mathf.Clamp01(currentTime / parameters.durationTime);
                curveProgress = parameters.animationCurve.Evaluate(animationProgress);

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
