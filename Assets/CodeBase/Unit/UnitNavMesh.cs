using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Unit
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitNavMesh : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Vector3 _destination;

        public void Initialize()
        {
            _agent = GetComponent<NavMeshAgent>();
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