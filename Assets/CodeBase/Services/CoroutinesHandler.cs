using System.Collections;
using UnityEngine;

namespace CodeBase.Services
{
    public sealed class CoroutinesHandler : MonoBehaviour
    {
        private static CoroutinesHandler Instance
        {
            get
            {
                if (SubInstance == null)
                {
                    GameObject gameObject = new GameObject("CoroutinesHandler");
                    SubInstance = gameObject.AddComponent<CoroutinesHandler>();
                    DontDestroyOnLoad(gameObject);
                }

                return SubInstance;
            }
        }
        
        private static CoroutinesHandler SubInstance;

        public static Coroutine StartRoutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        public static void StopRoutine(Coroutine routine)
        {
            if (routine != null)
            {
                Instance.StopCoroutine(routine);
            }
        }
    }
}