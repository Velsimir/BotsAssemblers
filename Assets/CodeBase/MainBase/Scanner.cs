using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Interfaces;
using CodeBase.ResourceLogic;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.MainBase
{
    public class Scanner
    {
        private readonly float _delay;
        private readonly float _radius;
        private readonly Transform _centerPoint;
        private Coroutine _coroutineScanner;
        private CoroutinesHandler _coroutinesHandler;
        
        public Scanner(float radius, float delay, Transform centerPoint, CoroutinesHandler coroutinesHandler)
        {
            _radius = radius;
            _delay = delay;
            _centerPoint = centerPoint;
            _coroutinesHandler = coroutinesHandler;
            
            StartScanning();
        }
        
        public event Action<Queue<Resource>> ScanFinished;
        
        private void StartScanning()
        {
            if (_coroutineScanner != null)
            {
                _coroutinesHandler.StopRoutine(_coroutineScanner);
                _coroutineScanner = null;
            }
            
            _coroutineScanner = _coroutinesHandler.StartRoutine(Scan());
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
            Queue<Resource> collectables = new Queue<Resource>();
            
            Collider[] colliders = Physics.OverlapSphere(_centerPoint.position, _radius);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.TryGetComponent(out Resource collectableResource))
                {
                    collectables.Enqueue(collectableResource);
                }
            }
            
            if (collectables.Count > 0)
            {
                ScanFinished?.Invoke(collectables);
            }
        }
    }
}