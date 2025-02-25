using System.Collections.Generic;
using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.Services
{
    public class Spawner<TSpawnableObjet> where TSpawnableObjet : MonoBehaviour, ISpawnable
    {
        private readonly TSpawnableObjet _spawnablePrefab;
        private readonly ObjectPool _pool;

        public Spawner(TSpawnableObjet spawnablePrefab) 
        {
            _spawnablePrefab = spawnablePrefab;
            _pool = new ObjectPool();
        }

        public TSpawnableObjet Spawn()
        {
            TSpawnableObjet spawnableObjet;

            if (_pool.HasFree)
            {
                spawnableObjet = _pool.Get();
            }
            else
            {
                spawnableObjet = Object.Instantiate(_spawnablePrefab);
                _pool.Track(spawnableObjet);
            }
            
            spawnableObjet.gameObject.SetActive(true);
            
            return spawnableObjet;
        }

        private class ObjectPool
        {
            private readonly List<TSpawnableObjet> _pool;

            public ObjectPool()
            {
                _pool = new List<TSpawnableObjet>();
            }

            public bool HasFree => _pool.Count > 0;

            public TSpawnableObjet Get()
            {
                TSpawnableObjet spawnableObjet = _pool[0];
                _pool.Remove(spawnableObjet);
                spawnableObjet.Dissapear += Add;
                
                return spawnableObjet;
            }

            public void Track(TSpawnableObjet newObject)
            {
                newObject.Dissapear += Add;
            }

            private void Add(ISpawnable takenObject)
            {
                if (takenObject is TSpawnableObjet spawnableObjet)
                {
                    _pool.Add(spawnableObjet);
                    spawnableObjet.Dissapear -= Add;
                }
            }
        }
    }
}