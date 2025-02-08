using UnityEngine;

namespace Rhythem
{
    public abstract class Manager<T> : Manager where T : Manager<T>
    {
        public static T Instance { get; set; }

        protected override void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }

            Instance = (T)this;

            if (transform.root != null)
            {
                DontDestroyOnLoad(transform.root);
            }
            else
            {
                DontDestroyOnLoad(this);
            }

            base.Awake();
        }

        protected override void OnDestroy()
        {
            Instance = null;
            base.OnDestroy();
        }
    }

    public class Manager : MonoBehaviour
    {
        protected virtual void Awake() { }
        protected virtual void OnDestroy() { }
    }
}