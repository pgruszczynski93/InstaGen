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

    public class TweenParameters
    {
        public RectTransform tweenableRectTransform;
        public RectTweenableObject startPos;
        public RectTweenableObject endPos;
        public AnimationCurve animationCurve;
        public float durationTime;
    }

    public static class TweenHelper
    {
        public static readonly Vector2 VectorZero = new Vector2(0, 0);

        static WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();

        public static IEnumerator SkipFrames(int frames)
        {
            for(int i=0; i<frames; i++)
            {
                yield return WaitForEndOfFrame;
            }
            yield return null;
        }

        public static IEnumerator SimpleTweenAction(Action<RectTweenableObject> onRectTransformChange = null, TweenParameters parameters = null)
        {
            float currentTime = 0.0f;
            float animationProgress = 0.0f;
            float curveProgress = 0.0f;

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

        //public static IEnumerator ArrayTweenAction(Action<Vector2x2, RectTransform> onRectTransformChange = null, RectTransform panel = null, TweenParameters parameters = null)
        //{
        //    float currentTime = 0.0f;
        //    float animationProgress = 0.0f;
        //    float curveProgress = 0.0f;

        //    while (currentTime < parameters.durationTime)
        //    {
        //        animationProgress = Mathf.Clamp01(currentTime / parameters.durationTime);
        //        curveProgress = parameters.animationCurve.Evaluate(animationProgress);

        //        if (onRectTransformChange != null)
        //        {
        //            onRectTransformChange(new Vector2x2(
        //                Vector2.Lerp(parameters.startPos.vectorRow1, parameters.endPos.vectorRow1, curveProgress),
        //                Vector2.Lerp(parameters.startPos.vectorRow2, parameters.endPos.vectorRow2, curveProgress)), panel);
        //        }


        //        currentTime += Time.deltaTime;
        //        yield return null;
        //    }

        //    if (onRectTransformChange != null)
        //    {
        //        onRectTransformChange(parameters.endPos, panel);
        //    }
        //}
    }
}
