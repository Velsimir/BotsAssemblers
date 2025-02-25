using UnityEngine;

namespace CodeBase.ResourceLogic
{
    [RequireComponent(typeof(MeshCollider))]
    public class ResourceNode : MonoBehaviour
    {
        public MeshCollider MeshCollider { get; private set; }

        public void Initialize()
        {
            MeshCollider = GetComponent<MeshCollider>();
        }
    }
}