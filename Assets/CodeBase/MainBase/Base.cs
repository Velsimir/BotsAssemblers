using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.BaseSpawnerLogic;
using CodeBase.ResourceLogic;
using CodeBase.UnitLogic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace CodeBase.MainBase
{
    public class Base : MonoBehaviour
    {
        [SerializeField] private BoxCollider _spawnUnitArea;
        [SerializeField] private BaseFlag _flag;

        private const int MinUnitsToBuildNewBase = 3;
        private const int ResourcesToSpawnNewUnit = 3;
        private const int ResourcesToSpawnNewBase = 5;
        
        private ResourceCollector _resourceCollector;
        private Scanner _scanner;
        private UnitSpawner _spawner;
        private UnitsHandler _unitsHandler;
        private ResourceHandler _resourceHandler;
        private BaseBuilder _baseBuilder;
        private Stage _currentStage;
        
        public void Initialize(Scanner scanner, UnitSpawner spawner, ResourceCollector resourceCollector, ResourceHandler resourceHandler, BaseBuilder baseBuilder)
        {
            _baseBuilder = baseBuilder;
            _spawner = spawner;
            _resourceCollector = resourceCollector;

            _resourceHandler = resourceHandler;
            
            _scanner = scanner;
            _scanner.ScanFinished += SendTaskToMine;

            _unitsHandler = new UnitsHandler();
            SpawnUnit();

            _flag = Instantiate(_flag);
            _flag.gameObject.SetActive(false);
            
            _currentStage = Stage.Mining;
        }

        private void OnDisable()
        {
            _scanner.ScanFinished -= SendTaskToMine;

            foreach (var unit in _unitsHandler.AllUnits)
            {
                unit.ResourceCollected -= TrySpendCollectedResource;
            }
        }

        public void SendUnitToBuild(Vector3 position)
        {
            if (_unitsHandler.AllUnits.Count >= MinUnitsToBuildNewBase)
            {
                if (_currentStage == Stage.Mining)
                {
                    _currentStage = Stage.Building;
                    _flag.gameObject.SetActive(true);
                }
                
                _flag.Move(position);
            }
            else
            {
                Debug.Log("Not enough units to build");
            }
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

        private void TrySpendCollectedResource()
        {
            _resourceCollector.IncreaseResources();
            
            switch (_currentStage)
            {
                case Stage.Mining:
                    TrySpawnNewUnit();
                    break;
                
                case Stage.Building:
                    TrySendUnitTuBuild();
                    break;
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
            unit.ResourceCollected += TrySpendCollectedResource;
            _unitsHandler.AddNewUnit(unit);
        }

        private async Task TrySendUnitTuBuild()
        {
            if (_resourceCollector.CollectedResources >= ResourcesToSpawnNewBase)
            {
                _resourceCollector.Spend(ResourcesToSpawnNewBase);
                _currentStage = Stage.Mining;
                
                await _unitsHandler.SendUnitToBuildAsync(_flag.transform.position);
                _baseBuilder.TakeUnitBuilder(_unitsHandler.BuilderUnit);
            }
        }
    }

    public enum Stage
    {
        Mining,
        Building,
    }
}
        
