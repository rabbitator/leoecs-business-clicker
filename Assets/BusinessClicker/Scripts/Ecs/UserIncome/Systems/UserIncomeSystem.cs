using System;
using BusinessClicker.Data;
using BusinessClicker.Ecs.BusinessBehaviour.Components;
using BusinessClicker.Ecs.Common.Components;
using Leopotam.EcsLite;
using UniRx;

namespace BusinessClicker.Ecs.UserIncome.Systems
{
    public class UserIncomeSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private EcsWorld _ecsWorld;
        private IDisposable _eventDisposable;

        public void Init(IEcsSystems systems)
        {
            _ecsWorld = systems.GetWorld();
            var gameData = systems.GetShared<GameData>();

            _eventDisposable = gameData.GameEvents.OnTransferBusinessIncomeToUser.AsObservable().Subscribe(GiveMoneyToUser);
        }

        private void GiveMoneyToUser(double value)
        {
            var userBalance = _ecsWorld.Filter<CurrentBalance>().Exc<Business>().End();
            var currentBalancePool = _ecsWorld.GetPool<CurrentBalance>();

            foreach (var entity in userBalance)
            {
                ref var currentBalance = ref currentBalancePool.Get(entity);
                currentBalance.Value += value;
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _eventDisposable.Dispose();
        }
    }
}