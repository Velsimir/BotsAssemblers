using System;
using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.ResourceLogic
{
    public class Resource : MonoBehaviour, ICollectable, IInteractable, ISpawnable
    {
        [SerializeField] private string _name;
        
        public event Action<ISpawnable> Dissapear;

        public Collider Collider { get; private set; }

        private void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        public void Collect()
        {
            gameObject.SetActive(false);
            transform.SetParent(null);
            Dissapear?.Invoke(this);
        }

        public void Interact(Transform parent)
        {
            transform.SetParent(parent);
            transform.position = parent.position;
        }
    }
}
