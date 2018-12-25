﻿using System.Collections;
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
        [SerializeField] private Button _previousButton;
        [SerializeField] private Button _nextPanelButton;
        [SerializeField] private RectTransform _scrollableElement;
        [SerializeField] private NoDragScrollRect _scrollRect;

        [SerializeField] private float _scrollTime;

        [SerializeField] private Text _debugText;
        [SerializeField] private InputField _debugInput;

        private void SetButtons(bool isEnabled)
        {
            _nextButton.gameObject.SetActive(isEnabled);
            _previousButton.gameObject.SetActive(isEnabled);
        }

        private void OnEnable()
        {
            if (_scrollRect == null)
            {
                return;
            }
            
            _scrollRect.onValueChanged.AddListener(TryToEnableScrollButtons);
        }

        private void OnDisable()
        {
            if (_scrollRect == null)
            {
                return;
            }

            _scrollRect.onValueChanged.RemoveListener(TryToEnableScrollButtons);
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
            {
                SetButtons(false);
                
                yield return StartCoroutine(TweenHelper.RectTweenAction(
                    tweenableObject => _scrollableElement.anchoredPosition = tweenableObject.propertyVector1,
                    parameters));
                
                SetButtons(true);
            }

            StopCoroutine(TweenHelper.RectTweenAction());
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

        private void Update()
        {
            if (_debugInput.isFocused)
            {
                _debugText.text = string.Format("XXS: {0} {1}", TouchScreenKeyboard.visible, AndroidNativeHelper.GetKeyboardDisplayInfo());
            }
        }

        private void ScrollInputPanel()
        {
        }
        
    }
}