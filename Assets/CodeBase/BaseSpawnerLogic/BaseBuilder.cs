using CodeBase.Bootstraps;
using CodeBase.MainBase;
using UnityEngine;

namespace CodeBase.BaseSpawnerLogic
{
    public class BaseBuilder : MonoBehaviour
    {
        private readonly Vector3 _spawnPositionForFirstBase = Vector3.zero;
        private BaseBootstrap _baseBootstrap;
        private BaseFlag _baseFlag;
        private ResourceHandler _resourceHandler;

        public void Initialize(ResourceHandler resourceHandler)
        {
            _baseBootstrap = Resources.Load<BaseBootstrap>("Prefabs/Base");
            _baseFlag = Resources.Load<BaseFlag>("Prefabs/Flag");
            _resourceHandler = resourceHandler;

            CreateFirstBase();
        }
        
        private void SpawnObjectAtMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                SetFlag(hit.point);
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
            _baseBootstrap = Instantiate(_baseBootstrap, _spawnPositionForFirstBase, Quaternion.identity);
            _baseBootstrap.Initialize(_resourceHandler);
        }
    }
}