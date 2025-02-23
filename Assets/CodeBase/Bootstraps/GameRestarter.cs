using System;
using UnityEngine;

namespace CodeBase.Bootstraps
{
    public class GameRestarter : MonoBehaviour
    {
        public event Action GameRestarted;
        
        [ContextMenu("Restart Game")]
        private void Restart() => GameRestarted?.Invoke();
    }
}