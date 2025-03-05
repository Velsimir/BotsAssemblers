using CodeBase.BaseSpawnerLogic;
using CodeBase.MainBase;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Bootstraps
{
    [RequireComponent(typeof(ResourceSpawnerBootstrap))]
    [RequireComponent(typeof(CoroutinesHandler))]
    public class GameBootstrap : MonoBehaviour
    {
        private ResourceSpawnerBootstrap _resourceSpawnerBootstrap;
        private BuildingSpawner _buildingSpawner;
        private CoroutinesHandler _coroutinesHandler;
        private ResourceHandler _resourceHandler;
        
        private void Awake()
        {
            _coroutinesHandler = GetComponent<CoroutinesHandler>();
            
            _resourceSpawnerBootstrap = GetComponent<ResourceSpawnerBootstrap>();
            _resourceSpawnerBootstrap.Initialize(_coroutinesHandler, this);
            
            _resourceHandler = new ResourceHandler(_resourceSpawnerBootstrap.ResourceSpawner);
            
            _buildingSpawner = GetComponent<BuildingSpawner>();
            _buildingSpawner.Initialize(_resourceHandler);
            
        }
    }
}