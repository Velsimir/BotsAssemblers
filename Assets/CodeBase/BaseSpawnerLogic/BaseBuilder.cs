using System;
using CodeBase.Bootstraps;
using CodeBase.MainBase;
using CodeBase.MouseInteractLogic;
using UnityEngine;

namespace CodeBase.BaseSpawnerLogic
{
    public class BaseBuilder : MonoBehaviour
    {
        private readonly Vector3 _spawnPositionForFirstBase = Vector3.zero;
        private BaseBootstrap _baseBootstrapPrefab;
        private BaseFlag _baseFlag;
        private ResourceHandler _resourceHandler;
        private CursorInteractLogic _cursorInteractLogic;
        private Base _currentBase;

        public void Initialize(ResourceHandler resourceHandler, CursorInteractLogic cursorInteractLogic)
        {
            _baseBootstrapPrefab = Resources.Load<BaseBootstrap>("Prefabs/Base");
            _baseFlag = Resources.Load<BaseFlag>("Prefabs/Flag");
            _resourceHandler = resourceHandler;
            _cursorInteractLogic = cursorInteractLogic;
            _cursorInteractLogic.LeftClick += TrySelectBase;
            _cursorInteractLogic.RightClick += TrySpawnBase;

            CreateFirstBase();
        }

        private void TrySpawnBase(RaycastHit hit)
        {
            if (_currentBase == null || _currentBase.IsEnoughUnits == false)
                return;
            
            if (hit.collider.gameObject.TryGetComponent(out ConstructionZone constructionZone))
            {
                SetFlag(hit.point);
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

        private void SetFlag(Vector3 position)
        {
            if (_baseFlag.IsInitialized == false)
            {
                _baseFlag = Instantiate(_baseFlag);
                _baseFlag.Initialize();
            }
            
            _baseFlag.Move(position);
        }

        private void CreateFirstBase()
        {
            _baseBootstrapPrefab = Instantiate(_baseBootstrapPrefab, _spawnPositionForFirstBase, Quaternion.identity);
            _baseBootstrapPrefab.Initialize(_resourceHandler);
        }
    }
}