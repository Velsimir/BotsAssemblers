using CodeBase.Interfaces;

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
            Unit.Deactivate();
        }
    }
}