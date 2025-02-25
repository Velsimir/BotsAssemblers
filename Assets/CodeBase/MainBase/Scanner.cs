using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Interfaces;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.MainBase
{
    public class Scanner<TObjectToSearch> : IRestartable where TObjectToSearch : ICollectable, IInteractable
    {
        private readonly float _delay;
        private readonly float _radius;
        private readonly Transform _centerPoint;
        private Coroutine _coroutineScanner;
        
        public Scanner(float radius, float delay, Transform centerPoint)
        {
            _radius = radius;
            _delay = delay;
            _centerPoint = centerPoint;
            StartScanning();
        }
        
        public event Action<List<TObjectToSearch>> ScanFinished;
        
        private void StartScanning()
        {
            if (_coroutineScanner != null)
            {
                CoroutinesHandler.StopRoutine(_coroutineScanner);
                _coroutineScanner = null;
            }
            
            _coroutineScanner = CoroutinesHandler.StartRoutine(Scan());
        }

        private IEnumerator Scan()
        {
            while (true)
            {
                yield return new WaitForSeconds(_delay);
                
                FindObjects();
            }
        }

        private void FindObjects()
        {
            List<TObjectToSearch> collectables = new List<TObjectToSearch>();
            
            Collider[] colliders = Physics.OverlapSphere(_centerPoint.position, _radius);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.TryGetComponent(out TObjectToSearch collectableResource))
                {
                    if (collectableResource.IsReserved == false)
                    {
                        collectables.Add(collectableResource);
                    }
                }
            }

            if (collectables.Count > 0)
            {
                ScanFinished?.Invoke(collectables);
            }
        }

        public void Restart()
        {
            StartScanning();
        }
    }
}