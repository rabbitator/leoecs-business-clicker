using BusinessClicker.Ecs.Finance.Components;
using Leopotam.EcsLite;

namespace BusinessClicker.Ecs.ProgressLoader.Systems
{
    public class ProgressLoaderSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var ecsWorld = systems.GetWorld();

            LoadUserBalanceProgress(ecsWorld);
            LoadBusinessProgress(ecsWorld);
        }

        private static void LoadUserBalanceProgress(EcsWorld ecsWorld)
        {
            var userBalanceFilter = ecsWorld.Filter<CurrentBalance>().Exc<CurrentIncome>().End();
            var balancePool = ecsWorld.GetPool<CurrentBalance>();

            foreach (var userBalance in userBalanceFilter)
            {
                ref var tmp = ref balancePool.Get(userBalance);
                tmp.Value = 9599.0f;
            }
        }

        private static void LoadBusinessProgress(EcsWorld ecsWorld)
        {
            var businessFilter = ecsWorld.Filter<CurrentIncome>().End();
            var balancePool = ecsWorld.GetPool<CurrentBalance>();

            foreach (var business in businessFilter)
            {
                ref var tmp = ref balancePool.Get(business);
                tmp.Value = 959.0f;
            }
        }
    }
}