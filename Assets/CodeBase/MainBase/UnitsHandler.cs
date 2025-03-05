using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.ResourceLogic;
using CodeBase.UnitLogic;

namespace CodeBase.MainBase
{
    public class UnitsHandler 
    {
        private readonly List<Unit> _freeUnits;
        private readonly List<Unit> _allUnits;
        private readonly Queue<TaskCompletionSource<Unit>> _pendingRequests;

        public UnitsHandler()
        {
            _freeUnits = new List<Unit>();
            _allUnits = new List<Unit>();
            _pendingRequests = new Queue<TaskCompletionSource<Unit>>();
        }

        public bool HasFreeUnits => _freeUnits.Count > 0;

        public async Task SendUnitToMineAsync(Resource resource)
        {
            Unit unit = await GetFreeUnitAsync();
            unit.TakeResourceToMine(resource);
            _freeUnits.Remove(unit);
            unit.ReturnedOnBase += AddFreeUnit;
            unit.ReturnedOnBase += TrySendFreeUnitNewTask;
        }

        public void AddNewUnit(Unit unit)
        {
            _freeUnits.Add(unit);
            _allUnits.Add(unit);
        }

        private Task<Unit> GetFreeUnitAsync()
        {
            if (_freeUnits.Count > 0)
            {
                Unit unit = _freeUnits[0];
                _freeUnits.Remove(unit);
                return Task.FromResult(unit);
            }
            else
            {
                TaskCompletionSource<Unit> newTask = new TaskCompletionSource<Unit>();
                _pendingRequests.Enqueue(newTask);
                return newTask.Task;
            }
        }

        private void AddFreeUnit(Unit unit)
        {
            _freeUnits.Add(unit);
            unit.ReturnedOnBase -= AddFreeUnit;
        }

        private void TrySendFreeUnitNewTask(Unit unit)
        {
            if (_pendingRequests.Count > 0)
            {
                TaskCompletionSource<Unit> newTask = _pendingRequests.Dequeue();
                unit.ReturnedOnBase -= TrySendFreeUnitNewTask;
                newTask.SetResult(unit);
            }
        }
    }
}