using System;
using System.Collections;
using CodeBase.Services;
using UnityEngine;
using Resource = CodeBase.ResourceLogic.Resource;

namespace CodeBase.UnitLogic
{
    public class Miner
    {
        private readonly Transform _minePoint;
        private readonly float _radiusToFindNearResource = 1f;
        private readonly CoroutinesHandler _coroutinesHandler;
        private readonly Transform _resourceHolder;
        
        private Coroutine _coroutineMine;

        public Miner(Transform minePoint, CoroutinesHandler coroutineHandler, Transform resourceHolder)
        {
            _minePoint = minePoint;
            _coroutinesHandler = coroutineHandler;
            _resourceHolder = resourceHolder;
        }
        
        public Resource Resource { get; private set; }

        public event Action MiningDone;

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
            yield return new WaitForSeconds(3f);
            
            Collider[] hitColliders = Physics.OverlapSphere(_minePoint.position, _radiusToFindNearResource);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out Resource resource))
                {
                    Resource = resource;
                    Resource.Interact(_resourceHolder);
                    break;
                }
            }

            if (Resource == null)
            {
                throw new AggregateException("Couldn't find resource");
            }
            
            MiningDone?.Invoke();
        }
    }
}