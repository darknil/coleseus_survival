using Zenject;

namespace Game.Input
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
