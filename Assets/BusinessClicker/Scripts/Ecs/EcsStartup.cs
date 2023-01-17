using BusinessClicker.Data;
using BusinessClicker.Ecs.ProgressLoader.Systems;
using BusinessClicker.Ecs.SceneInitializer;
using BusinessClicker.Ecs.VisualUpdate.Systems;
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

        private void Start()
        {
            _ecsWorld = new EcsWorld();

            var startupData = new StartupData
            {
                MainWindowPrefab = _configuration.MainWindowPrefab,
                BusinessCardPrefab = _configuration.BusinessCardPrefab,
                ImproveButtonPrefab = _configuration.ImproveButtonPrefab,
                BusinessesData = _configuration.BusinessesData
            };

            _initSystems = new EcsSystems(_ecsWorld, startupData);
            _initSystems.Add(new SceneInitSystem());
            _initSystems.Add(new ProgressLoaderSystem());
            _initSystems.Init();

            _updateSystems = new EcsSystems(_ecsWorld);
            _updateSystems.Add(new VisualUpdateSystem());
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