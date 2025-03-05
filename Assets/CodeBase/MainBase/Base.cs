using System.Collections.Generic;
using CodeBase.Interfaces;
using CodeBase.ResourceLogic;
using CodeBase.Services;
using CodeBase.UnitLogic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.MainBase
{
    public class Base : MonoBehaviour, IRestartable
    {
        [SerializeField] private BoxCollider _spawnUnitArea;
        
        private List<Unit> _units;
        private ResourceHandler _resourceHandler;

        public void Initialize(ResourceHandler resourceHandler, UnitSpawner spawner)
        {
            _units = new List<Unit>();
            _resourceHandler = resourceHandler;
            _resourceHandler.NewResourcesAdded += TrySendUnitsToMine;

            _units.Add(spawner.GetUnit(_spawnUnitArea));
        }

        private void OnDisable()
        {
            _resourceHandler.NewResourcesAdded -= TrySendUnitsToMine;
        }

        public void Restart()
        {
            foreach (Unit unit in _units)
            {
                unit.Restart();
            }
        }

        private void TrySendUnitsToMine()
        {
            if (_units.Count > 0 && _resourceHandler.TryGetResource(out Resource resource))
            {
                SendUnitToMine(resource);
            }
        }

        private void SendUnitToMine(Resource resource)
        {
            Unit unit = _units[0];
            unit.TakeResourceToMine(resource);
            _units.Remove(unit);
            unit.ReturnedOnBase += AddFreeUnit;
        }

        private void AddFreeUnit(Unit unit)
        {
            _units.Add(unit);
            unit.ReturnedOnBase -= AddFreeUnit;
            
            TrySendUnitsToMine();
        }
    }
}
        
