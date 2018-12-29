using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InstaGen
{
	public class InputPanelScroller : InputScroller
	{
		private bool _isHeightValueCached;
		private float _maxPanelHeight;
		
		private Vector2 _upAlignment;
		
		private DisplayKeyboardInfo _keyboardInfo;
		private RectTweenParameters _verticalTweenObjectParam = new RectTweenParameters();

//		private void OnEnable()
//		{
//			EventsHelper.OnInputPanelScrollFinished += ScrollDownPanel;
//		}
//
//		private void OnDisable()
//		{
//			EventsHelper.OnInputPanelScrollFinished -= ScrollDownPanel;
//		}
//		
//		

		public void ScrollUpPanel(string tempString="")
		{
			if (_isHeightValueCached)
			{
				return;
			}
			SetupScrollHeightToKeyboard();
			StartCoroutine(ScrollMovementAnimation(MoveDirection.Up));
		}

		private void ScrollDownPanel()
		{
			StartCoroutine(ScrollMovementAnimation(MoveDirection.Down));
		}
		
		private void SetupScrollHeightToKeyboard()
		{
			_maxPanelHeight = (int)(Screen.height*0.9f /4);
			_upAlignment = new Vector2(0, _maxPanelHeight);
			_isHeightValueCached = true;
		}

		private RectTweenParameters InitializeParametersForOffset(RectTransform panel, Vector2 alignment)
		{
			return new RectTweenParameters
			{
				tweenableRectTransform = panel,
				startPos = new RectTweenableObject(panel, panel.offsetMin, panel.offsetMax),
				endPos = new RectTweenableObject(panel, panel.offsetMin + alignment, panel.offsetMax + alignment),
				animationCurve = _animationCurve,
				durationTime = _scrollTime
			};
		}

		private void SetCurrentPanelOffset(RectTweenableObject tweenableObject)
		{
			_scrollableContent = tweenableObject.rectTransform;
			_scrollableContent.offsetMin = tweenableObject.propertyVector1;
			_scrollableContent.offsetMax = tweenableObject.propertyVector2;
		}
		
		protected override IEnumerator ScrollMovementAnimation(MoveDirection direction)
		{
			Vector2 moveDir = direction == MoveDirection.Up ? _upAlignment : -_upAlignment;
			_verticalTweenObjectParam = InitializeParametersForOffset(_scrollableContent, moveDir);
			
			yield return StartCoroutine(TweenHelper.RectTweenAction(SetCurrentPanelOffset, _verticalTweenObjectParam));
			StopCoroutine(TweenHelper.RectTweenAction());
		}
	}
}

