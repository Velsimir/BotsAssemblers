namespace CodeBase.Interfaces
{
    public interface IStateSwitcher
    {
        public void Switch<State>() where State : IState;
    }
}