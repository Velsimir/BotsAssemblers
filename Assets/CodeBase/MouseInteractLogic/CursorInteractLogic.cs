using CodeBase.BaseSpawnerLogic;
using CodeBase.MainBase;
using UnityEngine;

public class CursorInteractLogic : MonoBehaviour
{
    [SerializeField] private BaseBuilder _baseBuilder;
    
    private Camera _camera;
    private Base _currentBase;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectBase();
        }

        if (Input.GetMouseButtonDown(1) && _currentBase != null)
        {
            _currentBase.SendUnitToBuild(GetMousePositionByRayCast(GetRayCastHitByClick()));
            _baseBuilder.SpawnFlagAtMousePosition();
        }
    }

    private void SelectBase()
    {
        RaycastHit hit = GetRayCastHitByClick();
            
        GetMousePositionByRayCast(hit);
            
        if (TrySelectBase(hit, out Base currentBase))
        {
            _currentBase = currentBase;
        }
    }

    private RaycastHit GetRayCastHitByClick()
    {
        Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        
        return hit;
    }

    private Vector3 GetMousePositionByRayCast(RaycastHit hit)
    {
        return hit.point;
    }

    private bool TrySelectBase(RaycastHit hit, out Base selectedBase)
    {
        if (hit.collider.gameObject.TryGetComponent(out Base @base))
        {
            selectedBase = @base;
            return true;
        }
        else
        {
            selectedBase = null;
            return false;
        }
    }
}
