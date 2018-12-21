using System.Collections;
using UnityEngine;

namespace InstaGen
{
    public class MenuRotator : MonoBehaviour
    {
        private const int HORIZONTAL_ALIGNMENT = 1080;

        private readonly Vector2 _leftAlignment = new Vector2(-HORIZONTAL_ALIGNMENT, 0);
        private readonly Vector2 _rightAlignment = new Vector2(HORIZONTAL_ALIGNMENT, 0);

        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private float _animationDurationTime;
        [SerializeField] private RectTransform _currentMainPanel;

        [SerializeField] private int _currentRotatorIndex;
        [SerializeField] private RectTransform[] _panels;
        [SerializeField] private int _panelsCount;

        private RectTweenParameters firstTweenObjectParams = new RectTweenParameters();
        private RectTweenParameters secondTweenObjectParams = new RectTweenParameters();

        private void OnEnable()
        {
            GestureRecognizer.Instance.OnSwipeEnabled += InitialRotatorAnimation;
            GestureRecognizer.Instance.OnSwipe += PlayRotatorAnimation;
            GestureRecognizer.Instance.OnSwipe += Debug_RotatorDirectionInfo;
        }

        private void OnDisable()
        {
            GestureRecognizer.Instance.OnSwipeEnabled -= InitialRotatorAnimation;
            GestureRecognizer.Instance.OnSwipe -= PlayRotatorAnimation;
            GestureRecognizer.Instance.OnSwipe -= Debug_RotatorDirectionInfo;
        }

        private void Start()
        {
            SetupReferences();
        }

        private void SetupReferences()
        {
            _panelsCount = _panels.Length;
        }

        private void InitialRotatorAnimation()
        {
            StartCoroutine(StartInitialPanelAnimation());
        }

        private void PlayRotatorAnimation(SwipeData swipeData)
        {
            StartCoroutine(PanelAnimation(swipeData));
        }

        private void Debug_RotatorDirectionInfo(SwipeData swipeData)
        {
            Debug.Log(string.Format("[Menu Rotator] Direction: {0}", swipeData.SwipeDirection));
        }

        private RectTweenParameters InitializeParametersForOffset(RectTransform panel, Vector2 alignment)
        {
            return new RectTweenParameters
            {
                tweenableRectTransform = panel,
                startPos = new RectTweenableObject(panel, panel.offsetMin, panel.offsetMax),
                endPos = new RectTweenableObject(panel, panel.offsetMin + alignment, panel.offsetMax + alignment),
                animationCurve = _animationCurve,
                durationTime = _animationDurationTime
            };
        }

        private IEnumerator StartInitialPanelAnimation()
        {
            firstTweenObjectParams = InitializeParametersForOffset(_currentMainPanel, _leftAlignment);
            secondTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _leftAlignment);

            StartCoroutine(TweenHelper.RectTweenAction(tweenableObj => SetCurrentPanelOffset(tweenableObj),
                firstTweenObjectParams));
            yield return StartCoroutine(TweenHelper.RectTweenAction(tweenableObj => SetCurrentPanelOffset(tweenableObj),
                secondTweenObjectParams));

            GestureRecognizer.Instance.IsSwipingEnabled = true;
            StopCoroutine(TweenHelper.RectTweenAction());
        }

        private void SetCurrentPanelOffset(RectTweenableObject tweenableObject)
        {
            _currentMainPanel = tweenableObject.rectTransform;
            _currentMainPanel.offsetMin = tweenableObject.propertyVector1;
            _currentMainPanel.offsetMax = tweenableObject.propertyVector2;
        }

        private IEnumerator PanelAnimation(SwipeData swipeData)
        {
            GestureRecognizer.Instance.IsSwipingEnabled = false;

            if (swipeData.SwipeDirection == SwipeDirection.Left && _currentRotatorIndex >= 0 &&
                _currentRotatorIndex < _panelsCount - 1)
            {
                firstTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _leftAlignment);
                StartCoroutine(TweenHelper.RectTweenAction(tweenableObject => SetCurrentPanelOffset(tweenableObject),
                    firstTweenObjectParams));

                ++_currentRotatorIndex;
                secondTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _leftAlignment);
                yield return StartCoroutine(TweenHelper.RectTweenAction(
                    tweenableObject => SetCurrentPanelOffset(tweenableObject), secondTweenObjectParams));
            }

            if (swipeData.SwipeDirection == SwipeDirection.Right && _currentRotatorIndex > 0 &&
                _currentRotatorIndex < _panelsCount)
            {
                firstTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _rightAlignment);
                StartCoroutine(TweenHelper.RectTweenAction(tweenableObject => SetCurrentPanelOffset(tweenableObject),
                    firstTweenObjectParams));

                --_currentRotatorIndex;
                secondTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _rightAlignment);
                yield return StartCoroutine(TweenHelper.RectTweenAction(
                    tweenableObject => SetCurrentPanelOffset(tweenableObject), secondTweenObjectParams));
            }

            StopCoroutine(TweenHelper.RectTweenAction());
            GestureRecognizer.Instance.IsSwipingEnabled = true;
        }
    }
}