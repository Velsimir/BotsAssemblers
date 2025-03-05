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
        [SerializeField] private Base _base;
        [SerializeField] private ResourceCollector _resourceCollector;
        [SerializeField] private BaseData _baseData;
        [SerializeField] private TMP_Text _textResourceCollectorValue;
        
        private Scanner<Resource> _scanner;

        public void Awake()
        {
            CoroutinesHandler coroutinesHandler = GetComponent<CoroutinesHandler>();
            
            _scanner = new Scanner<Resource>(_baseData.RadiusToSearchResources, _baseData.ScanDelay, _base.transform, coroutinesHandler);
            _base.Initialize(new ResourceSearcher(_scanner), new UnitSpawner(_baseData.UnitPrefab, coroutinesHandler), _resourceCollector, _baseData.ResourcesToSpawnNewUnit);
            
            new ResourceCollectorView(_resourceCollector, _textResourceCollectorValue);
        }
    }
}