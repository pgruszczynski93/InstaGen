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
        
        private int _panelsCount;
        
        private RectTweenParameters _firstTweenObjectParams = new RectTweenParameters();
        private RectTweenParameters _secondTweenObjectParams = new RectTweenParameters();

        private void OnEnable()
        {
            EventsHelper.OnSwipeEnabled += InitialRotatorAnimation;
            EventsHelper.OnSwipe += PlayRotatorAnimation;
            EventsHelper.OnSwipe += Debug_RotatorDirectionInfo;
        }

        private void OnDisable()
        {
            EventsHelper.OnSwipeEnabled -= InitialRotatorAnimation;
            EventsHelper.OnSwipe -= PlayRotatorAnimation;
            EventsHelper.OnSwipe -= Debug_RotatorDirectionInfo;
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
            Debug.Log(string.Format("[Menu Rotator] Direction: {0}", swipeData.moveDirection));
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
            _firstTweenObjectParams = InitializeParametersForOffset(_currentMainPanel, _leftAlignment);
            _secondTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _leftAlignment);

            StartCoroutine(TweenHelper.RectTweenAction(SetCurrentPanelOffset, _firstTweenObjectParams));
            yield return StartCoroutine(TweenHelper.RectTweenAction(SetCurrentPanelOffset,_secondTweenObjectParams));

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

            if (swipeData.moveDirection == MoveDirection.Left && _currentRotatorIndex >= 0 &&
                _currentRotatorIndex < _panelsCount - 1)
            {
                _firstTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _leftAlignment);
                StartCoroutine(TweenHelper.RectTweenAction(SetCurrentPanelOffset, _firstTweenObjectParams));

                ++_currentRotatorIndex;
                _secondTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _leftAlignment);
                yield return StartCoroutine(TweenHelper.RectTweenAction(SetCurrentPanelOffset, _secondTweenObjectParams));
            }

            if (swipeData.moveDirection == MoveDirection.Right && _currentRotatorIndex > 0 &&
                _currentRotatorIndex < _panelsCount)
            {
                _firstTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _rightAlignment);
                StartCoroutine(TweenHelper.RectTweenAction(SetCurrentPanelOffset,_firstTweenObjectParams));

                --_currentRotatorIndex;
                _secondTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _rightAlignment);
                yield return StartCoroutine(TweenHelper.RectTweenAction( SetCurrentPanelOffset, _secondTweenObjectParams));
            }

            StopCoroutine(TweenHelper.RectTweenAction());
            GestureRecognizer.Instance.IsSwipingEnabled = true;
        }
    }
}