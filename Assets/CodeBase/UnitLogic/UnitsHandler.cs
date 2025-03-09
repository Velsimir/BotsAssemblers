using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.UnitLogic
{
    public class UnitsHandler 
    {
        private readonly List<Unit> _freeUnits;
        private readonly Queue<TaskCompletionSource<Unit>> _pendingRequests;
        private TaskCompletionSource<Unit> _requestToBuild;
        private readonly int _maxTask = 5;

        private Unit _builderUnit;

        public UnitsHandler()
        {
            _freeUnits = new List<Unit>();
            _pendingRequests = new Queue<TaskCompletionSource<Unit>>();
        }
        
        public async Task SendUnitToBuildAsync(Vector3 position)
        {
            if (_builderUnit == null)
            {
                _builderUnit = await GetFreeUnitAsync(true);
                _freeUnits.Remove(_builderUnit);
            }
            
            _builderUnit.SendNewTask(position, UnitTask.Build);

            _requestToBuild = null;
        }
        
        public async Task SendTaskToMineAsync(Vector3 position)
        {
            if (_pendingRequests.Count > _maxTask)
                return;
            
            Unit unit = await GetFreeUnitAsync();
            unit.SendNewTask(position, UnitTask.Mine);
            
            _freeUnits.Remove(unit);
            
            unit.ReturnedOnBase += AddFreeUnit;
        }

        public void AddNewUnit(Unit unit)
        {
            _freeUnits.Add(unit);

            TrySendFreeUnitNewTask(unit);
        }

        private Task<Unit> GetFreeUnitAsync(bool priority = false)
        {
            if (_freeUnits.Count > 0)
            {
                Unit unit = _freeUnits[0];
                _freeUnits.Remove(unit);
                return Task.FromResult(unit);
            }
            else
            {
                if (priority)
                {
                    _requestToBuild = new TaskCompletionSource<Unit>();
                    return _requestToBuild.Task;
                }
                else
                {
                    TaskCompletionSource<Unit> newTask = new TaskCompletionSource<Unit>();
                    _pendingRequests.Enqueue(newTask);
                    return newTask.Task;
                }
            }
        }

        private void AddFreeUnit(Unit unit)
        {
            _freeUnits.Add(unit);
            unit.ReturnedOnBase -= AddFreeUnit;
            TrySendFreeUnitNewTask(unit);
        }

        private void TrySendFreeUnitNewTask(Unit unit)
        {
            if (_requestToBuild != null)
            {
                Debug.Log("Sending new task to build");
                _requestToBuild.SetResult(unit);
                return;
            }

            if (_pendingRequests.Count > 0)
            {
                Debug.Log("Sending new task to mine");
                TaskCompletionSource<Unit> newTask = _pendingRequests.Dequeue();
                newTask.SetResult(unit);
            }
        }
    }
}