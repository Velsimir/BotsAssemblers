using CodeBase.Interfaces;

namespace CodeBase.UnitLogic.StateMachine
{
    public class MiningState : MovementState
    {
        public MiningState(IStateSwitcher stateSwitcher, Unit unit) : base(stateSwitcher, unit)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Unit.Animator.StartMining();
            Unit.Miner.StartMining();
        }

        public override void Exit()
        {
            base.Exit();
            Unit.Animator.StopMining();
        }
    }
}