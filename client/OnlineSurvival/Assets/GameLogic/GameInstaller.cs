using Leopotam.EcsLite;
using Game.ECS;
using Game.Input;
using Game.Muliplayer;
using Zenject;

namespace Game.MainLogic
{
    public sealed class GameInstaller : MonoInstaller
    {
        public ColyseusConnector connector;


        public override void InstallBindings()
        {
            BindESC();
            BindInput();
            BindConnector();
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

        private void BindConnector()
        {
            Container.BindInterfacesTo<ColyseusConnector>()
                .FromInstance(connector);
        }
    }
}
