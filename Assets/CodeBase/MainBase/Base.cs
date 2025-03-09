using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.BaseSpawnerLogic;
using CodeBase.ResourceLogic;
using CodeBase.UnitLogic;
using UnityEngine;

namespace CodeBase.MainBase
{
    public class Base : MonoBehaviour
    {
        [SerializeField] private BoxCollider _spawnUnitArea;

        private const int MinUnitsToBuildNewBase = 3;
        private const int ResourcesToSpawnNewUnit = 3;
        
        private ResourceCollector _resourceCollector;
        private Scanner _scanner;
        private UnitSpawner _spawner;
        private UnitsHandler _unitsHandler;
        private ResourceHandler _resourceHandler;
        private BaseFlag _flag;
        
        public void Initialize(Scanner scanner, UnitSpawner spawner, ResourceCollector resourceCollector, ResourceHandler resourceHandler)
        {
            _spawner = spawner;
            _flag = Resources.Load<BaseFlag>("Prefabs/Flag");
            _resourceCollector = resourceCollector;
            _resourceCollector.ValueChanged += TrySpawnNewUnit;

            _resourceHandler = resourceHandler;
            
            _scanner = scanner;
            _scanner.ScanFinished += SendTaskToMine;

            _unitsHandler = new UnitsHandler();
            SpawnUnit();
        }

        private void OnDisable()
        {
            _scanner.ScanFinished -= SendTaskToMine;
            _resourceCollector.ValueChanged -= TrySpawnNewUnit;

            foreach (var unit in _unitsHandler.AllUnits)
            {
                unit.ResourceCollected -= CollectResource;
            }
        }

        public async Task SendUnitToBuild(Vector3 position, BaseBuilder baseBuilder)
        {
            if (_unitsHandler.AllUnits.Count < MinUnitsToBuildNewBase)
            {
                Debug.Log("Слишком мало юнитов");
                return;
            }

            SetFlag(position);
            await _unitsHandler.SendUnitToBuildAsync(position);
            baseBuilder.TakeUnitBuilder(_unitsHandler.BuilderUnit);
        }
        
        private void SetFlag(Vector3 position)
        {
            if (_flag.IsInitialized == false)
            {
                _flag = Instantiate(_flag);
                _flag.Initialize();
            }
            
            _flag.gameObject.SetActive(true);
            _flag.Move(position);
        }

        private void SendTaskToMine(Queue<Resource> resources)
        {
            while (resources.Count > 0)
            {
                Resource resource = resources.Dequeue();
                
                if (_unitsHandler.HasFreeTasks && _resourceHandler.TryGetFreeResource(ref resource))
                {
                    _unitsHandler.SendTaskToMineAsync(resource.transform.position);
                }
            } 
        }

        private void TrySpawnNewUnit()
        {
            if (_resourceCollector.CollectedResources >= ResourcesToSpawnNewUnit)
            {
                SpawnUnit();
                _resourceCollector.Spend(ResourcesToSpawnNewUnit);
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
    }
}
        
