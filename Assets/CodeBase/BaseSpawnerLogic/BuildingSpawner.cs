using CodeBase.Bootstraps;
using CodeBase.MainBase;
using UnityEngine;

namespace CodeBase.BaseSpawnerLogic
{
    public class BuildingSpawner : MonoBehaviour
    {
        private BaseBootstrap _baseBootstrap;
        private ResourceHandler _resourceHandler;

        public void Initialize(ResourceHandler resourceHandler)
        {
            _baseBootstrap = Resources.Load<BaseBootstrap>("Prefabs/Base");
            _resourceHandler = resourceHandler;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnObjectAtMousePosition();
            }
        }
        
        private void SpawnObjectAtMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                SpawnBase(hit.point);
            }
        }

        private void SpawnBase(Vector3 position)
        {
            BaseBootstrap baseBootstrap = Instantiate(_baseBootstrap, position, Quaternion.identity);
            baseBootstrap.Initialize(_resourceHandler);
        }
    }
}