using BusinessClicker.Data;
using BusinessClicker.Data.Views;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Finance.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessClicker.Ecs.SceneInitializer
{
    public class SceneInitSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            var sharedData = systems.GetShared<StartupData>();

            var canvas = Object.FindObjectOfType<Canvas>();

            var mainWindowGO = Object.Instantiate(sharedData.MainWindowPrefab, canvas.transform);
            var mainWindowView = mainWindowGO.GetComponent<MainWindowView>();

            var userBalanceEntity = ecsWorld.NewEntity();
            ecsWorld.GetPool<CurrentBalance>().Add(userBalanceEntity);
            ref var mainWindowViewReference = ref ecsWorld.GetPool<UnityObjectReference>().Add(userBalanceEntity);
            mainWindowViewReference.UnityObject = mainWindowView;
            
            foreach (var businessData in sharedData.BusinessesData)
            {
                var cardGO = Object.Instantiate(sharedData.BusinessCardPrefab, mainWindowView.Scroll.content);
                var cardView = cardGO.GetComponent<BusinessCardView>();

                var cardEntity = ecsWorld.NewEntity();
                ecsWorld.GetPool<CurrentBalance>().Add(cardEntity);
                ecsWorld.GetPool<CurrentIncome>().Add(cardEntity);
                ref var cardReference = ref ecsWorld.GetPool<UnityObjectReference>().Add(cardEntity);
                cardReference.UnityObject = cardView;

                foreach (var _ in businessData.BusinessImprovements)
                {
                    Object.Instantiate(sharedData.ImproveButtonPrefab, cardView.ImproveButtonsRoot);
                }
            }
        }
    }
}