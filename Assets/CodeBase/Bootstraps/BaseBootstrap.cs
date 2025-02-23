using CodeBase.MainBase;
using UnityEngine;

namespace CodeBase.Bootstraps
{
    public class BaseBootstrap : MonoBehaviour
    {
        [SerializeField] private Base _base;
        [SerializeField] private BaseData _data;
        [SerializeField] private GameRestarter _gameRestarter;

        private void Awake()
        {
            _base.Initialize(_data);
            _gameRestarter.GameRestarted += Restart;
        }

        private void Restart()
        {
            _base.Restart();
        }
    }
}