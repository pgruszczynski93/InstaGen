using System.Collections;
using System.Collections.Generic;
using InstaGen;
using TMPro;
using UnityEngine;

namespace InstaGen
{
    public class HashtagObjectsManager : GenericSingleton<HashtagObjectsManager>
    {
        private static HashtagObjectsManager _instance = null;

        [SerializeField] private GameObject _hashtagButtonPrefab;
        [SerializeField] private TMP_InputField _popupHashtagText;

        [SerializeField] private TextGenerator _textGenerator;

        public TextGenerator TextGenerator
        {
            get { return _textGenerator; }
        }

        private void OnEnable()
        {
            EventsHelper.OnHashtagsTextsGenerate += GenerateHashtagButton;
        }

        private void OnDisable()
        {
            EventsHelper.OnHashtagsTextsGenerate -= GenerateHashtagButton;
        }

        public void GenerateHashtagButton()
        {
            GameObject go = GeneratedObjectsPool.Instance.GetFromPool(ObjectPoolTag.HashtagButton);
            HashtagButton hb = go.GetComponent<HashtagButton>();

            string hashtagText = _popupHashtagText.text;
            
            hb.SetHashtagText(hashtagText);
        }

        public void InvokeOnHashtagGenerate()
        {
            if (EventsHelper.OnHashtagsTextsGenerate != null)
            {
                EventsHelper.OnHashtagsTextsGenerate();
            }
        }
    }    
}
