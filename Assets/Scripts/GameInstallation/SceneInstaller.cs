using PigeonCorp.Evolution.Adapter;
using PigeonCorp.Hatcheries.Adapter;
using PigeonCorp.MainBuyButton.Adapter;
using PigeonCorp.MainScreen.Framework;
using PigeonCorp.MainTopBar.Adapter;
using PigeonCorp.Research.Adapter;
using PigeonCorp.Shipping.Adapter;
using Zenject;

namespace PigeonCorp.GameInstallation
{
    public class SceneInstaller : MonoInstaller
    {
        private DiContainer _container;
        
        public override void InstallBindings()
        {
            BindViewModels();
            BindMediators();
            BindPrefabs();
        }

        private void BindViewModels()
        {
            ProjectContext.Instance.Container
                .Bind<MainTopBarViewModel>()
                .AsSingle();
            
            ProjectContext.Instance.Container
                .Bind<MainBuyButtonViewModel>()
                .AsSingle();
            
            ProjectContext.Instance.Container
                .Bind<HatcheriesViewModel>()
                .AsSingle();
            
            ProjectContext.Instance.Container
                .Bind<ShippingViewModel>()
                .AsSingle();
            
            ProjectContext.Instance.Container
                .Bind<ResearchViewModel>()
                .AsSingle();
            
            ProjectContext.Instance.Container
                .Bind<EvolutionViewModel>()
                .AsSingle();
        }
        
        private void BindMediators()
        {
            ProjectContext.Instance.Container
                .Bind<MainTopBarMediator>()
                .AsSingle();
            
            ProjectContext.Instance.Container
                .Bind<MainBuyButtonMediator>()
                .AsSingle();
            
            ProjectContext.Instance.Container
                .Bind<HatcheriesMediator>()
                .AsSingle();
            
            ProjectContext.Instance.Container
                .Bind<ShippingMediator>()
                .AsSingle();
            
            ProjectContext.Instance.Container
                .Bind<ResearchMediator>()
                .AsSingle();
            
            ProjectContext.Instance.Container
                .Bind<EvolutionMediator>()
                .AsSingle();
        }

        private void BindPrefabs()
        {
            ProjectContext.Instance.Container
                .Bind<PigeonBehaviour>()
                .FromResource("Prefabs/Main/World/Pigeon");
        }
    }
}
