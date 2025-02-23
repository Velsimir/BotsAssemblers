using System.Collections.Generic;
using System.Linq;
using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.Unit.StateMachine
{
    public class UnitStateMachine : IStateSwitcher
    {
        private List<IState> _states;
        private IState _currentState;

        public UnitStateMachine(Unit unit)
        {
            _states = new List<IState>()
            {
                new IdlingState(this, unit),
                new RunningState(this, unit),
                new MiningState(this, unit)
            };
            
            _currentState = _states[0];
            
            _currentState.Enter();
            
            Debug.Log($"Entered {_currentState.GetType().Name}");
        }

        public void Switch<State>() where State : IState
        {
            IState state = _states.FirstOrDefault(state => state is State);
            
            _currentState.Exit();
            _currentState = state;
            _currentState.Enter();
        }
    }
}