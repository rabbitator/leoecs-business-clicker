using BusinessClicker.Data;
using BusinessClicker.Ecs.BusinessBehaviour.Components;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Improvement.Components;
using BusinessClicker.Utilities;
using Leopotam.EcsLite;
using UnityEngine;
using UniRx;

namespace BusinessClicker.Ecs.BusinessBehaviour.Systems
{
    public class BusinessIncomeSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private IEcsSystems _systems;

        public void Init(IEcsSystems systems)
        {
            _systems = systems;
            var gameData = systems.GetShared<GameData>();

            gameData.GameEvents.OnBusinessImprovementPurchased.Subscribe(ImprovePurchased).AddTo(_disposables);
        }

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
                var data = gameData.BusinessesData[business.Index];

                if (business.CurrentLevel <= 0) continue;

                var percentValues = FinancialCalculator.GetMaskedImprovements(improvementsPool.Get(entity).Value, data.BusinessImprovements);
                var totalIncome = FinancialCalculator.GetBusinessIncome(business.CurrentLevel, data.BaseIncome, percentValues);
                var deltaIncome = totalIncome / data.IncomeDelay * Time.fixedDeltaTime;

                ref var currentBalance = ref currentBalancePool.Get(entity);
                currentBalance.Value += deltaIncome;

                if (currentBalance.Value < totalIncome) continue;

                gameData.GameEvents.OnTransferBusinessIncomeToUser.OnNext(totalIncome);
                currentBalance.Value = 0.0f;
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _disposables.Dispose();
        }

        private void ImprovePurchased((int businessIndex, int improvementIndex) ids)
        {
            var (businessIndex, _) = ids;

            var ecsWorld = _systems.GetWorld();
            var gameData = _systems.GetShared<GameData>();

            var businesses = ecsWorld.Filter<Business>().End();

            var businessesPool = ecsWorld.GetPool<Business>();
            var improvementsPool = ecsWorld.GetPool<BusinessImprovements>();

            foreach (var entity in businesses)
            {
                ref var business = ref businessesPool.Get(entity);

                if (business.Index != businessIndex) continue;

                var businessData = gameData.BusinessesData[business.Index];
                var percentValues = FinancialCalculator.GetMaskedImprovements(improvementsPool.Get(entity).Value, businessData.BusinessImprovements);
                business.CurrentIncome = FinancialCalculator.GetBusinessIncome(business.CurrentLevel, businessData.BaseIncome, percentValues);
            }
        }
    }
}