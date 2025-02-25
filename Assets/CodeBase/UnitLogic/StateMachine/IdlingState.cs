using CodeBase.Interfaces;

namespace CodeBase.UnitLogic.StateMachine
{
    public class IdlingState : MovementState
    {
        public IdlingState(IStateSwitcher stateSwitcher, Unit unit) : base(stateSwitcher, unit)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Unit.View.StartIdling();
            Unit.NavMesh.Stop();
        }

        public override void Exit()
        {
            base.Exit();
            Unit.View.StopIdling();
        }
    }
}