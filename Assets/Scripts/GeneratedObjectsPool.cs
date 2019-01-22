using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace InstaGen
{
    [System.Serializable]
    public class Pool
    {
        public ObjectPoolTag objectTag;
        public int poolSize;
        public GameObject spawnedPrefab;
        [CanBeNull] public Transform parent;
    }

    public class GeneratedObjectsPool : GenericSingleton<GeneratedObjectsPool>
    {
        private Dictionary<ObjectPoolTag, Queue<GameObject>> _poolDictionary;

        [SerializeField] private List<Pool> _pools;

        private void Start()
        {
            SetupPoolDictionary();
        }

        private void SetupPoolDictionary()
        {
            _poolDictionary = new Dictionary<ObjectPoolTag, Queue<GameObject>>();

            for (int i = 0; i < _pools.Count; i++)
            {
                Queue<GameObject> currentPool = new Queue<GameObject>();
                for (int j = 0; j < _pools[i].poolSize; j++)
                {
                    GameObject spawnedObject = Instantiate(_pools[i].spawnedPrefab, _pools[i].parent);
                    spawnedObject.SetActive(false);
                    currentPool.Enqueue(spawnedObject);
                }

                _poolDictionary.Add(_pools[i].objectTag, currentPool);
            }
        }

        public GameObject GetFromPool(ObjectPoolTag poolTag)
        {
            if (_poolDictionary.ContainsKey(poolTag) == false)
            {
                return null;
            }

            GameObject objectFromQueue = _poolDictionary[poolTag].Dequeue();

            objectFromQueue.SetActive(true);

            _poolDictionary[poolTag].Enqueue(objectFromQueue);

            return objectFromQueue;
        }
    }
}

