using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InstaGen
{
    public struct OffsetMinMax
    {
        public Vector2 offsetMin;
        public Vector2 offsetMax;
    }

    public class MenuRotator : MonoBehaviour
    {
        const int HORIZONTAL_ALIGNMENT = 1080;
        const int VERTICAL_ALIGNMENT = 100;

        readonly Vector2 _leftAlignment = new Vector2 (-HORIZONTAL_ALIGNMENT, 0);
        readonly Vector2 _rightAlignment = new Vector2(HORIZONTAL_ALIGNMENT, 0);

        [SerializeField] bool _isCircularMovementEnabled;
        [SerializeField] int _currentRotatorIndex = 0;
        [SerializeField] int _panelsCount = 0;
        [SerializeField] float _animationDurationTime;

        [SerializeField] AnimationCurve _animationCurve;
        [SerializeField] RectTransform _startPanel; 
        [SerializeField] RectTransform[] _panels;

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

        IEnumerator StartInitialPanelAnimation()
        {
            //_startPanel.offsetMin =
            //StartCoroutine(TweenHelper.TweenAction2D(
            //    (newOffsetMin, newOffsetMax) =>
            //    {

            //    }
            //    ));

            StartCoroutine(AnimatePanel(_startPanel, _leftAlignment));
            yield return StartCoroutine(AnimatePanel(_panels[_currentRotatorIndex], _leftAlignment));

            GestureRecognizer.Instance.IsSwipingEnabled = true; 
            _startPanel.gameObject.SetActive(false);
        }

        IEnumerator PanelAnimation(SwipeData swipeData)
        {
            if (swipeData.SwipeDirection == SwipeDirection.Left && (_currentRotatorIndex >= 0 && _currentRotatorIndex < _panelsCount - 1))
            {
                StartCoroutine(AnimatePanel(_panels[_currentRotatorIndex], _leftAlignment));
                yield return StartCoroutine(AnimatePanel(_panels[++_currentRotatorIndex], _leftAlignment));
            }

            if (swipeData.SwipeDirection == SwipeDirection.Right && (_currentRotatorIndex > 0 && _currentRotatorIndex < _panelsCount))
            {
                StartCoroutine(AnimatePanel(_panels[_currentRotatorIndex], _rightAlignment));
                yield return StartCoroutine(AnimatePanel(_panels[--_currentRotatorIndex], _rightAlignment));
            }

            // TO DO
            //if (_isCircularMovementEnabled)
            //{
            //}
        }

        IEnumerator AnimatePanel(RectTransform panel, Vector2 alignment)
        {
            float currentTime = 0.0f;
            float animationProgress = 0.0f;
            float curveProgress = 0.0f;

            OffsetMinMax startPanelOffsets = new OffsetMinMax()
            {
                offsetMin = panel.offsetMin,
                offsetMax = panel.offsetMax
            };

            OffsetMinMax newPanelOffsets = new OffsetMinMax()
            {
                offsetMin = startPanelOffsets.offsetMin + alignment,
                offsetMax = startPanelOffsets.offsetMax + alignment
            };

            while (currentTime < _animationDurationTime)
            {
                animationProgress = Mathf.Clamp01(currentTime / _animationDurationTime);
                curveProgress = _animationCurve.Evaluate(animationProgress);

                panel.offsetMin = Vector2.Lerp(startPanelOffsets.offsetMin, newPanelOffsets.offsetMin, curveProgress);
                panel.offsetMax = Vector2.Lerp(startPanelOffsets.offsetMax, newPanelOffsets.offsetMax, curveProgress);

                currentTime += Time.deltaTime;
                yield return null;
            }

            panel.offsetMin = newPanelOffsets.offsetMin;
            panel.offsetMax = newPanelOffsets.offsetMax;
        }
    }
}
