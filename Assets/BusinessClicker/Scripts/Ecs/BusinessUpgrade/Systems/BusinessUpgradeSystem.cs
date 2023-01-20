using BusinessClicker.Data;
using BusinessClicker.Data.Views;
using BusinessClicker.Ecs.BusinessBehaviour.Components;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Utilities;
using Leopotam.EcsLite;
using UniRx;

namespace BusinessClicker.Ecs.BusinessUpgrade.Systems
{
    public class BusinessUpgradeSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private IEcsSystems _systems;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void Init(IEcsSystems systems)
        {
            _systems = systems;
            var ecsWorld = systems.GetWorld();

            var businesses = ecsWorld.Filter<Business>().End();

            var objectReferencesPool = ecsWorld.GetPool<UnityObjectReference>();

            foreach (var entity in businesses)
            {
                var cardView = (BusinessCardView) objectReferencesPool.Get(entity).UnityObject;
                _disposables.Add(cardView.LvlUpButton.Button.OnClickAsObservable().Subscribe(_ => ButtonClickHandler(entity)));
            }
        }

        private void ButtonClickHandler(int businessEntity)
        {
            var gameData = _systems.GetShared<GameData>();
            var ecsWorld = _systems.GetWorld();

            var currentBalancePool = ecsWorld.GetPool<CurrentBalance>();

            var userBalance = ecsWorld.Filter<CurrentBalance>().Exc<Business>().End();

            ref var business = ref ecsWorld.GetPool<Business>().Get(businessEntity);
            var businessData = gameData.BusinessesData[business.Index];
            var nextLevelPrice = FinancialCalculator.GetPriceForBusinessLevel(business.CurrentLevel + 1, businessData.BasePrice);

            foreach (var balanceEntity in userBalance)
            {
                ref var balance = ref currentBalancePool.Get(balanceEntity);

                if (balance.Value < nextLevelPrice) continue;

                balance.Value -= nextLevelPrice;
                business.CurrentLevel++;

                gameData.GameEvents.OnBusinessLevelPurchased.OnNext(business.Index);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _disposables.Dispose();
        }
    }
}