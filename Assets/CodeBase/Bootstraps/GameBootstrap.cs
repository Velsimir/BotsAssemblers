using CodeBase.BaseSpawnerLogic;
using CodeBase.MainBase;
using CodeBase.MouseInteractLogic;
using CodeBase.Services;
using CodeBase.UnitLogic;
using UnityEngine;
using Resource = CodeBase.ResourceLogic.Resource;

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
            Resource resourcePrefab = Resources.Load<Resource>("Prefabs/Resource");
            Unit unitPrefab = Resources.Load<Unit>("Prefabs/Unit");
            
            _coroutinesHandler = GetComponent<CoroutinesHandler>();

            _resourceSpawnerBootstrap = GetComponent<ResourceSpawnerBootstrap>();
            _resourceSpawnerBootstrap.Initialize(_coroutinesHandler, new Spawner<Resource>(resourcePrefab));
            
            _resourceHandler = new ResourceHandler(_resourceSpawnerBootstrap.ResourceSpawner);
            
            _cursorInteractLogic = GetComponent<CursorInteractLogic>();
            _baseBuilder = GetComponent<BaseBuilder>();
            _baseBuilder.Initialize(_resourceHandler, _cursorInteractLogic, new UnitSpawner(unitPrefab, _coroutinesHandler));
        }
    }
}