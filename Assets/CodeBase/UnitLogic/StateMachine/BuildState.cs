using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.UnitLogic.StateMachine
{
    public class BuildState : MovementState
    {
        public BuildState(IStateSwitcher stateSwitcher, Unit unit) : base(stateSwitcher, unit)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter BuildingState");
            Unit.Deactivate();
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Exit BuildingState");
        }
    }
}