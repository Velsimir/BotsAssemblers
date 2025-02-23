using System;
using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.Resource
{
    public class Resource : MonoBehaviour, ICollectable, IInteractable, ISpawnable, IRestartable
    {
        [SerializeField] private string _name;
        
        private bool _isCollected;
        private Collider _collider;

        public event Action<ISpawnable> Dissapear;

        public bool IsCollected => _isCollected;
        public Collider Collider => _collider;

        private void Awake()
        {
            _isCollected = false;
            _collider = GetComponent<Collider>();
        }

        public void Collect()
        {
            Debug.Log($"{_name} Collected");
            gameObject.SetActive(false);
            _isCollected = true;
            Dissapear?.Invoke(this);
        }

        public void Interact()
        {
            Debug.Log($"{_name} was Interacted");
        }

        public void Restart()
        {
            gameObject.SetActive(false);
            _isCollected = false;
            Dissapear?.Invoke(this);
        }
    }
}
