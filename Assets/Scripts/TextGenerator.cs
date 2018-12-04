using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace InstaGen
{
    public class TextGenerator : MonoBehaviour
    {

        const string TITLE_PATTERN = "{0} {1} @ {2}, {3}";
        const string EXTENDED_TITLE_PATTERN = "{0} {1}, {2} @ {3}, {4}";
        const string EXTENDED_TITLE_PATTERN_DATE = "{0} {1}, {2} @ {3}, {4}, {5}";
        const string DESCRIPTION_PATTERN = "{0} {1} {2} {1}{1}{2} {3} {1} {4}";

        [SerializeField] string _currentResult;
        [SerializeField] string _separator;
        [SerializeField] TMP_InputField[] _inputFields;
        [SerializeField] TMP_InputField _resultField;
        [SerializeField] Button _copyButton;

        void Start()
        {

        }

        //void Update()
        //{

        //}

        //void GenerateText()
        //{
        //    _currentResult = string.Format(EXTENDED_TITLE_PATTERN, _inputFields[0], _inputFields[1], _inputFields[2], _inputFields[3], _inputFields[4]);
        //}

        public void OnButtonClick()
        {
            _currentResult = string.Format(EXTENDED_TITLE_PATTERN, _inputFields[0].text, _inputFields[1].text, _inputFields[2].text, _inputFields[3].text, _inputFields[4].text );
            _resultField.text = _currentResult;
        }
    }

}
