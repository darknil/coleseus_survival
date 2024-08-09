using Leopotam.EcsLite;
using OS.Input;
using Zenject;

namespace OS.GameLogic
{
    public sealed class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindESC();
            BindInput();
        }


        private void BindESC()
        {
            Container.Bind<EcsWorld>().AsSingle();

            Container.BindInterfacesTo<ESCInstaller>()
                .AsSingle()
                .NonLazy();
        }

        private void BindInput()
        {
            Container.Bind<GameInput>()
                            .AsSingle()
                            .NonLazy();
        }
    }
}
