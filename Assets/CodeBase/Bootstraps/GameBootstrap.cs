using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Bootstraps
{
    [RequireComponent(typeof(BaseBootstrap))]
    [RequireComponent(typeof(ResourceSpawnerBootstrap))]
    [RequireComponent(typeof(CoroutinesHandler))]
    public class GameBootstrap : MonoBehaviour
    {
        private BaseBootstrap _baseBootstrap;
        private ResourceSpawnerBootstrap _resourceSpawnerBootstrap;
        private CoroutinesHandler _coroutinesHandler;
        
        private void Awake()
        {
            _coroutinesHandler = GetComponent<CoroutinesHandler>();
            
            _baseBootstrap = GetComponent<BaseBootstrap>();
            _baseBootstrap.Initialize(_coroutinesHandler, this);
            
            _resourceSpawnerBootstrap = GetComponent<ResourceSpawnerBootstrap>();
            _resourceSpawnerBootstrap.Initialize(_coroutinesHandler, this);
        }
    }
}