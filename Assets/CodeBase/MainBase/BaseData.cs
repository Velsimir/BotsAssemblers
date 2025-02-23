using UnityEngine;

namespace CodeBase.MainBase
{
    [CreateAssetMenu(menuName = "MainBase/BaseData", fileName = "BaseData")]
    public class BaseData : ScriptableObject
    {
        [SerializeField] private Unit.Unit _unitPrefab;
        [SerializeField] private int _countOfInitialUnits;
        [SerializeField] private float _radiusToSearchResources;
        [SerializeField] private float _scanDelay;
        
        public int CountOfInitialUnits => _countOfInitialUnits;
        public Unit.Unit UnitPrefab => _unitPrefab;
        public float RadiusToSearchResources => _radiusToSearchResources;
        public float ScanDelay => _scanDelay;
    }
}