using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.UnitLogic;
using UnityEngine;

namespace CodeBase.MainBase
{
    public class UnitsHandler 
    {
        private readonly List<Unit> _freeUnits;
        private readonly List<Unit> _allUnits;
        private readonly Queue<TaskCompletionSource<Unit>> _pendingRequests;

        private Unit _unitBuilder;
        private int _maxTask = 5;

        public UnitsHandler()
        {
            _freeUnits = new List<Unit>();
            _allUnits = new List<Unit>();
            _pendingRequests = new Queue<TaskCompletionSource<Unit>>();
        }
        
        public async Task SendUnitToBuildAsync(Vector3 position)
        {
            if (_unitBuilder == null)
            {
                _unitBuilder = await GetFreeUnitAsync();
                _freeUnits.Remove(_unitBuilder);
            }
            
            _unitBuilder.SendNewTask(position, UnitTask.Build);
        }
        
        public async Task SendTaskToMineAsync(Vector3 position)
        {
            if (_pendingRequests.Count > _maxTask)
                return;
            
            Unit unit = await GetFreeUnitAsync();
            unit.SendNewTask(position, UnitTask.Mine);
            
            _freeUnits.Remove(unit);
            
            unit.ReturnedOnBase += AddFreeUnit;
            unit.ReturnedOnBase += TrySendFreeUnitNewTask;
        }

        public void AddNewUnit(Unit unit)
        {
            _freeUnits.Add(unit);
            _allUnits.Add(unit);

            TrySendFreeUnitNewTask(unit);
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