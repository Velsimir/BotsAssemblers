namespace CodeBase.Interfaces
{
    public interface IState
    {
        void Enter();
        void Exit();
        void GetNextState();
    }
}