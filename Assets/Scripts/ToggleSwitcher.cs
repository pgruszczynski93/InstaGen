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
			EventsHelper.OnScroll += UpdateToggles;
		}

		private void OnDisable()
		{
			EventsHelper.OnScroll += UpdateToggles;
		}

		private void UpdateToggles(MoveDirection direction)
		{
			switch (direction)
			{
				case MoveDirection.Up:
					EnablePreviousToggle();
					break;
				case MoveDirection.Down:
					EnableNextToggle();
					break;

				default:
					break;
			}
		}

		private void EnableNextToggle()
		{
			if (InputContentScroller.VerticalContentScrollIndex > _toggleCount - 1)
			{
				return;
			}
			
			_sideToggles[InputContentScroller.VerticalContentScrollIndex].isOn = false;
			_sideToggles[++InputContentScroller.VerticalContentScrollIndex].isOn = true;
		}
		
		private void EnablePreviousToggle()
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
