using BusinessClicker.Data;
using BusinessClicker.Data.Views;
using BusinessClicker.Ecs.BusinessBehaviour.Components;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Improvement.Components;
using Leopotam.EcsLite;
using UniRx;

namespace BusinessClicker.Ecs.Improvement.Systems
{
    public class ImproveBusinessSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private IEcsSystems _systems;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void Init(IEcsSystems systems)
        {
            _systems = systems;
            var ecsWorld = _systems.GetWorld();
            var gameData = _systems.GetShared<GameData>();

            var businessesData = gameData.BusinessesData;

            var businesses = ecsWorld.Filter<Business>().End();

            var businessesPool = ecsWorld.GetPool<Business>();
            var objectReferences = ecsWorld.GetPool<UnityObjectReference>();

            foreach (var entity in businesses)
            {
                var cardView = (BusinessCardView) objectReferences.Get(entity).UnityObject;
                var businessIndex = businessesPool.Get(entity).Index;

                for (var i = 0; i < businessesData[businessIndex].BusinessImprovements.Length; i++)
                {
                    var improveView = cardView.ImproveButtonsRoot.GetChild(i).GetComponent<ImprovementButtonView>();
                    var improvementIndex = i;
                    improveView.Button.OnClickAsObservable().Subscribe(_ => ImproveRequestHandler(businessIndex, improvementIndex)).AddTo(_disposables);
                }
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _disposables.Dispose();
        }

        private void ImproveRequestHandler(int businessIndex, int improvementIndex)
        {
            var ecsWorld = _systems.GetWorld();
            var gameData = _systems.GetShared<GameData>();

            var userBalances = ecsWorld.Filter<CurrentBalance>().Exc<Business>().End();
            var businesses = ecsWorld.Filter<Business>().End();

            var currentBalancePool = ecsWorld.GetPool<CurrentBalance>();
            var businessesPool = ecsWorld.GetPool<Business>();
            var improvementsPool = ecsWorld.GetPool<BusinessImprovements>();

            foreach (var businessEntity in businesses)
            {
                ref var business = ref businessesPool.Get(businessEntity);
                ref var improvement = ref improvementsPool.Get(businessEntity);

                if (business.Index != businessIndex) continue;
                if (improvement.Values[improvementIndex]) continue;

                var improvementPrice = gameData.BusinessesData[businessIndex].BusinessImprovements[improvementIndex].Price;

                foreach (var userBalanceEntity in userBalances)
                {
                    ref var userBalance = ref currentBalancePool.Get(userBalanceEntity);

                    if (userBalance.Value < improvementPrice) continue;

                    userBalance.Value -= improvementPrice;
                    improvement.Values[improvementIndex] = true;

                    gameData.GameEvents.OnBusinessImprovementPurchased.OnNext((businessIndex, improvementIndex));
                }
            }
        }
    }
}