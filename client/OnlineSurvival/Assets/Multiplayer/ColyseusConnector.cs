using Assets.Helpers;
using Colyseus;
using UnityEngine;

public class ColyseusConnector : MonoBehaviourSingleton<ColyseusConnector>
{
    private const string DEFAULT_ROOM_NAME = "my_room";

    public PlayerController playerPrefab;
    public GameObject otherPlayerPrefab;

    public ColyseusRoom<MyRoomState> Room => room;
    public ColyseusClient Client => client;

    private static ColyseusRoom<MyRoomState> room = null;
    private ColyseusClient client;
    private PlayerController localPlayer;


    public void Start()
    {
        client = new("ws://localhost:2567");
    }

    public async void TryJoin()
    {
        localPlayer = CreatePlayer();
        // Подключение или создание комнаты
        room = await client.JoinOrCreate<MyRoomState>(DEFAULT_ROOM_NAME, new() { ["name"] = localPlayer.Nickname.text } );
        SubscribeRoom();

        Debug.Log($"Попытка подключения к лобби {(room != null ? "УСПЕШНА" : "НЕУДАЧНА")}");
    }

    public void TryLeave()
    {
        DestroyPlayer();
        room.Leave();

        Debug.Log($"Соединение {(room.colyseusConnection.IsOpen ? "не удалось закрыть" : "закрыто")}");
    }

    private void SubscribeRoom()
    {
        room.State.players.OnAdd(Joined);
    }

    private void Joined(string key, Player player)
    {
        if (room.SessionId == key) return;

        Instantiate(otherPlayerPrefab);

        Debug.Log("Клиент обработал вход пидора." +
            $"\nkey: {key}, player = x:{player.x}, y:{player}");
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
