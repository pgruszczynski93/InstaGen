using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InstaGen
{
	public class ToggleSwitcher : MonoBehaviour
	{
		[SerializeField] private int _toggleIndex;
		[SerializeField] private int _toggleCount;
		[SerializeField] private Toggle[] _sideToggles;

		private void SetReferences()
		{
			_toggleCount = _sideToggles.Length;
		}

		private void Start()
		{
			SetReferences();
		}

		public void SetNextToggle()
		{
			if (_toggleIndex < _toggleCount)
			{
				_sideToggles[_toggleIndex].isOn = false;
				_sideToggles[++_toggleIndex].isOn = true;
			}
		}

		public void SetPreviousToggle()
		{
			if (_toggleIndex > 0)
			{
				_sideToggles[_toggleIndex].isOn = false;
				_sideToggles[--_toggleIndex].isOn = true;
			}
		}
	}
}
