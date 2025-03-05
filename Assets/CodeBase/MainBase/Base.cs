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
        private Scanner<Resource> _scanner;
        private UnitSpawner _spawner;
        private ResourceHandler _resourceHandler;
        private UnitsHandler _unitsHandler;
        private int _resourcesToSpawnNewUnit;

        public void Initialize(Scanner<Resource> scanner, ResourceHandler resourceHandler, UnitSpawner spawner, ResourceCollector resourceCollector, int resourcesToSpawnNewUnit)
        {
            _spawner = spawner;
            _resourceHandler = resourceHandler;
            
            _scanner = scanner;
            _scanner.ScanFinished += SendTaskToMine;
            
            _unitsHandler = new UnitsHandler();
            
            _resourceCollector = resourceCollector;
            _resourceCollector.ValueChanged += TrySpawnNewUnit;
            
            _resourcesToSpawnNewUnit = resourcesToSpawnNewUnit;
            
            SpawnUnit();
        }

        private void OnDisable()
        {
            _scanner.ScanFinished -= SendTaskToMine;
            _resourceCollector.ValueChanged -= TrySpawnNewUnit;
        }

        private async void SendTaskToMine(List<Resource> resources)
        {
            foreach (var resource in resources)
            {
                Resource resourceToSend = resource;

                if (_resourceHandler.TryGetResource(ref resourceToSend))
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
            
            _unitsHandler.AddNewUnit(unit);
        }
    }
}
        
