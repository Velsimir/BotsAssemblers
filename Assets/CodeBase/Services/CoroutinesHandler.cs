using System.Collections;
using UnityEngine;

namespace CodeBase.Services
{
    public sealed class CoroutinesHandler : MonoBehaviour
    {
        public Coroutine StartRoutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        public void StopRoutine(Coroutine routine)
        {
            if (routine != null)
                StopCoroutine(routine);
        }
    }
}