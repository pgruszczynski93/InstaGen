using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InstaGen
{
	public class ToggleSwitcher : MonoBehaviour
	{
		[SerializeField] private Toggle[] _sideToggles;

		private int _toggleCount;

		private void SetReferences()
		{
			_toggleCount = _sideToggles.Length;
		}

		private void Awake()
		{
			SetReferences();
		}

		private void OnEnable()
		{
			InputScroller.OnScrollVerticalToNext += ScrollToNext;
			InputScroller.OnScrollVerticalToPrevious += ScrollToPrevious;
		}

		private void OnDisable()
		{
			InputScroller.OnScrollVerticalToNext -= ScrollToNext;
			InputScroller.OnScrollVerticalToPrevious -= ScrollToPrevious;
		}

		private void ScrollToNext()
		{
			if (InputContentScroller.VerticalContentScrollIndex > _toggleCount - 1)
			{
				return;
			}
			
			_sideToggles[InputContentScroller.VerticalContentScrollIndex].isOn = false;
			_sideToggles[++InputContentScroller.VerticalContentScrollIndex].isOn = true;
		}
		private void ScrollToPrevious()
		{
			if (InputContentScroller.VerticalContentScrollIndex < 1)
			{
				return;
			}
			
			_sideToggles[InputContentScroller.VerticalContentScrollIndex].isOn = false;
			_sideToggles[--InputContentScroller.VerticalContentScrollIndex].isOn = true;
		}


	}
}
