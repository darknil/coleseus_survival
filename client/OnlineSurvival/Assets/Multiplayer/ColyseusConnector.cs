using Colyseus;
using UnityEditor.PackageManager;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private const string DEFAULT_ROOM_NAME = "1KG_CHLENA";

    
    public ColyseusRoom<MyRoomState> MyRoom
    {
        get
        {
            if (room == null)
            {
                Debug.LogError("Рума не инициализированная. Саня че смотришь?");
            }
            return room;
        }
    }

    //public ColyseusClient Client
    //{
    //    get
    //    {
    //        // Initialize Colyseus client, if the client has not been initiated yet or input values from the Menu have been changed.
    //        if (_client == null || !_client.Settings.WebRequestEndpoint.Contains(_menuManager.HostAddress))
    //        {
    //            Initialize();
    //        }
    //        return _client;
    //    }
    //}


    public PlayerController playerPrefab;

    private static ColyseusRoom<MyRoomState> room = null;
    private static ColyseusClient client;


    async void Start()
    {
        // Подключение к серверу
        client = new("ws://localhost:2567");

        // Подключение или создание комнаты
        room = await client.JoinOrCreate<MyRoomState>("my_room");

        // Подписка на изменение состояния комнаты
        room.OnJoin += Joined;
        room.OnLeave += Leave;
        //myRoom.OnStateChange += OnPlayerChange;
    }

    private void Joined()
    {
        //room.State.OnChange();
    }

    private void Leave(int _)
    {
        
    }



    private void CreatePlayer()
    {
        Instantiate(playerPrefab);
    }
}
