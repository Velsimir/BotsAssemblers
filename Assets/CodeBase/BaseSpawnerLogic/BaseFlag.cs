using UnityEngine;

namespace CodeBase.BaseSpawnerLogic
{
    public class BaseFlag : MonoBehaviour
    {
        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            IsInitialized = true;
        }

        public void Move(Vector3 position)
        {
            transform.position = new Vector3(position.x, transform.position.y, position.z);
        }
    }
}