using System;
using System.Collections.Generic;
using CodeBase.Interfaces;
using CodeBase.ResourceLogic;

namespace CodeBase.MainBase
{
    public class ResourceHandler : IDisposable
    {
        private readonly Scanner _scanner;
        private readonly List<Resource> _reservedResources;
        private readonly ResourceSpawner _resourceSpawner;

        public ResourceHandler(ResourceSpawner resourceSpawner)
        {
            _reservedResources = new List<Resource>();
            _resourceSpawner = resourceSpawner;
            _resourceSpawner.ResourceSpawned += RemoveFromReserved;
        }

        public bool TryGetFreeResource(ref Resource resource)
        {
            if (_reservedResources.Contains(resource))
            {
                return false;
            }
            
            _reservedResources.Add(resource);
                    
            return true;
        }

        private void RemoveFromReserved(ISpawnable resource)
        {
            _reservedResources.Remove(resource as Resource);
        }

        public void Dispose()
        {
            _resourceSpawner.ResourceSpawned -= RemoveFromReserved;
        }
    }
}