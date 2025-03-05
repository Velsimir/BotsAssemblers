using System;

namespace CodeBase.MainBase
{
    public class ResourceCollector
    {
        public int CollectedResources { get; private set; }

        public event Action ValueChanged;

        public void IncreaseResources()
        {
            CollectedResources ++;
            ValueChanged?.Invoke();
        }

        public void Spend(int amount)
        {
            if (CollectedResources >= amount )
            {
                CollectedResources -= amount;
                ValueChanged?.Invoke();
            }
        }
    }
}