using UnityEngine;

namespace ARVORE
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }


        protected virtual void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this as T;
        }
    }
}
