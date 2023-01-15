using BusinessClicker.Data;
using BusinessClicker.Data.ObjectsStructure;
using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessClicker.Ecs.SceneInitializer
{
    public class SceneInitSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            var runtimeData = systems.GetShared<RuntimeData>();

            var canvas = Object.FindObjectOfType<Canvas>();
            var listWindow = Object.Instantiate(runtimeData.ListWindowPrefab, canvas.transform).GetComponent<ListWindow>();

            foreach (var business in runtimeData.BusinessesData)
            {
                var card = Object.Instantiate(runtimeData.BusinessCardPrefab, listWindow.Scroll.content).GetComponent<BusinessCard>();

                foreach (var _ in business.BusinessImprovements)
                {
                    Object.Instantiate(runtimeData.ImproveButtonPrefab, card.ImproveButtonsRoot);
                }
            }
        }
    }
}