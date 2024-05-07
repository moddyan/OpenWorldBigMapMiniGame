using UnityEngine;

namespace Game.Common
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T s_Instance;
        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    GameObject gameObject = new(typeof(T).Name);
                    s_Instance = gameObject.AddComponent<T>();
                    DontDestroyOnLoad(gameObject);
                }

                return s_Instance;
            }
        }

        public static void CreateInstance()
        {
            if (s_Instance == null)
            {
                GameObject gameObject = new(typeof(T).Name);
                s_Instance = gameObject.AddComponent<T>();
                DontDestroyOnLoad(gameObject);
            }
        }

        public static void DestroyInstance()
        {
            if (s_Instance != null)
            {
                Destroy(s_Instance.gameObject);
                s_Instance = null;
            }
        }

        public static bool HasInstance()
        {
            return s_Instance != null;
        }

        protected virtual void Awake()
        {
            if (s_Instance != null)
            {
                Debug.LogError($"[MonoSingleton] Already Exists {typeof(T)}", this);
            }
            else
            {
                s_Instance = this as T;
                DontDestroyOnLoad(gameObject);
                Init();
            }
        }

        protected virtual void Init()
        {
        }
    }
}