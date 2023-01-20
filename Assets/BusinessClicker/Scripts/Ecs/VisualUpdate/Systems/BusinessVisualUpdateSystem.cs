using BusinessClicker.Data;
using BusinessClicker.Data.Views;
using BusinessClicker.Ecs.BusinessBehaviour.Components;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Improvement.Components;
using BusinessClicker.Utilities;
using Leopotam.EcsLite;
using UniRx;
using UnityEngine;

namespace BusinessClicker.Ecs.VisualUpdate.Systems
{
    public class BusinessVisualUpdateSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private IEcsSystems _systems;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void Init(IEcsSystems systems)
        {
            _systems = systems;
            var ecsWorld = systems.GetWorld();
            var gameData = systems.GetShared<GameData>();

            var businesses = ecsWorld.Filter<Business>().End();

            var businessesPool = ecsWorld.GetPool<Business>();
            var improvementsPool = ecsWorld.GetPool<BusinessImprovements>();
            var objectReferencesPool = ecsWorld.GetPool<UnityObjectReference>();

            foreach (var entity in businesses)
            {
                ref var business = ref businessesPool.Get(entity);
                var improvements = improvementsPool.Get(entity);
                var view = (BusinessCardView) objectReferencesPool.Get(entity).UnityObject;
                var businessData = gameData.BusinessesData[business.Index];

                view.SetName(businessData.Name);
                view.SetLevel(business.CurrentLevel);
                view.SetIncome((int) FinancialCalculator.GetBusinessIncomeByComponents(_systems, business, improvements));
                view.LvlUpButton.SetPrice((int) FinancialCalculator.GetPriceForBusinessLevel(business.CurrentLevel + 1, businessData.BasePrice));

                var businessIndex = business.Index;
                gameData.GameEvents.OnBusinessLevelPurchased.Subscribe(_ => UpdateAfterLevelUp(businessIndex)).AddTo(_disposables);

                var improveButtons = view.ImproveButtonsRoot.GetComponentsInChildren<ImprovementButtonView>();
                for (var i = 0; i < improveButtons.Length; i++)
                {
                    improveButtons[i].SetFeatureName(businessData.BusinessImprovements[i].Name);
                    improveButtons[i].SetFeatureValue((int) businessData.BusinessImprovements[i].MultiplierPercent);
                    improveButtons[i].SetPrice((int) businessData.BusinessImprovements[i].Price);
                    if (improvements.Values[i]) improveButtons[i].SetPurchased();
                    improveButtons[i].Button.interactable = !improvements.Values[i];
                }
            }

            gameData.GameEvents.OnBusinessImprovementPurchased.AsObservable().Subscribe(UpdateAfterImprovement).AddTo(_disposables);
        }

        public void Run(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();

            var businesses = ecsWorld.Filter<Business>().End();

            var businessDataPool = ecsWorld.GetPool<Business>();
            var improvementsPool = ecsWorld.GetPool<BusinessImprovements>();
            var currentBalancePool = ecsWorld.GetPool<CurrentBalance>();
            var objectReferencesPool = ecsWorld.GetPool<UnityObjectReference>();

            foreach (var entity in businesses)
            {
                var business = businessDataPool.Get(entity);
                var improvements = improvementsPool.Get(entity);
                var view = (BusinessCardView) objectReferencesPool.Get(entity).UnityObject;

                var currentBalance = currentBalancePool.Get(entity);
                var income = FinancialCalculator.GetBusinessIncomeByComponents(_systems, business, improvements);
                view.ProgressBar.value = income <= 0.0 ? 0.0f : Mathf.Clamp01((float) (currentBalance.Value / income));
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _disposables.Dispose();
        }

        private void UpdateAfterLevelUp(int businessIndex)
        {
            var ecsWorld = _systems.GetWorld();

            var businesses = ecsWorld.Filter<Business>().End();

            var businessPool = ecsWorld.GetPool<Business>();
            var improvementsPool = ecsWorld.GetPool<BusinessImprovements>();
            var objectReferencePool = ecsWorld.GetPool<UnityObjectReference>();

            var businessesData = _systems.GetShared<GameData>().BusinessesData;

            foreach (var entity in businesses)
            {
                ref var business = ref businessPool.Get(entity);
                if (business.Index != businessIndex) continue;

                var improvements = improvementsPool.Get(entity);
                var view = (BusinessCardView) objectReferencePool.Get(entity).UnityObject;
                view.LvlUpButton.SetPrice((int) FinancialCalculator.GetPriceForBusinessLevel(business.CurrentLevel + 1, businessesData[businessIndex].BasePrice));
                view.SetIncome((int) FinancialCalculator.GetBusinessIncomeByComponents(_systems, business, improvements));
                view.SetLevel(business.CurrentLevel);
            }
        }

        private void UpdateAfterImprovement((int businessIndex, int improvementIndex) target)
        {
            var (businessIndex, improvementIndex) = target;

            var ecsWorld = _systems.GetWorld();

            var businesses = ecsWorld.Filter<Business>().End();

            var businessPool = ecsWorld.GetPool<Business>();
            var improvementsPool = ecsWorld.GetPool<BusinessImprovements>();
            var objectReferencePool = ecsWorld.GetPool<UnityObjectReference>();

            foreach (var entity in businesses)
            {
                var business = businessPool.Get(entity);
                if (business.Index != businessIndex) continue;

                var improvements = improvementsPool.Get(entity);
                var cardView = (BusinessCardView) objectReferencePool.Get(entity).UnityObject;
                cardView.SetIncome((int) FinancialCalculator.GetBusinessIncomeByComponents(_systems, business, improvements));

                var improvementView = cardView.ImproveButtonsRoot.GetChild(improvementIndex).GetComponent<ImprovementButtonView>();

                improvementView.SetPurchased();
                improvementView.Button.interactable = false;
            }
        }
    }
}