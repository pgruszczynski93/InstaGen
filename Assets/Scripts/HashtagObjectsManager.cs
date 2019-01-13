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
        private Dictionary<string, GameObject> _unusedHashtags;

        [SerializeField] private GameObject _hashtagButtonPrefab;
        [SerializeField] private TMP_InputField _popupHashtagText;

        [SerializeField] private Transform _availables; //TEMPORARYSOLUTION!!!!! REMOVE LATER


        public HashtagObjectsManager Instance
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
            _unusedHashtags = new Dictionary<string, GameObject>();
        }

        private void Awake()
        {
            CreateSingleton();
            SetInitialReferences();
        }

        private void OnEnable()
        {
            EventsHelper.OnHashtagButtonGenerate += GenerateHashtagButton;
        }

        private void OnDisable()
        {
            EventsHelper.OnHashtagButtonGenerate -= GenerateHashtagButton;
        }

        public void GenerateHashtagButton()
        {
            GameObject go = Instantiate(_hashtagButtonPrefab);
            HashtagButton hb = go.GetComponent<HashtagButton>();

            go.SetActive(true);
			
            hb.SetHashtagText(_popupHashtagText.text);
            hb.ParentToAvailablePanel();
        }

        public void InvokeOnHashtagGenerate()
        {
            if (EventsHelper.OnHashtagButtonGenerate != null)
            {
                EventsHelper.OnHashtagButtonGenerate();
            }
        }
    }    
}
