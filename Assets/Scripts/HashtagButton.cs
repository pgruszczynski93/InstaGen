using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InstaGen
{
	public class HashtagButton : MonoBehaviour
	{

		private const string HASH_CHAR = "#";

		[SerializeField] private HashtagButtonStatus _buttonStatus;
		
		[SerializeField] private HashtagExitButtonBehaviour _exitButton;
		[SerializeField] private Transform _usedParent;
		[SerializeField] private Transform _availableParent;
		[SerializeField] private Text _hashtagText;
		
		public void ChangeHashtagButtonParent()
		{
			if (_buttonStatus == HashtagButtonStatus.Used)
			{
				ChangeParent(_availableParent, HashtagButtonStatus.Available, false);
			}
			else
			{
				ChangeParent(_usedParent, HashtagButtonStatus.Used, true);
			}
		}

		public void ParentToUsedPanel()
		{
			ChangeParent(_usedParent, HashtagButtonStatus.Used, true);
		}

		public void ParentToAvailablePanel()
		{
			ChangeParent(_availableParent, HashtagButtonStatus.Available, false);
		}

		public void SetHashtagText(string text)
		{
			string hashtagText = string.Format("{0}{1}", HASH_CHAR, text);
			_hashtagText.text = hashtagText;
		}

		private void ChangeParent(Transform parent, HashtagButtonStatus status, bool hasExitButton)
		{
			_exitButton.gameObject.SetActive(hasExitButton);
			transform.SetParent(parent);
			_buttonStatus = status;
		}

	}

}
