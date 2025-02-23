using System.Collections;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Unit
{
    public class UnitMiner
    {
        private Coroutine _coroutineMine;
        
        public void StartMining()
        {
            if (_coroutineMine != null)
            {
                CoroutinesHandler.StopRoutine(_coroutineMine);
                _coroutineMine = null;
            }

            CoroutinesHandler.StartRoutine(Mine());
        }

        private IEnumerator Mine()
        {
            yield return new WaitForSeconds(3f);
        }
    }
}