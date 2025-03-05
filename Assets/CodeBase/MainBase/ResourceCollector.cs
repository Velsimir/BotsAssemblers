using System;
using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.MainBase
{
    public class ResourceCollector : MonoBehaviour
    {
        private int _collectedResources;

        public event Action<int> ValueChanged;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.transform.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect();
                _collectedResources++;
                ValueChanged?.Invoke(_collectedResources);
            }
        }

        private void Spend(int amount)
        {
            if (_collectedResources >= amount )
            {
                _collectedResources -= amount;
            }
        }
    }
}