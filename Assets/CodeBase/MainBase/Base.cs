using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.MainBase
{
    [RequireComponent(typeof(SphereCollider))]
    public class Base : MonoBehaviour, IRestartable
    {
        [SerializeField] private BoxCollider _spawnArea;
        
        private Unit.Unit _unit;
        private Scanner<Resource.Resource> _scanner;
        private float _scanDelay;
        private SphereCollider _searchSphere;
        private List<Resource.Resource> _resourcesToCollect;
        private Coroutine _restartCoroutine;

        public void Initialize(BaseData data)
        {
            _unit = data.UnitPrefab;
            
            _resourcesToCollect = new List<Resource.Resource>();
            
            _searchSphere  = GetComponent<SphereCollider>();
            _searchSphere.radius = data.RadiusToSearchResources;
            
            _scanner = new Scanner<Resource.Resource>(_searchSphere);
            _scanDelay = data.ScanDelay;

            EnableScanningCoroutine();

            SpawnUnits();
        }

        public void Restart()
        {
            _resourcesToCollect.Clear();

            _unit.Restart();
            
            EnableScanningCoroutine();
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

        private void EnableScanningCoroutine()
        {
            if (_restartCoroutine != null)
            {
                StopCoroutine(_restartCoroutine);
                _restartCoroutine = null;
            }

            _restartCoroutine = StartCoroutine(StartScanning());
        }

        private IEnumerator StartScanning()
        {
            while (true)
            {
                yield return new WaitForSeconds(_scanDelay);
                SearchResources();
            }
        }

        private void SearchResources()
        {
            _resourcesToCollect.AddRange(_scanner.GetObjects().
                Where(newResource => _resourcesToCollect.Any(resourceInBase => resourceInBase == newResource) == false));

            if (_resourcesToCollect.Count > 0)
            {
                _unit.TakeResourceToMine(_resourcesToCollect[0]);
            }
        }
    }
}
        
