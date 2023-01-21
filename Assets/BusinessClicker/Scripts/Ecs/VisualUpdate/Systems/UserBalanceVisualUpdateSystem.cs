using BusinessClicker.Data;
using BusinessClicker.Data.Views;
using BusinessClicker.Ecs.BusinessBehaviour.Components;
using BusinessClicker.Ecs.Common.Components;
using Leopotam.EcsLite;
using UniRx;

namespace BusinessClicker.Ecs.VisualUpdate.Systems
{
    public class UserBalanceVisualUpdateSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private IEcsSystems _systems;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void Init(IEcsSystems systems)
        {
            _systems = systems;
            var ecsWorld = systems.GetWorld();
            var gameData = systems.GetShared<GameData>();

            var userBalance = ecsWorld.Filter<CurrentBalance>().Exc<Business>().End();
            var businesses = ecsWorld.Filter<Business>().End();

            var businessesPool = ecsWorld.GetPool<Business>();
            var currentBalancePool = ecsWorld.GetPool<CurrentBalance>();
            var objectReferencesPool = ecsWorld.GetPool<UnityObjectReference>();

            foreach (var entity in userBalance)
            {
                ref var balance = ref currentBalancePool.Get(entity);
                var mainWindowView = (MainWindowView) objectReferencesPool.Get(entity).UnityObject;
                mainWindowView.SetBalance((int) balance.Value);
            }

            gameData.GameEvents.OnTransferBusinessIncomeToUser.AsObservable().Subscribe(UpdateUserBalance).AddTo(_disposables);
            gameData.GameEvents.OnBusinessImprovementPurchased.AsObservable().Subscribe(UpdateAfterImprovement).AddTo(_disposables);

            foreach (var entity in businesses)
            {
                ref var business = ref businessesPool.Get(entity);
                var businessIndex = business.Index;

                gameData.GameEvents.OnBusinessLevelPurchased.Subscribe(_ => UpdateAfterLevelUp(businessIndex)).AddTo(_disposables);
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _disposables.Dispose();
        }

        private void UpdateUserBalance(double _ = 0.0)
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

        private void UpdateAfterLevelUp(int _)
        {
            var ecsWorld = _systems.GetWorld();

            var userBalance = ecsWorld.Filter<CurrentBalance>().Exc<Business>().End();

            var currentBalancePool = ecsWorld.GetPool<CurrentBalance>();
            var objectReferencePool = ecsWorld.GetPool<UnityObjectReference>();

            foreach (var entity in userBalance)
            {
                ref var balance = ref currentBalancePool.Get(entity);
                var mainWindowView = (MainWindowView) objectReferencePool.Get(entity).UnityObject;
                mainWindowView.SetBalance((int) balance.Value);
            }
        }

        private void UpdateAfterImprovement((int businessIndex, int improvementIndex) _)
        {
            UpdateUserBalance();
        }
    }
}