using CodeBase.ResourceLogic;
using CodeBase.UnitLogic;
using UnityEngine;

namespace CodeBase.MainBase
{
    public class Base : MonoBehaviour
    {
        [SerializeField] private BoxCollider _spawnUnitArea;
        [SerializeField] private ResourceCollector _resourceCollector;
        
        private ResourceHandler _resourceHandler;
        private UnitSpawner _spawner;
        private UnitsHandler _unitsHandler;
        private int _resourcesToSpawnNewUnit;

        public void Initialize(ResourceHandler resourceHandler, UnitSpawner spawner, int resourcesToSpawnNewUnit)
        {
            _spawner = spawner;
            _resourceHandler = resourceHandler;
            _resourceHandler.NewResourcesAdded += SendTaskToMine;
            _unitsHandler = new UnitsHandler();

            _resourcesToSpawnNewUnit = resourcesToSpawnNewUnit;
            
            SpawnUnit();
        }

        private void OnDisable()
        {
            _resourceHandler.NewResourcesAdded -= SendTaskToMine;
        }

        private async void SendTaskToMine()
        {
            while (_resourceHandler.TryGetResource(out Resource resource))
            {
                await _unitsHandler.SendUnitToMineAsync(resource);
            }
        }

        private void TrySpawnNewUnit()
        {
            //if (_resourceHandler)
            {
                
            }
        }

        private void SpawnUnit()
        {
            Unit unit = _spawner.GetUnit(_spawnUnitArea);
            
            _unitsHandler.AddNewUnit(unit);
        }
    }
}
        
