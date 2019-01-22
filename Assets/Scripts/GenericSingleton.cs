using UnityEngine;

namespace  InstaGen
{
    public class GenericSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] possibleInstances = FindObjectsOfType(typeof(T)) as T[];
                    int possibeInstancesCount = possibleInstances.Length;

                    if (possibeInstancesCount == 1)
                    {
                        _instance = possibleInstances[0];
                    }

                    if (possibeInstancesCount > 1)
                    {
                        Debug.LogError("Many instances of "+ typeof(T) + " - cannot create a singleton" );
                    }

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        singletonObject.name = typeof(T).Name;
                        _instance = singletonObject.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }

}
