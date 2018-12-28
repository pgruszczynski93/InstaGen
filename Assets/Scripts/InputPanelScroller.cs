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
		
		[SerializeField] private Text text;

		public void ScrollUpPanel(string tempString="")
		{
			if (_isHeightValueCached)
			{
				return;
			}
			SetupScrollHeightToKeyboard();
			StartCoroutine(ScrollContent(SwipeDirection.Up));
		}

		private void SetupScrollHeightToKeyboard()
		{
			_maxPanelHeight = (int)(Screen.height*0.8f /4);
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
		
		protected override IEnumerator ScrollContent(SwipeDirection direction)
		{
			_verticalTweenObjectParam = InitializeParametersForOffset(_scrollableContent, _upAlignment);
			
			yield return StartCoroutine(TweenHelper.RectTweenAction(SetCurrentPanelOffset, _verticalTweenObjectParam));
			StopCoroutine(TweenHelper.RectTweenAction());
		}
	}
}

