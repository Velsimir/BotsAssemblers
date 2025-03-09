using System;
using CodeBase.MainBase;
using UnityEngine;

namespace CodeBase.MouseInteractLogic
{
    public class CursorInteractLogic : MonoBehaviour
    {
        private Camera _camera;

        public event Action<RaycastHit> LeftClick;
        public event Action<RaycastHit> RightClick;
    
        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                LeftClick?.Invoke(GetRayCastHitByClick());
                //SelectBase();
            }

            if (Input.GetMouseButtonDown(1))
            {
                RightClick?.Invoke(GetRayCastHitByClick());
                //_currentBase.SendUnitToBuild(GetMousePositionByRayCast(GetRayCastHitByClick()), _baseBuilder);
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
}
