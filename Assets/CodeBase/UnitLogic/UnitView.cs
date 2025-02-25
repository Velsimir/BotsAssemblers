using UnityEngine;

namespace CodeBase.UnitLogic
{
    public class UnitView
    {
        private const string IsIdling = "IsIdling";
        private const string IsRunning = "IsRunning";
        private const string IsMining = "IsMining";
        
        private static readonly int Idling = Animator.StringToHash(IsIdling);
        private static readonly int Running = Animator.StringToHash(IsRunning);
        private static readonly int Mining = Animator.StringToHash(IsMining);
        
        private Animator _animator;

        public UnitView(Animator animator)
        {
            _animator = animator;
        }

        public void StartIdling() => _animator.SetBool(Idling, true);
        public void StopIdling() => _animator.SetBool(Idling, false);
        
        public void StartRunning() => _animator.SetBool(Running, true);
        public void StopRunning() => _animator.SetBool(Running, false);
        
        public void StartMining() => _animator.SetBool(Mining, true);
        public void StopMining() => _animator.SetBool(Mining, false);
    }
}