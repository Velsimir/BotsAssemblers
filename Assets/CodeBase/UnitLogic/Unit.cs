using System;
using CodeBase.Interfaces;
using CodeBase.ResourceLogic;
using CodeBase.Services;
using CodeBase.UnitLogic.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.UnitLogic
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour, ISpawnable
    {
        [SerializeField] private Transform _resourceHolder;
        
        private readonly float _radiusMine = 1f;
        private UnitStateMachine _stateMachine;
        private Vector3 _basePosition;
        private Resource _currentResource;

        public event Action<Unit> ReturnedOnBase;
        public event Action ResourceCollected;
        public event Action<ISpawnable> Dissapear;

        public UnitAnimator Animator { get; private set; }
        public UnitNavMesh NavMesh { get; private set; }
        public Miner Miner { get; private set; }

        public void Initialize(Vector3 basePosition, CoroutinesHandler coroutinesHandler)
        {
            _basePosition = basePosition;

            Animator = new UnitAnimator(GetComponent<Animator>());
            
            NavMesh = new UnitNavMesh(GetComponent<NavMeshAgent>(), _basePosition);
            NavMesh.DestinationReached += ChangeState;
            
            _stateMachine = new UnitStateMachine(this);

            Miner = new Miner(transform, _radiusMine, coroutinesHandler);
            Miner.MiningDone += InteractWithResource;
        }

        private void Update()
        {
            if (_stateMachine.CurrentState is RunningState)
            {
                NavMesh.Update(Time.deltaTime);
            }
        }

        private void OnDisable()
        {
            NavMesh.DestinationReached -= ChangeState;
            Dissapear?.Invoke(this);
        }

        public void TakeResourceToMine(Resource resource)
        {
            NavMesh.SetDestination(resource.transform.position);
            
            _stateMachine.Switch<RunningState>();
        }

        private void ChangeState()
        {
            if (_currentResource != null)
            {
                _stateMachine.Switch<IdleState>();
                _currentResource.Collect();
                ResourceCollected?.Invoke();
                _currentResource = null;
                ReturnedOnBase?.Invoke(this);
            }
            else
            {
                _stateMachine.Switch<MineState>();
            }
        }

        private void InteractWithResource(IInteractable interactable)
        {
            interactable.Interact(_resourceHolder);
            
            NavMesh.SetDestination(_basePosition);
            _stateMachine.Switch<RunningState>();
            
            _currentResource = interactable as Resource;
        }
    }
}