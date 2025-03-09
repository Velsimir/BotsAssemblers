using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UnitLogic
{
    public class UnitSpawner
    {
        private readonly Spawner<Unit> _spawner;
        private readonly CoroutinesHandler _coroutineHandler;

        public UnitSpawner(Unit unitPrefab, CoroutinesHandler coroutinesHandler)
        {
            _spawner = new Spawner<Unit>(unitPrefab);
            _coroutineHandler = coroutinesHandler;
        }

        public Unit GetUnit(Collider baseArea)
        {
            Unit unit = _spawner.Spawn();
            
            unit.Initialize(GetSpawnPosition(baseArea), _coroutineHandler);
            
            return unit;
        }

        private Vector3 GetSpawnPosition(Collider spawnArea)
        {
            float xPosition = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            float zPosition = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);
            
            Vector3 spawnPoint = new Vector3(xPosition, spawnArea.bounds.min.y, zPosition);
            
            return spawnPoint;
        }
    }
}