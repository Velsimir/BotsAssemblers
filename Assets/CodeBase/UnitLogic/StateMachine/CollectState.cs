using CodeBase.Interfaces;

namespace CodeBase.UnitLogic.StateMachine
{
    public class CollectState : MovementState
    {
        public CollectState(IStateSwitcher stateSwitcher, Unit unit) : base(stateSwitcher, unit)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Unit.Miner.Resource.Collect();
            GetNextState();
        }

        public override void GetNextState()
        {
            StateSwitcher.Switch<IdleState>();
            Unit.ReleaseUnit();
        }
    }
}