using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Interfaces;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.MainBase
{
    public class Scanner<TObjectToSearch> where TObjectToSearch : ICollectable
    {
        private List<TObjectToSearch> _collectables;
        private readonly float _delay;
        private readonly float _radius;
        private readonly Transform _centerPoint;
        private Coroutine _coroutineScanner;
        
        public Scanner(float radius, float delay, Transform centerPoint)
        {
            _radius = radius;
            _delay = delay;
            _centerPoint = centerPoint;
        }
        
        public event Action ScanFinished;
        
        public List<TObjectToSearch> Collectables => new List<TObjectToSearch>(_collectables);

        public void StartScanning()
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
            yield return new WaitForSeconds(_delay);
            
            FindObjects();
        }

        private void FindObjects()
        {
            _collectables = new List<TObjectToSearch>();
            
            Collider[] colliders = Physics.OverlapSphere(_centerPoint.position, _radius);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.TryGetComponent(out TObjectToSearch collectableResource))
                    if(collectableResource.IsCollected == false)
                        _collectables.Add(collectableResource);
            }
            
            ScanFinished?.Invoke();
        }
    }
}