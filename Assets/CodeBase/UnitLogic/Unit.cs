using System;
using CodeBase.Interfaces;
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
        
        private UnitStateMachine _stateMachine;
        
        public event Action<Unit> ReturnedOnBase;
        public event Action ResourceCollected;
        public event Action BuildingDone;
        public event Action<ISpawnable> Dissapear;
        
        public UnitAnimator Animator { get; private set; }
        public UnitNavMesh NavMesh { get; private set; }
        public Miner Miner { get; private set; }
        public UnitTask CurrentTask { get; private set; }

        public void Initialize(Vector3 basePosition, CoroutinesHandler coroutinesHandler)
        {
            Animator = new UnitAnimator(GetComponent<Animator>());
            NavMesh = new UnitNavMesh(GetComponent<NavMeshAgent>(), basePosition);
            Miner = new Miner(transform, coroutinesHandler, _resourceHolder);
            _stateMachine = new UnitStateMachine(this);
        }

        private void Update()
        {
            if (_stateMachine.CurrentState is RunningState)
            {
                NavMesh.Update(Time.deltaTime);
            }
        }

        public void Deactivate()
        {
            BuildingDone?.Invoke();
            Dissapear?.Invoke(this);
            gameObject.SetActive(false);
        }

        public void ReleaseUnit()
        {
            ReturnedOnBase?.Invoke(this);
            ResourceCollected?.Invoke();
        }

        public void SendNewTask(Vector3 position, UnitTask currentTask)
        {
            CurrentTask = currentTask;
            GetPositionToRun(position);
        }

        private void GetPositionToRun(Vector3 position)
        {
            NavMesh.SetDestination(position);
            _stateMachine.Switch<RunningState>();
        }
    }

    public enum UnitTask
    {
        Mine,
        Build,
        Collect
    }
}