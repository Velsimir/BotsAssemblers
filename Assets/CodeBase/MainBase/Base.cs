using System.Collections.Generic;
using CodeBase.Interfaces;
using CodeBase.ResourceLogic;
using CodeBase.Services;
using CodeBase.UnitLogic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.MainBase
{
    public class Base : MonoBehaviour, IRestartable
    {
        [SerializeField] private BoxCollider _spawnUnitArea;
        
        private List<Unit> _units;
        private Unit _unitPrefab;
        private CoroutinesHandler _coroutineHandler;
        private ResourceHandler _resourceHandler;

        public void Initialize(Unit unitPrefab, ResourceHandler resourceHandler, int unitCount, CoroutinesHandler coroutineHandler)
        {
            _units = new List<Unit>();
            _resourceHandler = resourceHandler;
            _resourceHandler.NewResourcesAdded += TrySendUnitsToMine;
            
            _coroutineHandler = coroutineHandler;
            
            _unitPrefab = unitPrefab;

            for (int i = 0; i < unitCount; i++)
            {
                SpawnUnits();
            }
        }

        private void OnDisable()
        {
            _resourceHandler.NewResourcesAdded -= TrySendUnitsToMine;
        }

        public void Restart()
        {
            foreach (Unit unit in _units)
            {
                unit.Restart();
            }
        }

        private void TrySendUnitsToMine()
        {
            if (_units.Count > 0 && _resourceHandler.TryGetResource(out Resource resource))
            {
                SendUnitToMine(resource);
            }
        }

        private void SendUnitToMine(Resource resource)
        {
            Unit unit = _units[0];
            unit.TakeResourceToMine(resource);
            _units.Remove(unit);
            unit.ReturnedOnBase += AddFreeUnit;
        }

        private void AddFreeUnit(Unit unit)
        {
            _units.Add(unit);
            unit.ReturnedOnBase -= AddFreeUnit;
            
            TrySendUnitsToMine();
        }

        private void SpawnUnits()
        {
            float xPosition = Random.Range(_spawnUnitArea.bounds.min.x, _spawnUnitArea.bounds.max.x);
            float zPosition = Random.Range(_spawnUnitArea.bounds.min.z, _spawnUnitArea.bounds.max.z);
            
            Vector3 spawnPoint = new Vector3(xPosition, _spawnUnitArea.bounds.min.y, zPosition);
            
            Unit unit = Instantiate(_unitPrefab, spawnPoint, Quaternion.identity);
            
            unit.Initialize(spawnPoint, _coroutineHandler);
            
            _units.Add(unit);
        }
    }
}
        
