using UnityEngine;

namespace CodeBase
{
    public class CameraLogic : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float edgeScrollSize = 20f;

        private void Update()
        {
            Vector3 moveDirection = Vector3.zero;

            moveDirection = TakeDirectionByArrows(moveDirection);

            moveDirection = TakeDirectionByMouse(moveDirection);

            moveDirection.Normalize();

            Vector3 globalMoveDirection = transform.TransformDirection(moveDirection);
            globalMoveDirection.y = 0;

            transform.Translate(globalMoveDirection * moveSpeed * Time.deltaTime, Space.World);
        }

        private Vector3 TakeDirectionByMouse(Vector3 moveDirection)
        {
            if (Input.mousePosition.x <= edgeScrollSize) moveDirection.x -= 1;
            if (Input.mousePosition.x >= Screen.width - edgeScrollSize) moveDirection.x += 1;
            if (Input.mousePosition.y <= edgeScrollSize) moveDirection.z -= 1;
            if (Input.mousePosition.y >= Screen.height - edgeScrollSize) moveDirection.z += 1;
            return moveDirection;
        }

        private static Vector3 TakeDirectionByArrows(Vector3 moveDirection)
        {
            if (Input.GetKey(KeyCode.W)) moveDirection.z += 1;
            if (Input.GetKey(KeyCode.S)) moveDirection.z -= 1;
            if (Input.GetKey(KeyCode.A)) moveDirection.x -= 1;
            if (Input.GetKey(KeyCode.D)) moveDirection.x += 1;
            return moveDirection;
        }
    }
}