using CodeBase.Interfaces;
using CodeBase.Unit.StateMachine;
using UnityEngine;

namespace CodeBase.Unit
{
    [RequireComponent(typeof(UnitView))]
    [RequireComponent(typeof(UnitNavMesh))]
    public class Unit : MonoBehaviour, IRestartable
    {
        private UnitStateMachine _stateMachine;
        private DistanceChecker _distanceChecker;
        private bool _isBusy;
        private Resource.Resource _currentResource;
        
        private Vector3 _basePosition;
        private Quaternion _baseRotation;

        public void Initialize(Vector3 basePosition)
        {
            View = GetComponent<UnitView>();
            
            View.Initialize();
            
            NavMesh = GetComponent<UnitNavMesh>();
            NavMesh.Initialize();
            
            _stateMachine = new UnitStateMachine(this);

            _distanceChecker = new DistanceChecker(this);
            
            _basePosition = basePosition;
            _baseRotation = transform.rotation;
            
            _isBusy = false;
        }

        public UnitView View { get; private set; }
        public UnitNavMesh NavMesh { get; private set; }
        public bool IsBusy => _isBusy;

        private void Update()
        {
            _distanceChecker.Update(Time.deltaTime);
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

            NavMesh.SetDestination(resource.transform.position);
            _distanceChecker.SetPoint(resource.transform.position);
            
            _stateMachine.Switch<RunningState>();
            
            _isBusy = true;
        }
    }
}