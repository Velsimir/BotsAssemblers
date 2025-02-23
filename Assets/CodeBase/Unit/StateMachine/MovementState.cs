using CodeBase.Interfaces;

namespace CodeBase.Unit.StateMachine
{
    public class MovementState : IState
    {
        protected IStateSwitcher StateSwitcher;
        protected Unit Unit;

        protected MovementState(IStateSwitcher stateSwitcher, Unit unit)
        {
            StateSwitcher = stateSwitcher;
            Unit = unit;
        }

        public virtual void Enter()
        { }

        public virtual void Exit()
        { }
    }
}