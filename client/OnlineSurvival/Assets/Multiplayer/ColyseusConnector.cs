using System.Collections.Generic;
using Assets.Helpers;
using Colyseus;
using OS.PlayerSystem;
using UnityEngine;

namespace OS.Muliplayer
{
    public class ColyseusConnector : MonoBehaviourSingleton<ColyseusConnector>
    {
        private const string DEFAULT_ROOM_NAME = "my_room";

        public PlayerController playerPrefab;
        public Player otherPlayerPrefab;

        public ColyseusRoom<MyRoomState> Room => room;
        public ColyseusClient Client => client;

        private static ColyseusRoom<MyRoomState> room = null;
        private Dictionary<string, Player> players = new();
        private ColyseusClient client;
        private PlayerController localPlayer;


        public void Start()
        {
            client = new("ws://localhost:2567");
        }

        public async void TryJoin()
        {
            if (room != null && room.colyseusConnection.IsOpen) return;

            localPlayer = CreatePlayer();
            // Подключение или создание комнаты
            room = await client.JoinOrCreate<MyRoomState>(DEFAULT_ROOM_NAME, new() { ["name"] = localPlayer.Nickname.text } );
            SubscribeRoom();

            Debug.Log($"Попытка подключения к лобби {(room != null ? "УСПЕШНА" : "НЕУДАЧНА")}");
        }

        public void TryLeave()
        {
            if (room == null || !room.colyseusConnection.IsOpen) return;

            DestroyPlayer();
            room.Leave();

            Debug.Log($"Соединение {(room.colyseusConnection.IsOpen ? "не удалось закрыть" : "закрыто")}");
        }


        private void SubscribeRoom()
        {
            room.State.players.OnAdd(Joined);
            room.State.players.OnRemove(Leave);
        }

        private void Joined(string key, PlayerData player)
        {
            if (room.SessionId == key) return;

            var other = Instantiate(otherPlayerPrefab);
            other.SetName(player.name);
            players.Add(key, other);

            Debug.Log($"Клиент обработал вход пидора: {player.name}");
        }

        private void Leave(string key, PlayerData player)
        {
            Destroy(players[key]);
            players.Remove(key);

            Debug.Log($"Клиент обработал выход пидора: {player.name}");
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
}
