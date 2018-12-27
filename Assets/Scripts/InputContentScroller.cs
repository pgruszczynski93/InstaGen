using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace InstaGen
{
    public class InputContentScroller : InputScroller
    {
        public static int VerticalContentScrollIndex = 0;
        
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        [SerializeField] private Button _nextPanelButton;
        [SerializeField] private NoDragScrollRect _scrollRect;

        protected override void SetupInitialReferences()
        {
            _scrollStep = 600; 
        }

        private void SetButtons(bool isEnabled)
        {
            _nextButton.gameObject.SetActive(isEnabled);
            _previousButton.gameObject.SetActive(isEnabled);
        }

        private void OnEnable()
        {
            OnScrollVerticalToNext += ScrollToNext;
            OnScrollVerticalToPrevious += ScrollToPrevious;
            
            _scrollRect.onValueChanged.AddListener(TryToEnableScrollButtons);
        }

        private void OnDisable()
        {
            OnScrollVerticalToNext -= ScrollToNext;
            OnScrollVerticalToPrevious -= ScrollToPrevious;

            _scrollRect.onValueChanged.RemoveListener(TryToEnableScrollButtons);
        }
        

        private void TryToEnableScrollButtons(Vector2 bounds)
        {
            float currY = bounds.y;
            if (currY > 0 && currY < 1)
            {
                _nextPanelButton.gameObject.SetActive(false);
                return;
            }
            
            _nextButton.gameObject.SetActive(currY == 1.0f );
            _previousButton.gameObject.SetActive(currY == 0.0f);
            _nextPanelButton.gameObject.SetActive(currY == 0.0f);
        }
        
        protected override void ScrollToNext()
        {
            base.ScrollToNext();            
            StartCoroutine(ScrollContent(SwipeDirection.Down));
        }

        protected override void ScrollToPrevious()
        {
            base.ScrollToPrevious();            
            StartCoroutine(ScrollContent(SwipeDirection.Up));
        }

        protected override IEnumerator ScrollContent(SwipeDirection direction)
        {
            float scrollDirection = direction == SwipeDirection.Up ? _scrollStep * -1 : _scrollStep;

            Vector2 scrollAnchoredPos = _scrollableContent.anchoredPosition;
            RectTweenableObject tweenableObjectStart =
                new RectTweenableObject(_scrollableContent, scrollAnchoredPos, TweenHelper.VectorZero);
            RectTweenableObject tweenableObjectEnd = new RectTweenableObject(_scrollableContent,
                new Vector2(scrollAnchoredPos.x, scrollAnchoredPos.y + scrollDirection),
                TweenHelper.VectorZero);

            RectTweenParameters parameters = new RectTweenParameters
            {
                tweenableRectTransform = _scrollableContent,
                startPos = tweenableObjectStart,
                endPos = tweenableObjectEnd,
                animationCurve = _animationCurve,
                durationTime = _scrollTime
            };

            if (_isScrollingPossible)
            {
                SetButtons(false);
                
                yield return StartCoroutine(TweenHelper.RectTweenAction(
                    tweenableObject => _scrollableContent.anchoredPosition = tweenableObject.propertyVector1,
                    parameters));
                
                SetButtons(true);
            }

            StopCoroutine(TweenHelper.RectTweenAction());
        }
    }
}