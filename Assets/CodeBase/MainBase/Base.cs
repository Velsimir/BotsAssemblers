using System.Collections.Generic;
using System.Linq;
using CodeBase.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.MainBase
{
    [RequireComponent(typeof(SphereCollider))]
    public class Base : MonoBehaviour, IRestartable
    {
        [SerializeField] private BoxCollider _spawnArea;
        
        private Unit.Unit _unit;
        private Scanner<Resource.Resource> _scanner;
        private List<Resource.Resource> _resourcesToCollect;
        private Coroutine _restartCoroutine;

        public void Initialize(BaseData data)
        {
            _unit = data.UnitPrefab;
            
            _resourcesToCollect = new List<Resource.Resource>();
            
            _scanner = new Scanner<Resource.Resource>(data.RadiusToSearchResources, data.ScanDelay, transform);
            _scanner.StartScanning();
            _scanner.ScanFinished += SearchResources;

            SpawnUnits();
        }

        private void OnDisable()
        {
            _scanner.ScanFinished -= SearchResources;
        }

        public void Restart()
        {
            _resourcesToCollect.Clear();

            _unit.Restart();
            
            _scanner.StartScanning();
        }

        private void SpawnUnits()
        {
            float xPosition = Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x);
            float zPosition = Random.Range(_spawnArea.bounds.min.z, _spawnArea.bounds.max.z);
            
            Vector3 spawnPoint = new Vector3(xPosition, _spawnArea.bounds.min.y, zPosition);
            
            Unit.Unit unit = Instantiate(_unit, spawnPoint, Quaternion.identity);
            
            unit.Initialize(spawnPoint);
            
            _unit = unit;
        }

        private void SearchResources()
        {
            List<Resource.Resource> newResources = _scanner.Collectables
                .Where(newResource => !_resourcesToCollect.Contains(newResource))
                .ToList();

            _resourcesToCollect.AddRange(newResources);

            if (_resourcesToCollect.Count > 0)
                _unit.TakeResourceToMine(_resourcesToCollect[0]);
        }
    }
}
        
