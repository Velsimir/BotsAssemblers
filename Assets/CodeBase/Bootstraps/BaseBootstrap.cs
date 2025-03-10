using CodeBase.BaseSpawnerLogic;
using CodeBase.MainBase;
using CodeBase.Services;
using CodeBase.UnitLogic;
using TMPro;
using UnityEngine;

namespace CodeBase.Bootstraps
{
    [RequireComponent(typeof(CoroutinesHandler))]
    public class BaseBootstrap : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textResourceCollectorValue;
        
        private Base _base;
        private Scanner _scanner;

        public void Initialize(ResourceHandler resourceHandler, UnitSpawner unitSpawner, BaseBuilder baseBuilder)
        {
            _base = GetComponent<Base>();
            
            ResourceCollector resourceCollector = new ResourceCollector();
            new ResourceCollectorView(resourceCollector, _textResourceCollectorValue);
            
            CoroutinesHandler coroutinesHandler = GetComponent<CoroutinesHandler>();
            
            _scanner = new Scanner(_base.transform, coroutinesHandler);
            
            _base.Initialize(_scanner, unitSpawner, resourceCollector, resourceHandler, baseBuilder);
        }
    }
}