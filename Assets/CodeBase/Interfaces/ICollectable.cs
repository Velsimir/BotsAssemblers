
namespace CodeBase.Interfaces
{
    public interface ICollectable
    {
        bool IsReserved { get; }
        void Collect();
    }
}