using System;
using System.Collections;
using CodeBase.Interfaces;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UnitLogic
{
    public class Miner
    {
        private IInteractable _collectable;
        private readonly Transform _minePoint;
        private readonly float _radius;
        
        private Coroutine _coroutineMine;
        private CoroutinesHandler _coroutinesHandler;
        
        public event Action<IInteractable> MiningDone;

        public Miner(Transform minePoint, float radius, CoroutinesHandler coroutineHandler)
        {
            _minePoint = minePoint;
            _radius = radius;
            _coroutinesHandler = coroutineHandler;
        }

        public void StartMine()
        {
            if (_coroutineMine != null)
            {
                _coroutinesHandler.StopRoutine(_coroutineMine);
                _coroutineMine = null;
            }

            _coroutinesHandler.StartRoutine(Mine());
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
            {
                throw new AggregateException("Couldn't find resource");
            }
            
            yield return new WaitForSeconds(3f);

            MiningDone?.Invoke(_collectable);
        }
    }
}