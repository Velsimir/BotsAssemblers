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
            _camera = FindAnyObjectByType<Camera>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                LeftClick?.Invoke(GetRayCastHitByClick());
            }

            if (Input.GetMouseButtonDown(1))
            {
                RightClick?.Invoke(GetRayCastHitByClick());
            }
        }

        private RaycastHit GetRayCastHitByClick()
        {
            Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
        
            return hit;
        }
    }
}
