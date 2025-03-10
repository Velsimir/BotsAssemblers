using System.Collections.Generic;
using CodeBase.ResourceLogic;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Bootstraps
{
    public class ResourceSpawnerBootstrap : MonoBehaviour
    {
        [SerializeField] private float _radiusToSpawn;
        [SerializeField] private float _spawnInterval;
        
        private Spawner<Resource> _spawner;
        private ResourcePlacer _resourcePlacer;
        private ResourceSpawner _resourceSpawner;

        public void Initialize(CoroutinesHandler coroutinesHandler, Spawner<Resource> spawner)
        {
            _resourcePlacer = new ResourcePlacer(GetAllCollidersToSpawn());
            _resourceSpawner = new ResourceSpawner(spawner, _resourcePlacer, _spawnInterval, coroutinesHandler);
            
            _resourceSpawner.StartSpawn();
        }
        
        public ResourceSpawner ResourceSpawner => _resourceSpawner;

        private List<ResourceNode> GetAllCollidersToSpawn()
        {
            List<ResourceNode> colliders = new List<ResourceNode>();

            foreach (Collider foundedCollider in Physics.OverlapSphere(transform.position, _radiusToSpawn))
            {
                if (foundedCollider.TryGetComponent(out ResourceNode resourceNode))
                {
                    resourceNode.Initialize();
                    colliders.Add(resourceNode);
                }
            }
            
            return colliders;
        }
    }
}