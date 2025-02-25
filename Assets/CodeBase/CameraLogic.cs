using UnityEngine;

namespace CodeBase
{
    public class CameraLogic : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _edgeScrollSize = 20f;

        private void Update()
        {
            Vector3 moveDirection = Vector3.zero;

            moveDirection = TakeDirectionByMouse(moveDirection);

            moveDirection.Normalize();
            
            Vector3 globalMoveDirection = transform.TransformDirection(moveDirection);
            globalMoveDirection.y = 0;

            transform.Translate(globalMoveDirection * _moveSpeed * Time.deltaTime, Space.World);
        }

        private Vector3 TakeDirectionByMouse(Vector3 moveDirection)
        {
            if (Input.mousePosition.x <= _edgeScrollSize) moveDirection.x -= 1;
            if (Input.mousePosition.x >= Screen.width - _edgeScrollSize) moveDirection.x += 1;
            if (Input.mousePosition.y <= _edgeScrollSize) moveDirection.z -= 1;
            if (Input.mousePosition.y >= Screen.height - _edgeScrollSize) moveDirection.z += 1;
            
            return moveDirection;
        }
    }
}