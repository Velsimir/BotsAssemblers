using UnityEngine;

namespace CodeBase.Resource
{
    [RequireComponent(typeof(MeshCollider))]
    public class ResourceNode : MonoBehaviour
    {
        public MeshCollider MeshCollider { get; private set; }

        public void Initialize() => MeshCollider = GetComponent<MeshCollider>();
    }
}