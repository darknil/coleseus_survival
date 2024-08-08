using OS.Muliplayer;
using Zenject;

namespace OS.Multiplayer
{
    public class ConnectorInstaller : MonoInstaller
    {
        public ColyseusConnector connector;
        public ColyseusConnector.Settings settings;


        public override void InstallBindings()
        {
            Container.BindInstance(settings);

            Container.BindInterfacesTo<ColyseusConnector>()
                .FromInstance(connector);
        }
    }
}
