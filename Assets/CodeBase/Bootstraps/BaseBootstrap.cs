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
        [SerializeField] private BaseData _data;
        [SerializeField] private TMP_Text _textResourceCollectorValue;
        
        private GameBootstrap _gameBootstrap;
        private Scanner<Resource> _scanner;

        public void Initialize(CoroutinesHandler coroutinesHandler, GameBootstrap gameBootstrap)
        {
            _scanner = new Scanner<Resource>(_data.RadiusToSearchResources, _data.ScanDelay, _base.transform, coroutinesHandler);
            _base.Initialize(new ResourceHandler(_scanner), _resourceCollector, new UnitSpawner(_data.UnitPrefab, coroutinesHandler));
            
            _gameBootstrap = gameBootstrap;
            _gameBootstrap.GameRestarted += Restart;
            
            new ResourceCollectorView(_resourceCollector, _textResourceCollectorValue);
        }

        private void Restart()
        {
            _base.Restart();
            _scanner.Restart();
            _resourceCollector.Restart();
        }
    }
}