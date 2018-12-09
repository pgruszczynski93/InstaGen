using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace InstaGen
{
    public class InputContentScroller : MonoBehaviour
    {
        const int SCROLL_STEP = 600; //modify when necessary

        [SerializeField] float _scrollTime;

        [SerializeField] AnimationCurve _animationCurve;    
        [SerializeField] RectTransform _scrollableElement;
        [SerializeField] Button _nextButton;
        [SerializeField] Button _previousButton;

        void SetButtons(bool isEnabled)
        {
            _nextButton.gameObject.SetActive(isEnabled);
            _previousButton.gameObject.SetActive(isEnabled);
        }

        public void ScrollToNext()
        {
            if (_scrollableElement == null)
            {
                return;
            }
            StartCoroutine(AnimateScrolling(SwipeDirection.Down));
        }

        public void ScrollToPrevious()
        {
            if (_scrollableElement == null)
            {
                return;
            }
            StartCoroutine(AnimateScrolling(SwipeDirection.Up));
        }

        IEnumerator AnimateScrolling(SwipeDirection direction)
        {
            float scrollDirection = direction == SwipeDirection.Up ? SCROLL_STEP * (-1) : SCROLL_STEP;

            Vector2 scrollAnchoredPos = _scrollableElement.anchoredPosition;
            RectTweenableObject tweenableObjectStart = new RectTweenableObject(_scrollableElement, scrollAnchoredPos, TweenHelper.VectorZero);
            RectTweenableObject tweenableObjectEnd = new RectTweenableObject(_scrollableElement, new Vector2(scrollAnchoredPos.x, scrollAnchoredPos.y + scrollDirection), 
                                                        TweenHelper.VectorZero);

            TweenParameters parameters = new TweenParameters()
            {
                tweenableRectTransform = _scrollableElement,
                startPos = tweenableObjectStart,
                endPos = tweenableObjectEnd,
                animationCurve = _animationCurve,
                durationTime = _scrollTime
            };

            SetButtons(false);

            yield return StartCoroutine(TweenHelper.SimpleTweenAction(
                (tweenableObject) => _scrollableElement.anchoredPosition = tweenableObject.propertyVector1, 
                parameters));

            StopCoroutine(TweenHelper.SimpleTweenAction());

            SetButtons(true);
        }
    }
}
