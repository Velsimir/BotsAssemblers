using System.Collections.Generic;
using CodeBase.ResourceLogic;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Bootstraps
{
    public class ResourceSpawnerBootstrap : MonoBehaviour
    {
        [SerializeField] private Resource _resourcePrefab;
        [SerializeField] private float _radiusToSpawn;
        [SerializeField] private float _spawnInterval;
        
        private GameBootstrap _gameBootstrap;
        private Spawner<Resource> _spawner;
        private ResourcePlacer _resourcePlacer;
        private ResourceSpawner _resourceSpawner;

        public void Initialize(CoroutinesHandler coroutinesHandler, GameBootstrap gameBootstrap)
        {
            _spawner = new Spawner<Resource>(_resourcePrefab);
            _resourcePlacer = new ResourcePlacer(GetAllCollidersToSpawn());
            _resourceSpawner = new ResourceSpawner(_spawner, _resourcePlacer, _spawnInterval, coroutinesHandler);
            
            _resourceSpawner.StartSpawn();
            
            _gameBootstrap = gameBootstrap;
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