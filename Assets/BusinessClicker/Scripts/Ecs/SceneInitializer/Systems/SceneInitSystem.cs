using System;
using BusinessClicker.Data;
using BusinessClicker.Data.Views;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Income.Components;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BusinessClicker.Ecs.SceneInitializer.Systems
{
    public class SceneInitSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            var sharedData = systems.GetShared<GameData>();

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
                ecsWorld.GetPool<Business>().Add(cardEntity);
                ref var business = ref ecsWorld.GetPool<Business>().Get(cardEntity);
                business.Id = Array.IndexOf(sharedData.BusinessesData, businessData);

                ecsWorld.GetPool<CurrentBalance>().Add(cardEntity);
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