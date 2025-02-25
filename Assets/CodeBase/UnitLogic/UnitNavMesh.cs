using System;
using CodeBase.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.UnitLogic
{
    public class UnitNavMesh : IUpdatable
    {
        private readonly NavMeshAgent _agent;
        private Vector3 _destination;

        public UnitNavMesh(NavMeshAgent agent)
        {
            _agent = agent;
        }

        public event Action DestinationReached;

        public void Update(float deltaTime)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
            {
                DestinationReached?.Invoke();
            }
        }

        public void SetDestination(Vector3 destination)
        {
            _destination = destination;
        }

        public void Run()
        {
            _agent.SetDestination(_destination);
            _agent.isStopped = false;
        }

        public void Stop()
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
        }
    }
}