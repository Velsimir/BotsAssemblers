using System;

namespace CodeBase.Interfaces
{
    public interface ISpawnable
    {
        public event Action<ISpawnable> Dissapear;
    }
}