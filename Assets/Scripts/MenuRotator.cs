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
        [SerializeField] float _animationDurationTime;

        [SerializeField] AnimationCurve _animationCurve;
        [SerializeField] RectTransform _startPanel; 
        [SerializeField] RectTransform[] _panels;

        void Start()
        {
            GestureRecognizer.Instance.OnSwipeEnabled += InitialRotatorAnimation;
            GestureRecognizer.Instance.OnSwipe += PlayRotatorAnimation;
        }

        void OnDisable()
        {
            GestureRecognizer.Instance.OnSwipeEnabled -= InitialRotatorAnimation;
            GestureRecognizer.Instance.OnSwipe -= PlayRotatorAnimation;
        }

        void InitialRotatorAnimation()
        {
            StartCoroutine(StartInitialPanelAnimation());
        }

        void PlayRotatorAnimation(SwipeData swipeData)
        {
            StartCoroutine(PanelAnimation(swipeData));
        }

        IEnumerator StartInitialPanelAnimation()
        {
            StartCoroutine(AnimatePanel(_startPanel, _leftAlignment));
            yield return StartCoroutine(AnimatePanel(_panels[_currentRotatorIndex], _leftAlignment));

            GestureRecognizer.Instance.IsSwipingEnabled = true; 
            _startPanel.gameObject.SetActive(false);
        }

        IEnumerator PanelAnimation(SwipeData swipeData)
        {
            if (swipeData.SwipeDirection == SwipeDirection.Left && (_currentRotatorIndex >= 0 && _currentRotatorIndex < _panels.Length - 1))
            {
                StartCoroutine(AnimatePanel(_panels[_currentRotatorIndex], _leftAlignment));
                yield return StartCoroutine(AnimatePanel(_panels[++_currentRotatorIndex], _leftAlignment));
            }

            if (swipeData.SwipeDirection == SwipeDirection.Right && (_currentRotatorIndex > 0 && _currentRotatorIndex < _panels.Length))
            {
                StartCoroutine(AnimatePanel(_panels[_currentRotatorIndex], _rightAlignment));
                yield return StartCoroutine(AnimatePanel(_panels[--_currentRotatorIndex], _rightAlignment));
            }
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
