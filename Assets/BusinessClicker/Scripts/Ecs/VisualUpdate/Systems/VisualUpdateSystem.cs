using System;
using BusinessClicker.Data;
using BusinessClicker.Data.Views;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Income.Components;
using Leopotam.EcsLite;
using UniRx;
using UnityEngine;

namespace BusinessClicker.Ecs.VisualUpdate.Systems
{
    public class VisualUpdateSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private IEcsSystems _systems;
        private IDisposable _eventDisposable;

        public void Init(IEcsSystems systems)
        {
            _systems = systems;
            var ecsWorld = systems.GetWorld();
            var gameData = systems.GetShared<GameData>();

            var userBalance = ecsWorld.Filter<CurrentBalance>().Exc<Business>().End();
            var businesses = ecsWorld.Filter<Business>().End();

            var businessDataPool = ecsWorld.GetPool<Business>();
            var currentBalancePool = ecsWorld.GetPool<CurrentBalance>();
            var objectReferencesPool = ecsWorld.GetPool<UnityObjectReference>();

            foreach (var entity in userBalance)
            {
                ref var balance = ref currentBalancePool.Get(entity);
                var mainWindowView = (MainWindowView) objectReferencesPool.Get(entity).UnityObject;
                mainWindowView.SetBalance((int) balance.Value);

                _eventDisposable = gameData.GameEventsService.OnSomeEvent.AsObservable().Subscribe(UpdateView);
            }

            foreach (var entity in businesses)
            {
                ref var businessData = ref businessDataPool.Get(entity);
                var view = (BusinessCardView) objectReferencesPool.Get(entity).UnityObject;
                var businessStartupData = gameData.BusinessesData[businessData.Id];

                view.SetName(businessStartupData.Name);
                view.SetLevel(businessData.CurrentLevel);
                view.SetIncome((int) businessData.CurrentIncome);
            }
        }

        public void Run(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();

            var businesses = ecsWorld.Filter<Business>().End();

            var businessDataPool = ecsWorld.GetPool<Business>();
            var currentBalancePool = ecsWorld.GetPool<CurrentBalance>();
            var objectReferencesPool = ecsWorld.GetPool<UnityObjectReference>();

            foreach (var entity in businesses)
            {
                var view = (BusinessCardView) objectReferencesPool.Get(entity).UnityObject;
                ref var currentBalance = ref currentBalancePool.Get(entity);
                ref var businessData = ref businessDataPool.Get(entity);
                view.ProgressBar.value = Mathf.Clamp01(currentBalance.Value / businessData.CurrentIncome);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _eventDisposable.Dispose();
        }

        private void UpdateView(float value)
        {
            var ecsWorld = _systems.GetWorld();

            var userBalance = ecsWorld.Filter<CurrentBalance>().Exc<Business>().End();

            var currentBalancePool = ecsWorld.GetPool<CurrentBalance>();
            var objectReferencesPool = ecsWorld.GetPool<UnityObjectReference>();

            foreach (var entity in userBalance)
            {
                ref var balance = ref currentBalancePool.Get(entity);
                var mainWindowView = (MainWindowView) objectReferencesPool.Get(entity).UnityObject;
                mainWindowView.SetBalance((int) balance.Value);
            }
        }
    }
}