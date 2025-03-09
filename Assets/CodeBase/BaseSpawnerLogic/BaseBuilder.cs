using CodeBase.Bootstraps;
using CodeBase.MainBase;
using UnityEngine;

namespace CodeBase.BaseSpawnerLogic
{
    public class BaseBuilder : MonoBehaviour
    {
        private readonly Vector3 _spawnPositionForFirstBase = Vector3.zero;
        private BaseBootstrap _baseBootstrap;
        private ResourceHandler _resourceHandler;

        public void Initialize(ResourceHandler resourceHandler)
        {
            _baseBootstrap = Resources.Load<BaseBootstrap>("Prefabs/Base");
            _resourceHandler = resourceHandler;
            
            SpawnBase(_spawnPositionForFirstBase);
        }
        
        /*private void SpawnObjectAtMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                SpawnBase(hit.point);
            }
        }*/

        private void SpawnBase(Vector3 position)
        {
            BaseBootstrap baseBootstrap = Instantiate(_baseBootstrap, position, Quaternion.identity);
            baseBootstrap.Initialize(_resourceHandler);
        }
    }
}