using System.Collections;
using System.Collections.Generic;
using CodeBase.Interfaces;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Resource
{
    public class ResourceSpawner : IRestartable
    {
        private readonly Spawner<Resource> _spawner;
        private readonly ResourcePlacer _resourcePlacer;
        private readonly float _spawnInterval;
        private readonly int _minResources = 1;
        private readonly int _maxResources = 3;
        private readonly List<Resource> _allSpawnedResources;
        
        private Coroutine _spawnCoroutine;

        public ResourceSpawner(Spawner<Resource> spawner, ResourcePlacer resourcePlacer, float spawnInterval)
        {
            _spawner = spawner;
            _resourcePlacer = resourcePlacer;
            _spawnInterval = spawnInterval;
            _allSpawnedResources = new List<Resource>();
        }

        public void StartSpawn()
        {
            if (_spawnCoroutine != null)
            {
                CoroutinesHandler.StopRoutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }

            _spawnCoroutine = CoroutinesHandler.StartRoutine(Spawn());
        }

        public void Restart()
        {
            DeactivateAllResources();
            StartSpawn();
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);
                
                SpawnRandomCount();
            }
        }

        private void SpawnRandomCount()
        {
            for (int i = 0; i < Random.Range(_minResources, _maxResources); i++)
            {
                Resource resource = _spawner.Spawn();
                    
                if (_allSpawnedResources.Contains(resource) == false)
                    _allSpawnedResources.Add(resource);
                    
                _resourcePlacer.Put(resource);
            }
        }

        private void DeactivateAllResources()
        {
            foreach (var resource in _allSpawnedResources)
                resource.Restart();
        }
    }
}