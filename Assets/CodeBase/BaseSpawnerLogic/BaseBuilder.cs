using CodeBase.Bootstraps;
using CodeBase.MainBase;
using CodeBase.MouseInteractLogic;
using CodeBase.UnitLogic;
using UnityEngine;

namespace CodeBase.BaseSpawnerLogic
{
    public class BaseBuilder : MonoBehaviour
    {
        private BaseBootstrap _baseBootstrap;
        private CursorInteractLogic _cursorInteractLogic;
        private ResourceHandler _resourceHandler;
        private UnitSpawner _unitSpawner;
        private Base _currentBase;
        private Unit _builderUnit;

        public void Initialize(ResourceHandler resourceHandler, CursorInteractLogic cursorInteractLogic, UnitSpawner unitSpawner)
        {
            _baseBootstrap = Resources.Load<BaseBootstrap>("Prefabs/Base");
            _resourceHandler = resourceHandler;
            _cursorInteractLogic = cursorInteractLogic;
            _unitSpawner = unitSpawner;
            
            _cursorInteractLogic.LeftClick += TrySelectBase;
            _cursorInteractLogic.RightClick += TrySpawnBase;

            InstantiateAndInitializeBase(Vector3.zero);
        }

        private void OnDisable()
        {
            _cursorInteractLogic.LeftClick -= TrySelectBase;
        }

        public void TakeUnitBuilder(Unit unit)
        {
            _builderUnit = unit;
            _builderUnit.BuildingDone += CreateBase;
        }

        private void TrySpawnBase(RaycastHit hit)
        {
            if (_currentBase == null)
                return;
            
            if (hit.collider.gameObject.TryGetComponent(out ConstructionZone constructionZone))
            {
                _currentBase.SendUnitToBuild(hit.point);
            }
        }

        private void TrySelectBase(RaycastHit hit)
        {
            if (hit.collider.gameObject.TryGetComponent(out Base @base))
            {
                _currentBase = @base;
            }
            else
            {
                _currentBase = null;
            }
        }

        private void CreateBase()
        {
            InstantiateAndInitializeBase(_builderUnit.transform.position);
        }

        private void InstantiateAndInitializeBase(Vector3 position)
        {
            BaseBootstrap baseBootstrapPrefab = Instantiate(_baseBootstrap, position, Quaternion.identity);
            baseBootstrapPrefab.Initialize(_resourceHandler, _unitSpawner);

            if (_builderUnit != null)
            {
                _builderUnit.BuildingDone -= CreateBase;
            }
        }
    }
}