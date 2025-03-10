using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Interfaces;
using UnityEngine;

namespace CodeBase.UnitLogic
{
    public class UnitsHandler 
    {
        private readonly List<Unit> _freeUnits;
        private readonly List<Unit> _allUnits;
        private readonly Queue<TaskCompletionSource<Unit>> _pendingRequests;
        private TaskCompletionSource<Unit> _requestToBuild;
        private readonly int _maxTask = 5;

        public UnitsHandler()
        {
            _freeUnits = new List<Unit>();
            _allUnits = new List<Unit>();
            _pendingRequests = new Queue<TaskCompletionSource<Unit>>();
        }

        public Unit BuilderUnit { get; private set; }
        public bool HasFreeTasks => _pendingRequests.Count < _maxTask;

        public List<Unit> AllUnits => _allUnits;
        
        public async Task SendUnitToBuildAsync(Vector3 position)
        {
            if (_requestToBuild == null && BuilderUnit == null)
            {
                BuilderUnit = await GetFreeUnitAsync(true);
                _freeUnits.Remove(BuilderUnit);
                _allUnits.Remove(BuilderUnit);
            }
            
            BuilderUnit.SendNewTask(position, UnitTask.Build);

            BuilderUnit.Dissapear += ForgetUnit;
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
            _allUnits.Add(unit);

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
                _requestToBuild.SetResult(unit);
                return;
            }

            if (_pendingRequests.Count > 0)
            {
                TaskCompletionSource<Unit> newTask = _pendingRequests.Dequeue();
                newTask.SetResult(unit);
            }
        }

        private void ForgetUnit(ISpawnable unit)
        {
            BuilderUnit.Dissapear -= ForgetUnit;
            BuilderUnit = null;
        }
    }
}