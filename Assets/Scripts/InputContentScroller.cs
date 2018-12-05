using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace InstaGen
{
    public class InputContentScroller : MonoBehaviour
    {

        const int SCROLL_STEP = 600; //modify when necessary

        [SerializeField] float _contentMinPositionY;
        [SerializeField] float _contentMaxPositionY;

        [SerializeField] float _scrollDuration;
        [SerializeField] AnimationCurve _animationCurve;
        [SerializeField] RectTransform _scrollableElement;
        [SerializeField] Button _nextButton;
        [SerializeField] Button _previousButton;

        // TO REFACTOR & FINISH
        void Start()
        {
            _contentMinPositionY = _scrollableElement.anchoredPosition.y;
            _contentMaxPositionY = -_contentMinPositionY;
        }

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
            yield return StartCoroutine(ScrollContent(direction));
        }

        IEnumerator ScrollContent(SwipeDirection direction)
        {
            float time = 0.0f;
            Vector2 beginPos = _scrollableElement.anchoredPosition;
            float scrollDirection = direction == SwipeDirection.Up ? SCROLL_STEP * (-1) : SCROLL_STEP;
            Vector2 endPos = new Vector2(beginPos.x, beginPos.y + scrollDirection);
            while (time < _scrollDuration)
            {
                time += Time.deltaTime;
                _scrollableElement.anchoredPosition = Vector2.Lerp(beginPos, endPos, time/_scrollDuration);
                yield return null;
            }
            _scrollableElement.anchoredPosition = new Vector2(0, Mathf.Clamp(_scrollableElement.anchoredPosition.y, _contentMinPositionY, _contentMaxPositionY));
        }
    }
}
