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
        [SerializeField] private GameRestarter _gameRestarter;
        
        private Spawner<Resource> _spawner;
        private ResourcePlacer _resourcePlacer;
        private ResourceSpawner _resourceSpawner;

        private void Awake()
        {
            _spawner = new Spawner<Resource>(_resourcePrefab);
            _resourcePlacer = new ResourcePlacer(GetAllCollidersToSpawn());
            _resourceSpawner = new ResourceSpawner(_spawner, _resourcePlacer, _spawnInterval);

            _resourceSpawner.StartSpawn();
            
            _gameRestarter.GameRestarted += Restart;
        }

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

        private void Restart()
        {
            _resourceSpawner.Restart();
        }
    }
}