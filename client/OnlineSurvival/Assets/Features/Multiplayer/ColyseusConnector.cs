using System;
using System.Collections.Generic;
using Assets.Helpers;
using Colyseus;
using Leopotam.EcsLite;
using OS.Input;
using OS.Move;
using OS.Players;
using OS.PlayerSystem;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
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
        private EcsWorld ecsWord;

        private static ColyseusRoom<MyRoomState> room = null;
        private Dictionary<string, PlayerNickname> players = new();
        private ColyseusClient client;
        private PlayerController localPlayer;


        [Inject]
        public void Construct(Settings settings, DiContainer container, EcsWorld ecsWorld)
        {
            this.settings = settings;
            this.container = container;
            this.ecsWord = ecsWorld;
        }

        public void Start()
        {
            client = new("ws://localhost:2567");
        }

        public async void TryJoin()
        {
            if (room != null && room.colyseusConnection.IsOpen) return;

            // Подключение или создание комнаты
            room = await client.JoinOrCreate<MyRoomState>(DEFAULT_ROOM_NAME, new() { ["name"] = PlayerPrefs.GetString(Association.PlayerPrefs.PLAYER_NAME) } );
            //localPlayer = CreatePlayer();
            CreateLocalPlayer();

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

            var player = container.InstantiatePrefabForComponent<PlayerNickname>(settings.otherPlayerPrefab);
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
            player.SetName(PlayerPrefs.GetString(Association.PlayerPrefs.PLAYER_NAME));

            return player;
        }

        private void CreateLocalPlayer()
        {
            var playerGO = Instantiate(settings.playerPrefab);
            var playerEntity = ecsWord.NewEntity();
            var config = container.Resolve<PlayerConfig>();

            var players = ecsWord.GetPool<PlayerComponent>();
            var inputs = ecsWord.GetPool<PlayerInputComponent>();
            var movables = ecsWord.GetPool<MovableComponent>();

            // basePlayer
            ref var playerComponent = ref players.Add(playerEntity);
            playerComponent.name = config.Nickname;
            playerGO.GetComponentInChildren<TMP_Text>().text = config.Nickname;

            // inputs
            ref var playerInputComponent = ref inputs.Add(playerEntity);

            // move
            ref var movableComponent = ref movables.Add(playerEntity);
            movableComponent.transform = playerGO.transform;
            movableComponent.rigidbody = playerGO.GetComponent<Rigidbody2D>();
            movableComponent.speed = config.Speed;
        }

        private void DestroyPlayer()
        {
            var filter = ecsWord.Filter<MovableComponent>().Exc<PlayerInputComponent>().End();
            var movables = ecsWord.GetPool<MovableComponent>();

            foreach (var entity in filter)
            {
                ref var player = ref movables.Get(entity);
                Destroy(player.transform.gameObject);
            }
        }


        [Serializable]
        public class Settings 
        {
            public GameObject playerPrefab;
            public PlayerNickname otherPlayerPrefab;
        }
    }
}
