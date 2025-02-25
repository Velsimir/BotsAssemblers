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
        }

        public override void Exit()
        {
            base.Exit();
            Unit.Animator.StopMining();
        }
    }
}