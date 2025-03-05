using CodeBase.MainBase;
using CodeBase.ResourceLogic;
using CodeBase.Services;
using TMPro;
using UnityEngine;

namespace CodeBase.Bootstraps
{
    [RequireComponent(typeof(CoroutinesHandler))]
    public class BaseBootstrap : MonoBehaviour
    {
        [SerializeField] private BaseData _baseData;
        [SerializeField] private TMP_Text _textResourceCollectorValue;
        
        private Base _base;
        private Scanner _scanner;

        public void Initialize(ResourceHandler resourceHandler)
        {
            _base = GetComponent<Base>();
            ResourceCollector resourceCollector = new ResourceCollector();
            new ResourceCollectorView(resourceCollector, _textResourceCollectorValue);
            
            CoroutinesHandler coroutinesHandler = GetComponent<CoroutinesHandler>();
            UnitSpawner unitSpawner = new UnitSpawner(_baseData.UnitPrefab, coroutinesHandler);
            
            _scanner = new Scanner(_baseData.RadiusToSearchResources, _baseData.ScanDelay, _base.transform, coroutinesHandler);
            _base.Initialize(_scanner, unitSpawner, _baseData.ResourcesToSpawnNewUnit, resourceCollector, resourceHandler);
        }
    }
}