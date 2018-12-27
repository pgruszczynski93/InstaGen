using System;
using System.Collections;
using UnityEngine;

namespace InstaGen
{
    public class InputScroller : MonoBehaviour, IScrollContentVertical {

        protected int _scrollStep; //modify when necessary

        [SerializeField] protected bool _isScrollingPossible;
        [SerializeField] protected float _scrollTime;

        [SerializeField] protected AnimationCurve _animationCurve;
        [SerializeField] protected RectTransform _scrollableContent;
        
        public static Action OnScrollVerticalToNext = delegate {  };
        public static Action OnScrollVerticalToPrevious = delegate {  };
        
        protected virtual void Start()
        {
            SetupInitialReferences();
        }

        protected virtual void SetupInitialReferences(){}

        protected virtual void ScrollToNext()
        {
            if (_scrollableContent == null)
            {
                return;
            }
        }

        protected virtual void ScrollToPrevious()
        {
            if (_scrollableContent == null)
            {
                return;
            }
        }

        public void ScrollVerticallyToNext()
        {
            if (OnScrollVerticalToNext != null)
            {
                OnScrollVerticalToNext();
            }
        }

        public void ScrollVerticallyToPrevious()
        {
            if (OnScrollVerticalToPrevious != null)
            {
                OnScrollVerticalToPrevious();
            }
        }
        
        protected virtual IEnumerator ScrollContent(SwipeDirection direction)
        {
            yield return null;
        }
    }
}
