using System;
using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.MainBase
{
    public class ResourceCollector : MonoBehaviour, IRestartable
    {
        private int _collectedResources;
        
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.transform.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect();
                _collectedResources++;
                ValueChanged.Invoke(_collectedResources);
            }
        }

        public event Action<int> ValueChanged;

        public void Restart()
        {
            _collectedResources = 0;
            ValueChanged.Invoke(_collectedResources);
        }
    }
}