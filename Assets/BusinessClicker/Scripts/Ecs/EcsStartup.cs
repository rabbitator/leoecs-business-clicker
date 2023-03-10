using BusinessClicker.Data;
using BusinessClicker.Ecs.BusinessBehaviour.Systems;
using BusinessClicker.Ecs.BusinessUpgrade.Systems;
using BusinessClicker.Ecs.Improvement.Systems;
using BusinessClicker.Ecs.Progress.Systems;
using BusinessClicker.Ecs.SceneInitializer.Systems;
using BusinessClicker.Ecs.UserIncome.Systems;
using BusinessClicker.Ecs.VisualUpdate.Systems;
using BusinessClicker.Services;
using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessClicker.Ecs
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField]
        private Configuration _configuration;

        private EcsWorld _ecsWorld;
        private IEcsSystems _initSystems;
        private IEcsSystems _updateSystems;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            _ecsWorld = new EcsWorld();

            var gameEvents = new GameEvents();

            var gameData = new GameData
            {
                MainWindowPrefab = _configuration.MainWindowPrefab,
                BusinessCardPrefab = _configuration.BusinessCardPrefab,
                ImproveButtonPrefab = _configuration.ImproveButtonPrefab,
                BusinessesData = _configuration.BusinessesData,
                GameEvents = gameEvents
            };

            _initSystems = new EcsSystems(_ecsWorld, gameData);
            _initSystems.Add(new SceneInitSystem());
            _initSystems.Add(new ProgressLoaderSystem());
            _initSystems.Add(new ProgressSaverSystem());
            _initSystems.Add(new UserIncomeSystem());
            _initSystems.Add(new BusinessUpgradeSystem());
            _initSystems.Add(new ImproveBusinessSystem());
            _initSystems.Add(new UserBalanceVisualUpdateSystem());
            _initSystems.Init();

            _updateSystems = new EcsSystems(_ecsWorld, gameData);
            _updateSystems.Add(new BusinessIncomeSystem());
            _updateSystems.Add(new BusinessVisualUpdateSystem());
            _updateSystems.Init();
        }

        private void Update()
        {
            _updateSystems.Run();
        }

        private void OnDestroy()
        {
            _initSystems.Destroy();
            _updateSystems.Destroy();
            _ecsWorld.Destroy();
        }
    }
}