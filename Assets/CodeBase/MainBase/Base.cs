using System.Collections.Generic;
using CodeBase.ResourceLogic;
using CodeBase.UnitLogic;
using UnityEngine;

namespace CodeBase.MainBase
{
    public class Base : MonoBehaviour
    {
        [SerializeField] private BoxCollider _spawnUnitArea;

        private ResourceCollector _resourceCollector;
        private Scanner _scanner;
        private UnitSpawner _spawner;
        private UnitsHandler _unitsHandler;
        private ResourceHandler _resourceHandler;
        private int _resourcesToSpawnNewUnit;

        public void Initialize(Scanner scanner, UnitSpawner spawner, int resourcesToSpawnNewUnit, ResourceCollector resourceCollector, ResourceHandler resourceHandler)
        {
            _spawner = spawner;

            _resourceCollector = resourceCollector;
            _resourceCollector.ValueChanged += TrySpawnNewUnit;

            _resourceHandler = resourceHandler;
            
            _scanner = scanner;
            _scanner.ScanFinished += SendTaskToMine;

            _unitsHandler = new UnitsHandler();

            _resourcesToSpawnNewUnit = resourcesToSpawnNewUnit;

            SpawnUnit();
        }

        private void OnDisable()
        {
            _scanner.ScanFinished -= SendTaskToMine;
            _resourceCollector.ValueChanged -= TrySpawnNewUnit;
        }

        private async void SendTaskToMine(Queue<Resource> resources)
        {
            while (_unitsHandler.HasFreeUnits && resources.Count > 0)
            {
                Resource resource = resources.Dequeue();

                if (_resourceHandler.TryGetResource(ref resource))
                {
                    await _unitsHandler.SendUnitToMineAsync(resource);
                }
  
            } 
        }

        private void TrySpawnNewUnit()
        {
            if (_resourceCollector.CollectedResources >= _resourcesToSpawnNewUnit)
            {
                SpawnUnit();
                _resourceCollector.Spend(_resourcesToSpawnNewUnit);
            }
        }

        private void SpawnUnit()
        {
            Unit unit = _spawner.GetUnit(_spawnUnitArea);
            unit.ResourceCollected += CollectResource;
            _unitsHandler.AddNewUnit(unit);
        }

        private void CollectResource()
        {
            _resourceCollector.IncreaseResources();
        }

        private void RemoveUnitFromBase()
        {
            //unit.ResourceCollected -= CollectResource;
        }
    }
}
        
