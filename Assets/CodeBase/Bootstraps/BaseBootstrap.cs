using CodeBase.MainBase;
using CodeBase.ResourceLogic;
using CodeBase.Services;
using TMPro;
using UnityEngine;

namespace CodeBase.Bootstraps
{
    public class BaseBootstrap : MonoBehaviour
    {
        [SerializeField] private Base _base;
        [SerializeField] private ResourceCollector _resourceCollector;
        [SerializeField] private BaseData _baseData;
        [SerializeField] private TMP_Text _textResourceCollectorValue;
        
        private GameBootstrap _gameBootstrap;
        private Scanner<Resource> _scanner;

        public void Initialize(CoroutinesHandler coroutinesHandler, GameBootstrap gameBootstrap)
        {
            _scanner = new Scanner<Resource>(_baseData.RadiusToSearchResources, _baseData.ScanDelay, _base.transform, coroutinesHandler);
            _base.Initialize(new ResourceHandler(_scanner), new UnitSpawner(_baseData.UnitPrefab, coroutinesHandler), _baseData.ResourcesToSpawnNewUnit);
            
            _gameBootstrap = gameBootstrap;
            
            new ResourceCollectorView(_resourceCollector, _textResourceCollectorValue);
        }
    }
}