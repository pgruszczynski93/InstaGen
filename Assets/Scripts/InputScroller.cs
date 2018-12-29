using System;
using System.Collections;
using UnityEngine;

namespace InstaGen
{
    public class InputScroller : MonoBehaviour {

        protected int _scrollStep; //modify when necessary

        [SerializeField] protected bool _isScrollingPossible;
        [SerializeField] protected float _scrollTime;

        [SerializeField] protected AnimationCurve _animationCurve;
        [SerializeField] protected RectTransform _scrollableContent;
        
        
        protected virtual void Start()
        {
            SetupInitialReferences();
        }

        protected virtual void SetupInitialReferences(){}

        protected virtual void ScrollObject(MoveDirection direction)
        {
            if (_scrollableContent == null)
            {
                return;
            }
        }

        private void StartScrollMovement(MoveDirection direction)
        {
            if (EventsHelper.OnScroll != null)
            {
                EventsHelper.OnScroll(direction);
            }
        }

        public void ScrollVerticallyUp()
        {
            StartScrollMovement(MoveDirection.Up);
        }

        public void ScrollVerticallyDown()
        {
            StartScrollMovement(MoveDirection.Down);
        }

        protected virtual IEnumerator ScrollMovementAnimation(MoveDirection direction)
        {
            yield return null;
        }
    }
}
