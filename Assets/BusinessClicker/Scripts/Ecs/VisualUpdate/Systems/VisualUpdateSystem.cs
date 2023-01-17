using BusinessClicker.Data;
using BusinessClicker.Data.Views;
using BusinessClicker.Ecs.Common.Components;
using BusinessClicker.Ecs.Finance.Components;
using Leopotam.EcsLite;

namespace BusinessClicker.Ecs.VisualUpdate.Systems
{
    public class VisualUpdateSystem : IEcsInitSystem, IEcsRunSystem
    {
        public void Init(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();
            var startupData = systems.GetShared<StartupData>();

            var businesses = ecsWorld.Filter<CurrentBalance>().Inc<CurrentIncome>().End();
            var objectReferencesPool = ecsWorld.GetPool<UnityObjectReference>();

            foreach (var entity in businesses)
            {
                var view = (BusinessCardView) objectReferencesPool.Get(entity).UnityObject;
            }
        }

        public void Run(IEcsSystems systems)
        {
            
        }
    }
}