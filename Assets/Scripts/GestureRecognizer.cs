using System;
using UnityEngine;

namespace InstaGen
{
    public struct SwipeData
    {
        public Vector2 SwipeStartPos;
        public Vector2 SwipeEndPos;
        public SwipeDirection SwipeDirection;
    }

    public class GestureRecognizer : MonoBehaviour
    {
        [SerializeField] private bool _isContinuousTouchEnabled;
        [SerializeField] private float _minSwipeDistance;
        [SerializeField] private Vector2 _touchBeginPos;
        [SerializeField] private Vector2 _touchEndPos;

        public Action OnSwipeEnabled = delegate { };

        static GestureRecognizer()
        {
            Instance = null;
        }

        public bool IsSwipingEnabled { get; set; }

        public static GestureRecognizer Instance { get; private set; }

        public event Action<SwipeData> OnSwipe = delegate { };

        private void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            CollectTouchInput();
        }

        public void EnableSwipping()
        {
            OnSwipeEnabled();
        }

        private void CollectTouchInput()
        {
            if (IsSwipingEnabled == false || Input.touchCount < 1)
            {
                return;
            }
            
            Touch currentTouch = Input.GetTouch(0);

            if (currentTouch.phase == TouchPhase.Began)
            {
                _touchBeginPos = _touchEndPos = currentTouch.position;
            }

            if (_isContinuousTouchEnabled && currentTouch.phase == TouchPhase.Moved)
            {
                _touchEndPos = currentTouch.position;
            }

            if (currentTouch.phase >= TouchPhase.Ended)
            {
                _touchEndPos = currentTouch.position;
            }
            
            DetectSwipe();

        }

        private void DetectSwipe()
        {
            if (IsAnySwipeDetected() == false)
            {
                return;
            }
        
            SwipeDirection direction = SwipeDirection.NoSwipe;
            
            if (IsHorizontalSwipeDetected())
            {
                direction = _touchEndPos.x > _touchBeginPos.x ? SwipeDirection.Right : SwipeDirection.Left;
            }
            else
            {
                direction = _touchEndPos.y > _touchBeginPos.y ? SwipeDirection.Up : SwipeDirection.Down;
            }

            SendSwipe(direction);

            _touchBeginPos = _touchEndPos;
        }

        private bool IsAnySwipeDetected()
        {
            return VerticalSwipeDistance() > _minSwipeDistance || HorizontalSwipeDistance() > _minSwipeDistance;
        }

        private bool IsHorizontalSwipeDetected()
        {
            return HorizontalSwipeDistance() > VerticalSwipeDistance();
        }

        private float VerticalSwipeDistance()
        {
            return Math.Abs(_touchEndPos.y - _touchBeginPos.y);
        }

        private float HorizontalSwipeDistance()
        {
            return Math.Abs(_touchEndPos.x - _touchBeginPos.x);
        }

        private void SendSwipe(SwipeDirection direction)
        {
            SwipeData swipeData = new SwipeData
            {
                SwipeDirection = direction,
                SwipeStartPos = _touchBeginPos,
                SwipeEndPos = _touchEndPos
            };

            OnSwipe(swipeData);
        }
    }
}