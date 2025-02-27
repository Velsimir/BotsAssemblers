using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Interfaces;
using CodeBase.ResourceLogic;

namespace CodeBase.MainBase
{
    public class ResourceHandler : IDisposable
    {
        private readonly Scanner<Resource> _scanner;
        private readonly List<Resource> _reservedResources;
        private readonly List<Resource> _foundedNewResources;
        
        public ResourceHandler(Scanner<Resource> scanner)
        {
            _reservedResources = new List<Resource>();
            _foundedNewResources = new List<Resource>();
            
            _scanner = scanner;
            _scanner.ScanFinished += AddNewResources;
        }
        
        public event Action NewResourcesAdded; 

        public bool TryGetResource(out Resource resource)
        {
            if (_foundedNewResources.Count > 0)
            {
                resource = _foundedNewResources[0];
                    
                _reservedResources.Add(resource);
                _foundedNewResources.Remove(resource);
                    
                resource.Dissapear += RemoveFromReserved;
                
                return true;
            }
            else
            {
                resource = null;
                return false;
            }
        }

        private void RemoveFromReserved(ISpawnable resource)
        {
            _reservedResources.Remove(resource as Resource);
            resource.Dissapear -= RemoveFromReserved;
        }

        private void AddNewResources(List<Resource> foundedResources)
        {
            List<Resource> newResources = foundedResources.Except(_reservedResources).Except(_foundedNewResources).ToList();
            
            _foundedNewResources.AddRange(newResources);
            
            NewResourcesAdded?.Invoke();
        }

        public void Dispose()
        {
            _scanner.ScanFinished -= AddNewResources;
        }
    }
}