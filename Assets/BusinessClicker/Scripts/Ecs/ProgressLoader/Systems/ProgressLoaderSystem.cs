using BusinessClicker.Data;
using BusinessClicker.Ecs.Income.Components;
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
            var gameData = systems.GetShared<GameData>();

            var businessFilter = ecsWorld.Filter<Business>().End();
            var balancePool = ecsWorld.GetPool<CurrentBalance>();
            var dataPool = ecsWorld.GetPool<Business>();

            foreach (var entity in businessFilter)
            {
                ref var businessBalance = ref balancePool.Get(entity);
                ref var businessData = ref dataPool.Get(entity);

                // TODO: Real loading from player prefs
                businessData.CurrentLevel = businessData.Id == 0 ? 1 : 0;
                businessData.CurrentIncome = gameData.BusinessesData[businessData.Id].BaseIncome;
                businessBalance.Value = 0.0f;
            }
        }
    }
}