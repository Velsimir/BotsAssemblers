using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.ResourceLogic
{
    public class ResourcePlacer
    {
        private readonly List<ResourceNode> _colliders;

        public ResourcePlacer(List<ResourceNode> colliders)
        {
            _colliders = colliders;
        }

        public void Put(Resource resource)
        {
            resource.transform.position = GetRandomPosition(resource);
        }

        private Vector3 GetRandomPosition(Resource resource)
        {
            ResourceNode randomCollider = _colliders[Random.Range(0, _colliders.Count)];
            
            Bounds resourceBounds = resource.Collider.bounds;
            float resourceWidth = resourceBounds.size.x;
            float resourceHalfHeight = resourceBounds.size.y / 2;
            float resourceLength = resourceBounds.size.z;
            
            float positionX = Random.Range(randomCollider.MeshCollider.bounds.min.x + resourceWidth, randomCollider.MeshCollider.bounds.max.x - resourceWidth);
            float positionZ = Random.Range(randomCollider.MeshCollider.bounds.min.z + resourceLength, randomCollider.MeshCollider.bounds.max.z - resourceLength);
            
            return new Vector3(positionX, randomCollider.MeshCollider.bounds.max.y + resourceHalfHeight, positionZ);
        }
    }
}