using System;
using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.MainBase
{
    [RequireComponent(typeof(BoxCollider))]
    public class ResourceCollector : MonoBehaviour
    {
        public int CollectedResources { get; private set; }

        public event Action ValueChanged;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.transform.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect();
                CollectedResources++;
                ValueChanged?.Invoke();
            }
        }

        public void Spend(int amount)
        {
            if (CollectedResources >= amount )
            {
                CollectedResources -= amount;
                ValueChanged?.Invoke();
            }
        }
    }
}