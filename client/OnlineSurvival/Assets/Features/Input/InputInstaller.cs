using Zenject;

namespace OS.Input
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameInput>()
                .AsSingle()
                .NonLazy();
        }
    }
}
