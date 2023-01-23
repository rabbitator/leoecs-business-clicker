﻿using BusinessClicker.Data;
using BusinessClicker.Ecs.BusinessBehaviour.Components;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Improvement.Components;
using BusinessClicker.Utilities;
using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessClicker.Ecs.BusinessBehaviour.Systems
{
    public class BusinessIncomeSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var gameData = systems.GetShared<GameData>();
            var ecsWorld = systems.GetWorld();

            var businesses = ecsWorld.Filter<Business>().End();

            var currentBalancePool = ecsWorld.GetPool<CurrentBalance>();
            var improvementsPool = ecsWorld.GetPool<BusinessImprovements>();
            var businessPool = ecsWorld.GetPool<Business>();

            foreach (var entity in businesses)
            {
                var business = businessPool.Get(entity);
                var improvement = improvementsPool.Get(entity);
                var data = gameData.BusinessesData[business.Index];

                if (business.CurrentLevel <= 0) continue;

                var totalIncome = FinancialCalculator.GetBusinessIncomeByComponents(systems, business, improvement);
                var deltaIncome = totalIncome / data.IncomeDelay * Time.deltaTime;

                ref var currentBalance = ref currentBalancePool.Get(entity);
                currentBalance.Value += deltaIncome;

                if (currentBalance.Value < totalIncome) continue;

                gameData.GameEvents.OnTransferBusinessIncomeToUser.OnNext(totalIncome);
                currentBalance.Value = 0.0f;
            }
        }
    }
}