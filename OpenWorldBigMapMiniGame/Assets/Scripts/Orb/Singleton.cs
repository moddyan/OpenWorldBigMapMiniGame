using System;

namespace Game.Common
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T s_Instance;
        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new T();
                    s_Instance.Init();
                }

                return s_Instance;
            }
        }

        public static void CreateInstance()
        {
            if (s_Instance == null)
            {
                s_Instance = new T();
                s_Instance.Init();
            }
        }

        public static void DestroyInstance()
        {
            if (s_Instance != null)
            {
                s_Instance.OnDestroy();
                s_Instance = null;
            }
        }

        public static bool HasInstance()
        {
            return s_Instance != null;
        }

        protected virtual void Init()
        {
        }

        protected virtual void OnDestroy()
        {
        }
    }
}