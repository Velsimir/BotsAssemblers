using System;
using CodeBase.Interfaces;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UnitLogic
{
    public class DistanceChecker : IUpdatable
    {
        private const float MinDistance = 0.5f;
        
        private readonly Unit _unit;
        
        private Vector3 _target;
        private bool _isTargetReached;

        public DistanceChecker(Unit unit)
        {
            _unit = unit;
            _isTargetReached = true;
        }

        public event Action TargetReached;

        public void Update(float deltaTime)
        {
            if (_isTargetReached)
                return;
            
            Check();
        }

        private void Check()
        {
            if (_unit.transform.position.IsEnoughDistance(_target, MinDistance))
            {
                TargetReached?.Invoke();
                _isTargetReached = true;
            }
        }

        public void SetPoint(Vector3 destination)
        {
            _target = destination;
            _isTargetReached = false;
        }
    }
}