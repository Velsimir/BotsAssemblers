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

            Unit.NavMesh.DestinationReached += GetNextState;
        }

        public override void Exit()
        {
            base.Exit();
            Unit.NavMesh.DestinationReached -= GetNextState;
            Unit.Animator.StopRunning();
        }

        public override void GetNextState()
        {
            base.GetNextState();

            switch (Unit.CurrentTask)
            {
                case UnitTask.Mine:
                    StateSwitcher.Switch<MineState>();
                    break;
                
                case UnitTask.Collect:
                    StateSwitcher.Switch<CollectState>();
                    break;
                
                case UnitTask.Build:
                    StateSwitcher.Switch<BuildState>();
                    break;
            }
        }
    }
}