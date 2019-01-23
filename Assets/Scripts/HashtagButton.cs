using UnityEngine;
using UnityEngine.UI;

namespace InstaGen
{
	public class HashtagButton : MonoBehaviour
	{
		[SerializeField] private string _assignedString;
		[SerializeField] private Transform _usedParent;
		[SerializeField] private Text _hashtagText;
		[SerializeField] private Toggle _toggle;

		private void OnEnable()
		{
            _toggle.onValueChanged.AddListener((toggleState) =>
            {
                TextGenerator textGenerator = TextGenerator.Instance;
                if (toggleState)
				{
                    textGenerator.AddToSelectedHashtagTextsList(_assignedString);
                }
                else
				{
                    textGenerator.RemoveFromSelectedHashtagTextsList(_assignedString);
                }

                textGenerator.GenerateSelectedHashtagsOutput();
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
            _assignedString = text;
            _hashtagText.text = _assignedString;
		}

	}

}
