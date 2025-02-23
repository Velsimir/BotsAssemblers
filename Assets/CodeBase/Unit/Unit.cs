using CodeBase.Interfaces;
using CodeBase.Unit.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Unit
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour, IRestartable
    {
        private UnitStateMachine _stateMachine;
        private Resource.Resource _currentResource;
        
        private Vector3 _basePosition;
        private Quaternion _baseRotation;
        private bool _isBusy;

        public void Initialize(Vector3 basePosition)
        {
            View = new UnitView(GetComponent<Animator>());
            
            NavMesh = new UnitNavMesh(GetComponent<NavMeshAgent>());
            NavMesh.DestinationReached += ChangeState;
            
            _stateMachine = new UnitStateMachine(this);
            
            _basePosition = basePosition;
            _baseRotation = transform.rotation;
            
            _isBusy = false;
        }

        public UnitView View { get; private set; }
        public UnitNavMesh NavMesh { get; private set; }
        public bool IsBusy => _isBusy;

        private void Update()
        {
            if (_stateMachine.CurrentState is RunningState)
                NavMesh.Update(Time.deltaTime);
        }

        private void OnDisable()
        {
            NavMesh.DestinationReached -= ChangeState;
        }

        public void Restart()
        {
            _isBusy = false;
            
            _stateMachine.Switch<IdlingState>();
            
            transform.position = _basePosition;
            transform.rotation = _baseRotation;
        }

        public void TakeResourceToMine(Resource.Resource resource)
        {
            _currentResource = resource;

            NavMesh.SetDestination(_currentResource.transform.position);
            
            _stateMachine.Switch<RunningState>();
            
            _isBusy = true;
        }

        private void ChangeState()
        {

        }
    }
}