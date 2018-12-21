using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace InstaGen
{
    public class InputContentScroller : MonoBehaviour
    {
        private const int SCROLL_STEP = 600; //modify when necessary

        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private bool _isScrollingPossible;

        [SerializeField] private Button _nextButton;
        [SerializeField] private CanvasGroup _normalInputPanel;
        [SerializeField] private Button _previousButton;
        [SerializeField] private CanvasGroup _rotatorInputPanel;
        [SerializeField] private RectTransform _scrollableElement;
        [SerializeField] private NoDragScrollRect _scrollRect;

        [SerializeField] private float _scrollTime;

        private void SetButtons(bool isEnabled)
        {
            if (_nextButton == null || _previousButton == null)
            {
                return;
            }
            
            _nextButton.gameObject.SetActive(isEnabled);
            _previousButton.gameObject.SetActive(isEnabled);
        }

        private void OnEnable()
        {
            if (_scrollRect == null)
            {
                return;
            }
            
            _scrollRect.onValueChanged.AddListener(bounds => TryToEnableScrollButtons(bounds));
        }

        private void OnDisable()
        {
            if (_scrollRect == null)
            {
                return;
            }

            _scrollRect.onValueChanged.RemoveListener(bounds => TryToEnableScrollButtons(bounds));
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

        private IEnumerator AnimateScrolling(SwipeDirection direction)
        {
            float scrollDirection = direction == SwipeDirection.Up ? SCROLL_STEP * -1 : SCROLL_STEP;

            Vector2 scrollAnchoredPos = _scrollableElement.anchoredPosition;
            RectTweenableObject tweenableObjectStart =
                new RectTweenableObject(_scrollableElement, scrollAnchoredPos, TweenHelper.VectorZero);
            RectTweenableObject tweenableObjectEnd = new RectTweenableObject(_scrollableElement,
                new Vector2(scrollAnchoredPos.x, scrollAnchoredPos.y + scrollDirection),
                TweenHelper.VectorZero);

            RectTweenParameters parameters = new RectTweenParameters
            {
                tweenableRectTransform = _scrollableElement,
                startPos = tweenableObjectStart,
                endPos = tweenableObjectEnd,
                animationCurve = _animationCurve,
                durationTime = _scrollTime
            };

            if (_isScrollingPossible)
                yield return StartCoroutine(TweenHelper.RectTweenAction(
                    tweenableObject => _scrollableElement.anchoredPosition = tweenableObject.propertyVector1,
                    parameters));

            StopCoroutine(TweenHelper.RectTweenAction());
        }

        private void TryToEnableScrollButtons(Vector2 bounds)
        {
            float currY = bounds.y;
            _nextButton.gameObject.SetActive(currY > 0 && currY <= 1);
            _previousButton.gameObject.SetActive(currY >= 0 && currY < 1);
        }
    }
}