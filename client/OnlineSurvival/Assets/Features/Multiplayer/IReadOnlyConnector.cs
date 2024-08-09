using Colyseus;

namespace OS.Muliplayer
{
    public interface IReadOnlyConnector
    {
        ColyseusRoom<MyRoomState> Room { get; }
        ColyseusClient Client { get; }
    }
}