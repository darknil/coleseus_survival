using Colyseus;
using UnityEngine;

public class ColyseusConnector : MonoBehaviour
{
    private const string DEFAULT_ROOM_NAME = "my_room";

    public PlayerController playerPrefab;

    public ColyseusRoom<MyRoomState> Room
    { 
        get => room;
        private set
        {
            if (value == null) return;
            room = value;
            //room.OnJoin += Joined;
            //room.OnLeave += Leave;
        }
    }

    public ColyseusClient Client => client;

    private ColyseusRoom<MyRoomState> room = null;
    private ColyseusClient client;


    public void Start()
    {
        client = new("ws://localhost:2567");
    }

    public async void TryJoin()
    {
        // Подключение или создание комнаты
        room = await client.Create<MyRoomState>(DEFAULT_ROOM_NAME);

        Debug.Log($"Попытка подключения к лобби {(room != null ? "УСПЕШНА" : "НЕУДАЧНА")}");

        room.State.players.ForEach((value, player) => Debug.Log($"value: {value};\nplayer: {player}"));
    }

    public void TryLeave()
    {
        room.Leave();

        Debug.Log($"Соединение {(room.colyseusConnection.IsOpen ? "не удалось закрыть" : "закрыто")}");
    }

    private void Joined()
    {
        CreatePlayer();

        Debug.Log("Клиент обработал вход");
    }

    private void Leave(int _)
    {
        Debug.Log("Клиент обработал выход");
    }


    private void CreatePlayer()
    {
        Instantiate(playerPrefab);
    }
}
