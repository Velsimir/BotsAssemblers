using CodeBase.Interfaces;

namespace CodeBase.UnitLogic.StateMachine
{
    public class MineState : MovementState
    {
        public MineState(IStateSwitcher stateSwitcher, Unit unit) : base(stateSwitcher, unit)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Unit.Animator.StartMining();
            Unit.Miner.StartMine();
            Unit.Miner.MiningDone += GetNextState;
        }

        public override void Exit()
        {
            base.Exit();
            Unit.Miner.MiningDone -= GetNextState;
            Unit.Animator.StopMining();
        }

        public override void GetNextState()
        {
            base.GetNextState();
            
            Unit.SendNewTask(Unit.NavMesh.BasePosition, UnitTask.Collect);
            StateSwitcher.Switch<RunningState>();
        }
    }
}