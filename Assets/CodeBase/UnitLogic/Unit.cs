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
    public class Unit : MonoBehaviour, IRestartable
    {
        [SerializeField] private Transform _resourceHolder;
        
        private UnitStateMachine _stateMachine;
        private Vector3 _basePosition;
        private Quaternion _baseRotation;
        private bool _isBackPackFull;
        private float _radiusMine = 1f;

        public event Action<Unit> ReturnedOnBase; 

        public UnitAnimator Animator { get; private set; }
        public UnitNavMesh NavMesh { get; private set; }
        public Miner Miner { get; private set; }

        public void Initialize(Vector3 basePosition, CoroutinesHandler coroutinesHandler)
        {
            Animator = new UnitAnimator(GetComponent<Animator>());
            
            NavMesh = new UnitNavMesh(GetComponent<NavMeshAgent>());
            NavMesh.DestinationReached += ChangeState;
            
            _stateMachine = new UnitStateMachine(this);
            
            _basePosition = basePosition;
            _baseRotation = transform.rotation;

            Miner = new Miner(transform, _radiusMine, coroutinesHandler);
            Miner.MiningDone += InteractWithResource;
            
            _isBackPackFull = false;
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
        }

        public void Restart()
        {
            _isBackPackFull = false;
            
            _stateMachine.Switch<IdleState>();
            
            transform.position = _basePosition;
            transform.rotation = _baseRotation;
        }

        public void TakeResourceToMine(Resource resource)
        {
            NavMesh.SetDestination(resource.transform.position);
            
            _stateMachine.Switch<RunningState>();
        }

        private void ChangeState()
        {
            if (_isBackPackFull)
            {
                _stateMachine.Switch<IdleState>();
                _isBackPackFull = false;
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
            
            _isBackPackFull = true;
        }
    }
}