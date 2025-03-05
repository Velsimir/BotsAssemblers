using CodeBase.ResourceLogic;
using CodeBase.UnitLogic;
using UnityEngine;

namespace CodeBase.MainBase
{
    public class Base : MonoBehaviour
    {
        [SerializeField] private BoxCollider _spawnUnitArea;
        
        private ResourceCollector _resourceCollector;
        private ResourceSearcher _resourceSearcher;
        private UnitSpawner _spawner;
        private UnitsHandler _unitsHandler;
        private int _resourcesToSpawnNewUnit;

        public void Initialize(ResourceSearcher resourceSearcher, UnitSpawner spawner, ResourceCollector resourceCollector, int resourcesToSpawnNewUnit)
        {
            _spawner = spawner;
            
            _resourceSearcher = resourceSearcher;
            _resourceSearcher.NewResourcesFounded += SendTaskToMine;
            
            _unitsHandler = new UnitsHandler();
            
            _resourceCollector = resourceCollector;
            _resourceCollector.ValueChanged += TrySpawnNewUnit;
            
            _resourcesToSpawnNewUnit = resourcesToSpawnNewUnit;
            
            SpawnUnit();
        }

        private void OnDisable()
        {
            _resourceSearcher.NewResourcesFounded -= SendTaskToMine;
            _resourceCollector.ValueChanged -= TrySpawnNewUnit;
        }

        private async void SendTaskToMine()
        {
            while (_resourceSearcher.TryGetResource(out Resource resource))
            {
                await _unitsHandler.SendUnitToMineAsync(resource);
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
        
