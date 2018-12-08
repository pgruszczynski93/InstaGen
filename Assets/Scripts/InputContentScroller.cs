using System.Collections;
using System.Collections.Generic;
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

        public void ScrollToNext()
        {
            StartCoroutine(AnimateScrolling(SwipeDirection.Down));
        }

        public void ScrollToPrevious()
        {
            StartCoroutine(AnimateScrolling(SwipeDirection.Up));
        }

        IEnumerator AnimateScrolling(SwipeDirection direction)
        {
            float scrollDirection = direction == SwipeDirection.Up ? SCROLL_STEP * (-1) : SCROLL_STEP;
            Vector2 scrollAnchoredPos = _scrollableElement.anchoredPosition;

            TweenParameters parameters = new TweenParameters()
            {
                startPos = scrollAnchoredPos,
                endPos = new Vector2(scrollAnchoredPos.x, scrollAnchoredPos.y + scrollDirection),
                animationCurve = _animationCurve,
                durationTime = _scrollTime
            };

            yield return StartCoroutine(TweenHelper.TweenAction(
                (newAnchoredPos) => _scrollableElement.anchoredPosition = newAnchoredPos, 
                parameters));
        }
    }
}
