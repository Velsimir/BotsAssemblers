using CodeBase.BaseSpawnerLogic;
using CodeBase.MainBase;
using CodeBase.MouseInteractLogic;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Bootstraps
{
    [RequireComponent(typeof(ResourceSpawnerBootstrap))]
    [RequireComponent(typeof(CoroutinesHandler))]
    [RequireComponent(typeof(CursorInteractLogic))]
    public class GameBootstrap : MonoBehaviour
    {
        private CursorInteractLogic _cursorInteractLogic;
        private ResourceSpawnerBootstrap _resourceSpawnerBootstrap;
        private BaseBuilder _baseBuilder;
        private CoroutinesHandler _coroutinesHandler;
        private ResourceHandler _resourceHandler;
        
        private void Awake()
        {
            _coroutinesHandler = GetComponent<CoroutinesHandler>();
            
            _resourceSpawnerBootstrap = GetComponent<ResourceSpawnerBootstrap>();
            _resourceSpawnerBootstrap.Initialize(_coroutinesHandler, this);
            
            _resourceHandler = new ResourceHandler(_resourceSpawnerBootstrap.ResourceSpawner);
            
            _cursorInteractLogic = GetComponent<CursorInteractLogic>();
            _baseBuilder = GetComponent<BaseBuilder>();
            _baseBuilder.Initialize(_resourceHandler, _cursorInteractLogic);
        }
    }
}