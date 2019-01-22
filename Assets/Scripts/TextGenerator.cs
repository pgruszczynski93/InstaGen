using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InstaGen
{
    public class TextGenerator : MonoBehaviour
    {
        [SerializeField] private TMP_Text _hashtagsOutputText;

        [SerializeField] private List<string> _hashtagTextsList;
        
        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        public void GenerateSelectedHashtagsOutput()
        {
            string textResult = "";
            _hashtagsOutputText.text = "";

            //foreach (KeyValuePair<string, string> pair in HashtagObjectsManager.Instance.HashtagTexts)
            //{
            //    textResult += (pair.Value + " ");
            //}
            
            _hashtagsOutputText.text = textResult;
        }
    }
}