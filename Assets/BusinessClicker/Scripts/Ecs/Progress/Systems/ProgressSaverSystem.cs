using System;
using BusinessClicker.Data;
using BusinessClicker.Ecs.BusinessBehaviour.Components;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Improvement.Components;
using Leopotam.EcsLite;
using UniRx;
using UnityEngine;

namespace BusinessClicker.Ecs.Progress.Systems
{
    public class ProgressSaverSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private IDisposable _subscribeDisposable;
        private IEcsSystems _systems;

        public void Init(IEcsSystems systems)
        {
            _systems = systems;
            var gameData = systems.GetShared<GameData>();

            _subscribeDisposable = gameData.GameEvents.OnApplicationQuit.Subscribe(ApplicationQuitHandler);
        }

        private void SaveUserBalanceProgress()
        {
            var ecsWorld = _systems.GetWorld();

            var userBalanceFilter = ecsWorld.Filter<CurrentBalance>().Exc<Business>().End();
            var balancePool = ecsWorld.GetPool<CurrentBalance>();

            foreach (var entity in userBalanceFilter)
            {
                ref var userBalance = ref balancePool.Get(entity);
                PlayerPrefs.SetFloat(PlayerPrefsNames.GetUserBalanceName(), (float) userBalance.Value);
            }
        }

        private void SaveBusinessProgress()
        {
            var ecsWorld = _systems.GetWorld();

            var businessFilter = ecsWorld.Filter<Business>().End();

            var balancePool = ecsWorld.GetPool<CurrentBalance>();
            var businessesPool = ecsWorld.GetPool<Business>();
            var improvementsPool = ecsWorld.GetPool<BusinessImprovements>();

            foreach (var entity in businessFilter)
            {
                ref var businessBalance = ref balancePool.Get(entity);
                ref var business = ref businessesPool.Get(entity);
                ref var improvements = ref improvementsPool.Get(entity);

                var businessLevelPrefName = PlayerPrefsNames.GetBusinessLevelName(business.Index);
                var businessBalancePrefName = PlayerPrefsNames.GetBusinessBalanceName(business.Index);

                PlayerPrefs.SetInt(businessLevelPrefName, business.CurrentLevel);
                PlayerPrefs.SetFloat(businessBalancePrefName, (float) businessBalance.Value);

                for (var i = 0; i < improvements.Values.Length; i++)
                {
                    var improvementPrefName = PlayerPrefsNames.GetImprovementName(business.Index, i);
                    PlayerPrefs.SetInt(improvementPrefName, improvements.Values[i] ? 1 : 0);
                }
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _subscribeDisposable.Dispose();
        }

        private void ApplicationQuitHandler(Unit _)
        {
            SaveUserBalanceProgress();
            SaveBusinessProgress();
        }
    }
}