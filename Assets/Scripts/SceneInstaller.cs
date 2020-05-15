using PigeonCorp.MainBuyButton;
using PigeonCorp.MainScreen;
using PigeonCorp.MainTopBar;
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
        }
        
        private void BindMediators()
        {
            ProjectContext.Instance.Container
                .Bind<MainTopBarMediator>()
                .AsSingle();
            
            ProjectContext.Instance.Container
                .Bind<MainBuyButtonMediator>()
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
