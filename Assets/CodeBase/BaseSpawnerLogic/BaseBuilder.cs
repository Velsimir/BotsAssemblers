using System;
using CodeBase.Bootstraps;
using CodeBase.MainBase;
using CodeBase.MouseInteractLogic;
using CodeBase.UnitLogic;
using UnityEngine;

namespace CodeBase.BaseSpawnerLogic
{
    public class BaseBuilder : MonoBehaviour
    {
        private readonly Vector3 _spawnPositionForFirstBase = Vector3.zero;
        private BaseBootstrap _baseBootstrapPrefab;
        private ResourceHandler _resourceHandler;
        private CursorInteractLogic _cursorInteractLogic;
        private Base _currentBase;
        private Unit _builderUnit;

        public void Initialize(ResourceHandler resourceHandler, CursorInteractLogic cursorInteractLogic)
        {
            _baseBootstrapPrefab = Resources.Load<BaseBootstrap>("Prefabs/Base");
            _resourceHandler = resourceHandler;
            _cursorInteractLogic = cursorInteractLogic;
            _cursorInteractLogic.LeftClick += TrySelectBase;
            _cursorInteractLogic.RightClick += TrySpawnBase;

            InstantiateAndInitializeBase(_spawnPositionForFirstBase);
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
                _currentBase.SendUnitToBuild(hit.point, this);
            }
        }

        private void OnDisable()
        {
            _cursorInteractLogic.LeftClick -= TrySelectBase;
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
            _baseBootstrapPrefab = Instantiate(_baseBootstrapPrefab, position, Quaternion.identity);
            _baseBootstrapPrefab.Initialize(_resourceHandler);

            if (_builderUnit != null)
            {
                _builderUnit.BuildingDone -= CreateBase;
            }
        }
    }
}