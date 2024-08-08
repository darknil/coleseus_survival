using System;
using System.Collections.Generic;
using Assets.Helpers;
using Colyseus;
using OS.PlayerSystem;
using UnityEngine;
using Zenject;

namespace OS.Muliplayer
{
    public class ColyseusConnector : MonoBehaviour, IReadOnlyConnector
    {
        private const string DEFAULT_ROOM_NAME = "my_room";

        public ColyseusRoom<MyRoomState> Room => room;
        public ColyseusClient Client => client;

        private DiContainer container;
        private Settings settings;

        private static ColyseusRoom<MyRoomState> room = null;
        private Dictionary<string, Player> players = new();
        private ColyseusClient client;
        private PlayerController localPlayer;


        [Inject]
        public void Construct(Settings settings, DiContainer container)
        {
            this.settings = settings;
            this.container = container;
        }

        public void Start()
        {
            client = new("ws://localhost:2567");
        }

        public async void TryJoin()
        {
            if (room != null && room.colyseusConnection.IsOpen) return;

            // Подключение или создание комнаты
            room = await client.JoinOrCreate<MyRoomState>(DEFAULT_ROOM_NAME, new() { ["name"] = PlayerPrefs.GetString(NicknameController.NICKNAME_KEY) } );
            localPlayer = CreatePlayer();
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

        private void Joined(string key, PlayerData playerData)
        {
            if (room.SessionId == key) return;

            var player = container.InstantiatePrefabForComponent<Player>(settings.otherPlayerPrefab);
            player.SetName(playerData.name);
            var netPlayer = room.State.players[key];
            netPlayer.OnChange(() => player.transform.position = new(netPlayer.x, netPlayer.y));
            players.Add(key, player);

            Debug.Log($"Клиент обработал вход пидора: {playerData.name}");
        }

        private void Leave(string key, PlayerData player)
        {
            Destroy(players[key].gameObject);
            players.Remove(key);

            Debug.Log($"Клиент обработал выход пидора: {player.name}");
        }


        private PlayerController CreatePlayer()
        {
            var player = container.InstantiatePrefabForComponent<PlayerController>(settings.playerPrefab);
            player.SetName(PlayerPrefs.GetString(NicknameController.NICKNAME_KEY));

            return player;
        }

        private void DestroyPlayer()
        {
            Destroy(localPlayer.gameObject);
        }


        [Serializable]
        public class Settings 
        {
            public PlayerController playerPrefab;
            public Player otherPlayerPrefab;
        }
    }
}
