using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InstaGen
{
	public class HashtagButton : MonoBehaviour
	{
		[SerializeField] private HashtagButtonStatus _buttonStatus;

		[SerializeField] private string _assignedString;
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
                    //mainInstance.AddToDictionaries(_assignedString);
				}
				else
				{
                    //mainInstance.RemoveFromDictionaries(_assignedString);
				}

                mainInstance.TextGenerator.GenerateSelectedHashtagsOutput();
			});

            _toggle.isOn = false;
        }

        private void OnDisable()
		{
			_toggle.onValueChanged.RemoveAllListeners();
            _toggle.isOn = false;
        }

		public void SetHashtagText(string text)
		{
			_assignedString = text.GetFormattedHashtag();
			_hashtagText.text = _assignedString;
		}

	}

}
