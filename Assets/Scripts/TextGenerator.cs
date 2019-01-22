using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InstaGen
{
    public class TextGenerator : GenericSingleton<TextGenerator>
    {
        [SerializeField] private TMP_Text _hashtagsOutputText;

        [SerializeField] private List<string> _hashtagTextsList;
        [SerializeField] private List<string> _selectedHashtagsTextsList;

        public void AddToHashtagTextsList(string hashtagText)
        {
            _hashtagTextsList.AddUnique(hashtagText);
        }

        public void AddToSelectedHashtagTextsList(string hashtagText)
        {
            _selectedHashtagsTextsList.AddUnique(hashtagText);
        }

        public void RemoveFromSelectedHashtagTextsList(string hashtagText)
        {
            _selectedHashtagsTextsList.Remove(hashtagText);
        }

        public void GenerateSelectedHashtagsOutput()
        {
            string textResult = "";
            _hashtagsOutputText.text = "";

            for (int i = 0; i < _selectedHashtagsTextsList.Count; i++)
            {
                textResult += (_selectedHashtagsTextsList[i] + " ");
            }

            _hashtagsOutputText.text = textResult;
        }
    }
}