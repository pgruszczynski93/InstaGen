using System.Collections;
using UnityEngine;

namespace InstaGen
{
    public class MenuRotator : MonoBehaviour
    {
        const int HORIZONTAL_ALIGNMENT = 1080;
        const int VERTICAL_ALIGNMENT = 100;

        readonly Vector2 _leftAlignment = new Vector2 (-HORIZONTAL_ALIGNMENT, 0);
        readonly Vector2 _rightAlignment = new Vector2(HORIZONTAL_ALIGNMENT, 0);

        [SerializeField] int _currentRotatorIndex = 0;
        [SerializeField] int _panelsCount = 0;
        [SerializeField] float _animationDurationTime;

        [SerializeField] AnimationCurve _animationCurve;
        [SerializeField] RectTransform _currentMainPanel; 
        [SerializeField] RectTransform[] _panels;

        TweenParameters firstTweenObjectParams = new TweenParameters();
        TweenParameters secondTweenObjectParams = new TweenParameters();

        void OnEnable()
        {
            GestureRecognizer.Instance.OnSwipeEnabled += InitialRotatorAnimation;
            GestureRecognizer.Instance.OnSwipe += PlayRotatorAnimation;
            GestureRecognizer.Instance.OnSwipe += Debug_RotatorDirectionInfo;
        }

        void OnDisable()
        {
            GestureRecognizer.Instance.OnSwipeEnabled -= InitialRotatorAnimation;
            GestureRecognizer.Instance.OnSwipe -= PlayRotatorAnimation;
            GestureRecognizer.Instance.OnSwipe -= Debug_RotatorDirectionInfo;
        }

        void Start()
        {
            SetupReferences();
        }

        void SetupReferences()
        {
            _panelsCount = _panels.Length;
        }

        void InitialRotatorAnimation()
        {
            StartCoroutine(StartInitialPanelAnimation());
        }

        void PlayRotatorAnimation(SwipeData swipeData)
        {
            StartCoroutine(PanelAnimation(swipeData));
        }

        void Debug_RotatorDirectionInfo(SwipeData swipeData)
        {
            Debug.Log(string.Format("[Menu Rotator] Direction: {0}", swipeData.SwipeDirection));
        }

        TweenParameters InitializeParametersForOffset(RectTransform panel, Vector2 alignment)
        {
            return new TweenParameters()
            {
                tweenableRectTransform = panel,
                startPos = new RectTweenableObject(panel, panel.offsetMin, panel.offsetMax),
                endPos = new RectTweenableObject(panel, panel.offsetMin + alignment, panel.offsetMax + alignment),
                animationCurve = _animationCurve,
                durationTime = _animationDurationTime
            };
        }

        IEnumerator StartInitialPanelAnimation()
        {
            firstTweenObjectParams = InitializeParametersForOffset(_currentMainPanel, _leftAlignment);
            secondTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _leftAlignment);

            StartCoroutine(TweenHelper.SimpleTweenAction((tweenableObj) => SetCurrentPanelOffset(tweenableObj), firstTweenObjectParams));
            yield return StartCoroutine(TweenHelper.SimpleTweenAction((tweenableObj) => SetCurrentPanelOffset(tweenableObj), secondTweenObjectParams));

            GestureRecognizer.Instance.IsSwipingEnabled = true;
            StopCoroutine(TweenHelper.SimpleTweenAction());
        }

        void SetCurrentPanelOffset(RectTweenableObject tweenableObject)
        {
            _currentMainPanel = tweenableObject.rectTransform;
            _currentMainPanel.offsetMin = tweenableObject.propertyVector1;
            _currentMainPanel.offsetMax = tweenableObject.propertyVector2;
        }

        IEnumerator PanelAnimation(SwipeData swipeData)
        {
            GestureRecognizer.Instance.IsSwipingEnabled = false;

            if (swipeData.SwipeDirection == SwipeDirection.Left && (_currentRotatorIndex >= 0 && _currentRotatorIndex < _panelsCount - 1))
            {
                firstTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _leftAlignment);
                StartCoroutine(TweenHelper.SimpleTweenAction((tweenableObject) => SetCurrentPanelOffset(tweenableObject), firstTweenObjectParams));

                ++_currentRotatorIndex;
                secondTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _leftAlignment);
                yield return StartCoroutine(TweenHelper.SimpleTweenAction((tweenableObject) => SetCurrentPanelOffset(tweenableObject), secondTweenObjectParams));

            }

            if (swipeData.SwipeDirection == SwipeDirection.Right && (_currentRotatorIndex > 0 && _currentRotatorIndex < _panelsCount))
            {
                firstTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _rightAlignment);
                StartCoroutine(TweenHelper.SimpleTweenAction((tweenableObject) => SetCurrentPanelOffset(tweenableObject), firstTweenObjectParams));

                --_currentRotatorIndex;
                secondTweenObjectParams = InitializeParametersForOffset(_panels[_currentRotatorIndex], _rightAlignment);
                yield return StartCoroutine(TweenHelper.SimpleTweenAction((tweenableObject) => SetCurrentPanelOffset(tweenableObject), secondTweenObjectParams));
            }

            GestureRecognizer.Instance.IsSwipingEnabled = true;
        }
    }
}
