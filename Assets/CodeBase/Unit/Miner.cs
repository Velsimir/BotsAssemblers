using System;
using System.Collections;
using CodeBase.Interfaces;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Unit
{
    public class Miner
    {
        private IInteractable _collectable;
        private readonly Transform _minePoint;
        private readonly float _radius;
        
        private Coroutine _coroutineMine;
        
        public event Action<IInteractable> MiningDone;

        public Miner(Transform minePoint, float radius)
        {
            _minePoint = minePoint;
            _radius = radius;
        }

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
            Collider[] hitColliders = Physics.OverlapSphere(_minePoint.position, _radius);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IInteractable resource))
                {
                    _collectable = resource;
                    break;
                }
            }

            if (_collectable == null)
                throw new AggregateException("Couldn't find resource");
            
            yield return new WaitForSeconds(3f);
            Debug.Log("Mining done");
            MiningDone?.Invoke(_collectable);
        }
    }
}