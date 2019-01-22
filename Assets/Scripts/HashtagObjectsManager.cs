using System.Collections;
using System.Collections.Generic;
using InstaGen;
using TMPro;
using UnityEngine;

namespace InstaGen
{
    public class HashtagObjectsManager : MonoBehaviour
    {
        private static HashtagObjectsManager _instance = null;

        private Dictionary<string, GameObject> _usedHashtags;
        private Dictionary<string, string> _hashtagTexts;

        [SerializeField] private GameObject _hashtagButtonPrefab;
        [SerializeField] private TMP_InputField _popupHashtagText;

        [SerializeField] private TextGenerator _textGenerator;

        public TextGenerator TextGenerator
        {
            get { return _textGenerator; }
        }

        public Dictionary<string, GameObject> UsedHashtags
        {
            get
            {
                return _usedHashtags;
            }
        }

        public Dictionary<string, string> HashtagTexts
        {
            get { return _hashtagTexts; }
        }
        
        public static HashtagObjectsManager Instance
        {
            get { return _instance; }
        }
        
        private void CreateSingleton()
        {
            if (_instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void SetInitialReferences()
        {
            _usedHashtags = new Dictionary<string, GameObject>();
            _hashtagTexts = new Dictionary<string, string>();
        }

        public void AddToDictionaries(string hashtagName)
        {
            if (_hashtagTexts.ContainsKey(hashtagName) == false)
            {
                _hashtagTexts.Add(hashtagName, hashtagName);
            }
        }

        public void RemoveFromDictionaries(string hashTagName)
        {
            if (_hashtagTexts.ContainsKey(hashTagName))
            {
                _hashtagTexts.Remove(hashTagName);
            }
        }

        private void Awake()
        {
            CreateSingleton();
            SetInitialReferences();
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

            go.SetActive(true);

            string hashtagText = _popupHashtagText.text;
            
            hb.SetHashtagText(hashtagText);
            hb.ParentToRootPanel();
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
