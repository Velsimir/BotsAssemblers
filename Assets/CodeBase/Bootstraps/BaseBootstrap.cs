using CodeBase.MainBase;
using CodeBase.ResourceLogic;
using TMPro;
using UnityEngine;

namespace CodeBase.Bootstraps
{
    public class BaseBootstrap : MonoBehaviour
    {
        [SerializeField] private Base _base;
        [SerializeField] private ResourceCollector _resourceCollector;
        [SerializeField] private BaseData _data;
        [SerializeField] private GameRestarter _gameRestarter;
        [SerializeField] private TMP_Text _textResourceCollectorValue;
        
        private Scanner<Resource> _scanner;

        private void Awake()
        {
            _scanner = new Scanner<Resource>(_data.RadiusToSearchResources, _data.ScanDelay, _base.transform);
            _base.Initialize(_data.UnitPrefab, new ResourceHandler(_scanner), _data.CountOfInitialUnits);
            _gameRestarter.GameRestarted += Restart;
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