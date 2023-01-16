using BusinessClicker.Data;
using BusinessClicker.Ecs.RenderUI.Systems;
using BusinessClicker.Ecs.WorldInitializer.Systems;
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
            _initSystems.Add(new MainWindowInitSystem());
            _initSystems.Add(new BusinessCardsInitSystem());
            _initSystems.Add(new UpdateUISystem());
            _initSystems.Init();

            _updateSystems = new EcsSystems(_ecsWorld);
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
