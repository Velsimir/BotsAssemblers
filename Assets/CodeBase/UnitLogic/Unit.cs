using System;
using CodeBase.Interfaces;
using CodeBase.ResourceLogic;
using CodeBase.Services;
using CodeBase.UnitLogic.StateMachine;
using CodeBase.UnitLogic.UnitTaskLogic;
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

        public event Action<Unit> ReturnedOnBase;
        public event Action ResourceCollected;
        public event Action<ISpawnable> Dissapear;

        public UnitAnimator Animator { get; private set; }
        public UnitNavMesh NavMesh { get; private set; }
        public Miner Miner { get; private set; }
        public UnitStateMachine StateMachine { get; private set; }

        public void Initialize(Vector3 basePosition, CoroutinesHandler coroutinesHandler)
        {
            Animator = new UnitAnimator(GetComponent<Animator>());
            NavMesh = new UnitNavMesh(GetComponent<NavMeshAgent>(), basePosition);
            StateMachine = new UnitStateMachine(this);

            Miner = new Miner(transform, _radiusMine, coroutinesHandler, _resourceHolder);
        }

        private void Update()
        {
            if (StateMachine.CurrentState is RunningState)
            {
                NavMesh.Update(Time.deltaTime);
            }
        }

        private void OnDisable()
        {
            Dissapear?.Invoke(this);
        }

        public void GetPositionForNewBase(Vector3 position)
        {
            NavMesh.SetDestination(position);
            StateMachine.Switch<RunningState>();
        }

        public void TakeResourceToMine(Resource resource)
        {
            NavMesh.SetDestination(resource.transform.position);
            StateMachine.Switch<RunningState>();
        }
    }
}