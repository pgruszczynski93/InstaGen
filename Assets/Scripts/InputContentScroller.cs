using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine;

namespace InstaGen
{
    public class InputContentScroller : MonoBehaviour
    {
        const int SCROLL_STEP = 600; //modify when necessary

        [SerializeField] float _scrollTime;
        [SerializeField] bool _isScrollingPossible;

        [SerializeField] AnimationCurve _animationCurve;    
        [SerializeField] RectTransform _scrollableElement;
        [SerializeField] NoDragScrollRect _scrollRect;
        [SerializeField] CanvasGroup _rotatorInputPanel;
        [SerializeField] CanvasGroup _normalInputPanel;

        [SerializeField] Button _nextButton;
        [SerializeField] Button _previousButton;

        void SetButtons(bool isEnabled)
        {
            _nextButton.gameObject.SetActive(isEnabled);
            _previousButton.gameObject.SetActive(isEnabled);
        }

        void OnEnable()
        {
            _scrollRect.onValueChanged.AddListener((bounds) => TryToEnableScrollButtons(bounds));
        }

        void OnDisable()
        {
            _scrollRect.onValueChanged.RemoveListener((bounds) => TryToEnableScrollButtons(bounds));
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

            RectTweenParameters parameters = new RectTweenParameters()
            {
                tweenableRectTransform = _scrollableElement,
                startPos = tweenableObjectStart,
                endPos = tweenableObjectEnd,
                animationCurve = _animationCurve,
                durationTime = _scrollTime
            };

            print(_scrollRect.normalizedPosition.y == 0 ? "dol" : _scrollRect.normalizedPosition.y == 1 ? "gora " : "jupi");

            if (_isScrollingPossible)
            {
                yield return StartCoroutine(TweenHelper.RectTweenAction(
                    (tweenableObject) => _scrollableElement.anchoredPosition = tweenableObject.propertyVector1,
                    parameters));
            }

            StopCoroutine(TweenHelper.RectTweenAction());
        }

        void TryToEnableScrollButtons(Vector2 bounds)
        {
            float currY = bounds.y;
            print("curry" + currY);
            _nextButton.gameObject.SetActive(currY > 0 && currY <= 1);
            _previousButton.gameObject.SetActive(currY >= 0 && currY < 1);
        }
    }
}
