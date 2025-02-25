using System;
using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.ResourceLogic
{
    public class Resource : MonoBehaviour, ICollectable, IInteractable, ISpawnable, IRestartable
    {
        [SerializeField] private string _name;
        
        private Collider _collider;

        public event Action<ISpawnable> Dissapear;

        public Collider Collider => _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
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

        public void Restart()
        {
            gameObject.SetActive(false);
            transform.SetParent(null);
            Dissapear?.Invoke(this);
        }
    }
}
