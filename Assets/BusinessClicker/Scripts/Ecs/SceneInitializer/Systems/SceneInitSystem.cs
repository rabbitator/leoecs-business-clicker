using System;
using BusinessClicker.Data;
using BusinessClicker.Data.Views;
using BusinessClicker.Ecs.BusinessBehaviour.Components;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Improvement.Components;
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

                var businessEntity = ecsWorld.NewEntity();
                ecsWorld.GetPool<Business>().Add(businessEntity);
                ref var business = ref ecsWorld.GetPool<Business>().Get(businessEntity);
                business.Index = Array.IndexOf(sharedData.BusinessesData, businessData);

                ecsWorld.GetPool<CurrentBalance>().Add(businessEntity);
                ref var cardReference = ref ecsWorld.GetPool<UnityObjectReference>().Add(businessEntity);
                cardReference.UnityObject = cardView;

                ref var improvements = ref ecsWorld.GetPool<BusinessImprovements>().Add(businessEntity);
                improvements.Value = new bool[businessData.BusinessImprovements.Length];

                foreach (var _ in businessData.BusinessImprovements)
                {
                    Object.Instantiate(sharedData.ImproveButtonPrefab, cardView.ImproveButtonsRoot);
                }
            }
        }
    }
}