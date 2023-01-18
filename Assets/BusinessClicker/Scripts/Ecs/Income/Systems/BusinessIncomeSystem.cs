using BusinessClicker.Data;
using BusinessClicker.Data.Views;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Income.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessClicker.Ecs.Income.Systems
{
    public class BusinessIncomeSystem : IEcsInitSystem, IEcsRunSystem
    {
        public void Init(IEcsSystems systems)
        {
        }

        public void Run(IEcsSystems systems)
        {
            var gameData = systems.GetShared<GameData>();
            var ecsWorld = systems.GetWorld();

            var businesses = ecsWorld.Filter<Business>().End();

            var currentBalancePool = ecsWorld.GetPool<CurrentBalance>();
            var businessPool = ecsWorld.GetPool<Business>();

            foreach (var entity in businesses)
            {
                ref var currentBalance = ref currentBalancePool.Get(entity);
                ref var business = ref businessPool.Get(entity);
                var data = gameData.BusinessesData[business.Id];

                if (business.CurrentLevel <= 0) continue;

                var totalIncome = business.CurrentLevel * data.BaseIncome * (1 /* +1*impr1+1*impr2 // TODO: Improvements */);
                var deltaIncome = totalIncome / data.IncomeDelay * Time.deltaTime;
                currentBalance.Value += deltaIncome;

                if (currentBalance.Value < totalIncome) continue;

                gameData.GameEventsService.OnSomeEvent.OnNext(currentBalance.Value);
                currentBalance.Value = 0.0f;
            }
        }
    }
}