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

		[SerializeField] private string _assignedString;
		//[SerializeField] private HashtagExitButtonBehaviour _exitButton;
		[SerializeField] private Transform _usedParent;
		[SerializeField] private Text _hashtagText;
		[SerializeField] private Toggle _toggle;

		private void OnEnable()
		{
			_toggle.onValueChanged.AddListener((toggleState) =>
            {
                HashtagObjectsManager mainInstance = HashtagObjectsManager.Instance;

                if (toggleState)
				{
                    mainInstance.AddToDictionaries(_assignedString);
				}
				else
				{
                    mainInstance.RemoveFromDictionaries(_assignedString);
				}

                mainInstance.TextGenerator.GenerateSelectedHashtagsOutput();
			});
		}

		private void OnDisable()
		{
			_toggle.onValueChanged.RemoveAllListeners();	
		}

		public void ParentToRootPanel(){
            _toggle.isOn = false;
		}

		public void SetHashtagText(string text)
		{
			string hashtagText = string.Format("{0}{1}", HASH_CHAR, text);
			_assignedString = hashtagText;
			_hashtagText.text = _assignedString;
		}
	}

}
