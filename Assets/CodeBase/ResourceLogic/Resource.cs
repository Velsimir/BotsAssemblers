using System;
using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.ResourceLogic
{
    public class Resource : MonoBehaviour, ICollectable, IInteractable, ISpawnable, IRestartable
    {
        [SerializeField] private string _name;
        
        private bool _isReserved;
        private Collider _collider;

        public event Action<ISpawnable> Dissapear;

        public bool IsReserved => _isReserved;
        public Collider Collider => _collider;

        private void Awake()
        {
            _isReserved = false;
            _collider = GetComponent<Collider>();
        }

        public void Reserv()
        {
            _isReserved = true;
        }

        public void Collect()
        {
            gameObject.SetActive(false);
            transform.SetParent(null);
            Dissapear?.Invoke(this);
            _isReserved = false;
        }

        public void Interact(Transform parent)
        {
            transform.SetParent(parent);
            transform.position = parent.position;
        }

        public void Restart()
        {
            gameObject.SetActive(false);
            _isReserved = false;
            Dissapear?.Invoke(this);
        }
    }
}
