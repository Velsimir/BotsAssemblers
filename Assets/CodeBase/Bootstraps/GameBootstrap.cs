using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Bootstraps
{
    [RequireComponent(typeof(ResourceSpawnerBootstrap))]
    [RequireComponent(typeof(CoroutinesHandler))]
    public class GameBootstrap : MonoBehaviour
    {
        private ResourceSpawnerBootstrap _resourceSpawnerBootstrap;
        private CoroutinesHandler _coroutinesHandler;
        
        private void Awake()
        {
            _coroutinesHandler = GetComponent<CoroutinesHandler>();
            
            _resourceSpawnerBootstrap = GetComponent<ResourceSpawnerBootstrap>();
            _resourceSpawnerBootstrap.Initialize(_coroutinesHandler, this);
        }
    }
}