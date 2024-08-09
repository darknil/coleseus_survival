using Colyseus;

namespace Game.Muliplayer
{
    public interface IReadOnlyConnector
    {
        ColyseusRoom<MyRoomState> Room { get; }
        ColyseusClient Client { get; }
    }
}