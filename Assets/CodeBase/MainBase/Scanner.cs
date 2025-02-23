using System.Collections.Generic;
using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.MainBase
{
    public class Scanner<TObjectToSearch> where TObjectToSearch : ICollectable
    {
        private readonly SphereCollider _collider;
        private List<TObjectToSearch> _collectables;
        
        public Scanner(SphereCollider collider)
        {
            _collider = collider;
        }

        public List<TObjectToSearch> GetObjects()
        {
            _collectables = new List<TObjectToSearch>();
            
            Collider[] colliders = Physics.OverlapSphere(_collider.transform.position, _collider.radius);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.TryGetComponent(out TObjectToSearch collectableResource))
                    if(collectableResource.IsCollected == false)
                        _collectables.Add(collectableResource);
            }
            
            return new List<TObjectToSearch>(_collectables);
        }
    }
}