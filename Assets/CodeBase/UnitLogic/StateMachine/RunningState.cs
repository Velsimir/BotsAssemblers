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
            Unit.View.StartRunning();
            Unit.NavMesh.Run();
        }

        public override void Exit()
        {
            base.Exit();
            Unit.View.StopRunning();
        }
    }
}