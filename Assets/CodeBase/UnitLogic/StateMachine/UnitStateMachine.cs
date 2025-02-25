using System.Collections.Generic;
using System.Linq;
using CodeBase.Interfaces;

namespace CodeBase.UnitLogic.StateMachine
{
    public class UnitStateMachine : IStateSwitcher
    {
        private List<IState> _states;
        private IState _currentState;

        public UnitStateMachine(Unit unit)
        {
            _states = new List<IState>()
            {
                new IdleState(this, unit),
                new RunningState(this, unit),
                new MineState(this, unit)
            };
            
            _currentState = _states[0];
            
            _currentState.Enter();
        }
        
        public IState CurrentState => _currentState;

        public void Switch<TState>() where TState : IState
        {
            IState state = _states.FirstOrDefault(state => state is TState);
            
            _currentState.Exit();
            _currentState = state;
            _currentState.Enter();
        }
    }
}