﻿using BusinessClicker.Data;
using BusinessClicker.Ecs.BusinessBehaviour.Components;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Improvement.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessClicker.Ecs.ProgressLoader.Systems
{
    public class ProgressLoaderSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            LoadUserBalanceProgress(systems);
            LoadBusinessProgress(systems);
        }

        private static void LoadUserBalanceProgress(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();

            var userBalanceFilter = ecsWorld.Filter<CurrentBalance>().Exc<Business>().End();
            var balancePool = ecsWorld.GetPool<CurrentBalance>();

            foreach (var entity in userBalanceFilter)
            {
                ref var userBalance = ref balancePool.Get(entity);
                userBalance.Value = PlayerPrefs.GetFloat(PlayerPrefsNames.UserBalance, 0.0f);
            }
        }

        private void LoadBusinessProgress(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();

            var businessFilter = ecsWorld.Filter<Business>().End();

            var balancePool = ecsWorld.GetPool<CurrentBalance>();
            var businessesPool = ecsWorld.GetPool<Business>();
            var improvementsPool = ecsWorld.GetPool<BusinessImprovements>();

            foreach (var entity in businessFilter)
            {
                ref var businessBalance = ref balancePool.Get(entity);
                ref var business = ref businessesPool.Get(entity);
                ref var improvements = ref improvementsPool.Get(entity);

                // TODO: Real loading from player prefs
                business.CurrentLevel = business.Index == 0 ? 1 : 0;
                businessBalance.Value = 0.0f;

                for (var i = 0; i < improvements.Values.Length; i++)
                {
                    improvements.Values[i] = false;
                }
            }
        }
    }
}