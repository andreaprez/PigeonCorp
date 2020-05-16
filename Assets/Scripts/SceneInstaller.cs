using PigeonCorp.Hatcheries;
using PigeonCorp.MainBuyButton;
using PigeonCorp.MainScreen;
using PigeonCorp.MainTopBar;
using PigeonCorp.Research;
using PigeonCorp.Shipping;
using Zenject;

namespace PigeonCorp.Installers
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
        }

        private void BindPrefabs()
        {
            ProjectContext.Instance.Container
                .Bind<PigeonBehaviour>()
                .FromResource("Prefabs/Main/Pigeon");
        }
    }
}
