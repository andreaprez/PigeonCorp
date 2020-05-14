using PigeonCorp.MainTopBar;
using Zenject;

namespace PigeonCorp.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Containers.Scene = Container;
            
            Container
                .Bind<MainTopBarViewModel>()
                .AsSingle();
        }
    }
}
