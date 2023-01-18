using BusinessClicker.Data.Views;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Income.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessClicker.Ecs.BusinessUpgrade.Systems
{
    public class LevelUpgradeSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            var businesses = ecsWorld.Filter<Business>().End();
            var objectReferencesPool = ecsWorld.GetPool<UnityObjectReference>();

            foreach (var entity in businesses)
            {
                var cardView = (BusinessCardView) objectReferencesPool.Get(entity).UnityObject;
                cardView.LvlUpButton.Button.onClick.AddListener(() => ButtonClickHandler(entity));
            }
        }

        private void ButtonClickHandler(int entity)
        {
            // Get component with current level for entity
            // Get user wallet entity and current user balance component
            // If user has enough money, subtract amount from his balance and increment business level
            
            Debug.Log($"Business #{entity} level up request received");
        }
    }
}