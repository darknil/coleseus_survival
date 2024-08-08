using Assets.Helpers;
using Colyseus;
using UnityEngine;

public class ColyseusConnector : MonoBehaviourSingleton<ColyseusConnector>
{
    private const string DEFAULT_ROOM_NAME = "my_room";

    public PlayerController playerPrefab;
    public GameObject otherPlayerPrefab;

    private PlayerController localPlayer;

    public ColyseusRoom<MyRoomState> Room
    { 
        get => room;
        private set
        {
            if (value == null) return;
            room = value;
            room.OnJoin += Joined;
            room.OnLeave += Leave;
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
        room = await client.JoinOrCreate<MyRoomState>(DEFAULT_ROOM_NAME);
        localPlayer = CreatePlayer();

        Debug.Log($"Попытка подключения к лобби {(room != null ? "УСПЕШНА" : "НЕУДАЧНА")}");
    }

    public void TryLeave()
    {
        DestroyPlayer();
        room.Leave();

        Debug.Log($"Соединение {(room.colyseusConnection.IsOpen ? "не удалось закрыть" : "закрыто")}");
    }

    private void Joined()
    {
        Instantiate(playerPrefab);

        Debug.Log("Клиент обработал вход пидора");
    }

    private void Leave(int _)
    {
        Debug.Log("Клиент обработал выход пидора");
    }


    private PlayerController CreatePlayer()
    {
        var player = Instantiate(playerPrefab);
        player.SetName(PlayerPrefs.GetString(NicknameController.NICKNAME_KEY));

        return player;
    }

    private void DestroyPlayer()
    {
        Destroy(localPlayer.gameObject);
    }
}
