namespace CodeBase.Interfaces
{
    public interface ICollectable
    {
        bool IsCollected { get; }
        
        void Collect();
    }
}