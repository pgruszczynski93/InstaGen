using UnityEngine;
using System;

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
        static GestureRecognizer _instance = null;

        [SerializeField] bool _isContinuousTouchEnabled;
        [SerializeField] float _minSwipeDistance;
        [SerializeField] Vector2 _touchBeginPos;
        [SerializeField] Vector2 _touchEndPos;

        public Action OnSwipeEnabled = delegate { };
        public event Action<SwipeData> OnSwipe = delegate { };

        public bool IsSwipingEnabled { get; set; }
        public static GestureRecognizer Instance
        {
            get
            {
                return _instance;
            }
        }

        void Awake()
        {
            if(_instance)
            {
                DestroyImmediate(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // This line fixes badly working Rect Mask 2D 
            Shader.EnableKeyword("UNITY_UI_CLIP_RECT");
        }

        void Update()
        {
            CollectTouchInput();
        }

        public void EnableSwipping()
        {
            OnSwipeEnabled();
        }

        void CollectTouchInput()
        {
            if (IsSwipingEnabled && Input.touchCount > 0)
            {
                Touch currentTouch = Input.GetTouch(0);

                if(currentTouch.phase == TouchPhase.Began)
                {
                    _touchBeginPos = _touchEndPos = currentTouch.position;
                }

                if (_isContinuousTouchEnabled && currentTouch.phase == TouchPhase.Moved)
                {
                    _touchEndPos = currentTouch.position;
                    DetectSwipe();
                }

                if( currentTouch.phase == TouchPhase.Ended || currentTouch.phase == TouchPhase.Canceled)
                {
                    _touchEndPos = currentTouch.position;
                    DetectSwipe();
                }
            }
        }

        void DetectSwipe()
        {
            if (IsAnySwipeDetected())
            {
                SwipeDirection direction = SwipeDirection.NoSwipe;
                if (IsHorizontalSwipeDetected())
                {
                    direction = _touchEndPos.x > _touchBeginPos.x ? SwipeDirection.Right : SwipeDirection.Left;
                    SendSwipe(direction);
                }
                else
                {
                    direction = _touchEndPos.y > _touchBeginPos.y ? SwipeDirection.Up : SwipeDirection.Down;
                    SendSwipe(direction);
                }
                _touchBeginPos = _touchEndPos;
            }
        }

        bool IsAnySwipeDetected()
        {
            return VerticalSwipeDistance() > _minSwipeDistance || HorizontalSwipeDistance() > _minSwipeDistance;
        }

        bool IsHorizontalSwipeDetected()
        {
            return HorizontalSwipeDistance() > VerticalSwipeDistance();    
        }

        float VerticalSwipeDistance()
        {
            return Math.Abs(_touchEndPos.y - _touchBeginPos.y);
        }

        float HorizontalSwipeDistance()
        {
            return Math.Abs(_touchEndPos.x - _touchBeginPos.x);
        }

        void SendSwipe(SwipeDirection direction)
        {
            SwipeData swipeData = new SwipeData()
            {
                SwipeDirection = direction,
                SwipeStartPos = _touchBeginPos,
                SwipeEndPos = _touchEndPos
            };

            OnSwipe(swipeData);
        }
    }

}
