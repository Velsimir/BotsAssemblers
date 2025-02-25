using CodeBase.Interfaces;

namespace CodeBase.UnitLogic.StateMachine
{
    public class RunningState : MovementState
    {
        public RunningState(IStateSwitcher stateSwitcher, Unit unit) : base(stateSwitcher, unit)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Unit.Animator.StartRunning();
            Unit.NavMesh.Run();
        }

        public override void Exit()
        {
            base.Exit();
            Unit.Animator.StopRunning();
        }
    }
}