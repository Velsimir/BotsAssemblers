using CodeBase.UnitLogic;
using UnityEngine;

namespace CodeBase.MainBase
{
    [CreateAssetMenu(menuName = "MainBase/BaseData", fileName = "BaseData")]
    public class BaseData : ScriptableObject
    {
        [SerializeField] private Unit _unitPrefab;
        [SerializeField] private int _resourcesToSpawnNewUnit;
        [SerializeField] private float _radiusToSearchResources;
        [SerializeField] private float _scanDelay;
        
        public int ResourcesToSpawnNewUnit => _resourcesToSpawnNewUnit;
        public Unit UnitPrefab => _unitPrefab;
        public float RadiusToSearchResources => _radiusToSearchResources;
        public float ScanDelay => _scanDelay;
    }
}