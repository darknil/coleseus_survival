using System;
using System.Collections.Generic;
using Colyseus;
using Leopotam.EcsLite;
using Game.Ecs.Multiplayer;
using Game.ECS.Input;
using Game.ECS.Move;
using Game.ECS.Players;
using Game.Players;
using Game.PlayerSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Muliplayer
{
    public class ColyseusConnector : MonoBehaviour, IReadOnlyConnector
    {
        public ColyseusRoom<MyRoomState> Room => room;
        public ColyseusClient Client => client;

        private DiContainer container;
        private Settings settings;
        private EcsWorld ecsWord;

        private static ColyseusRoom<MyRoomState> room = null;
        private Dictionary<string, PlayerNickname> players = new();
        private ColyseusClient client;


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
            room = await client.JoinOrCreate<MyRoomState>(Association.Multiplayer.DEFAULT_ROOM_NAME,
                                                          new() { ["name"] = PlayerPrefs.GetString(Association.PlayerPrefs.PLAYER_NAME) });
            CreateLocalPlayer();

            SubscribeRoom();

            Debug.Log($"Попытка подключения к лобби {(room != null ? "УСПЕШНА" : "НЕУДАЧНА")}");
        }

        public void TryLeave()
        {
            if (room == null || !room.colyseusConnection.IsOpen) return;

            DestroyLocalPlayer();
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

            var player = Instantiate(settings.otherPlayerPrefab);
            player.GetComponentInChildren<TMP_Text>().text = playerData.name;
            var netPlayerData = room.State.players[key];
            netPlayerData.OnChange(() => player.transform.position = new(netPlayerData.x, netPlayerData.y));
            players.Add(key, player);

            Debug.Log($"Клиент обработал вход пидора: {playerData.name}");
        }

        private void Leave(string key, PlayerData player)
        {
            Destroy(players[key].gameObject);
            players.Remove(key);

            Debug.Log($"Клиент обработал выход пидора: {player.name}");
        }

        private void CreateLocalPlayer()
        {
            var playerGO = Instantiate(settings.playerPrefab);
            var playerEntity = ecsWord.NewEntity();
            var config = container.Resolve<PlayerConfig>();


            // basePlayer
            var playersPool = ecsWord.GetPool<PlayerComponent>();
            ref var playerComponent = ref playersPool.Add(playerEntity);
            playerComponent.name = config.Nickname;
            playerGO.GetComponentInChildren<TMP_Text>().text = config.Nickname;

            // inputs
            var inputPlayersPool = ecsWord.GetPool<PlayerInputComponent>();
            ref var playerInputComponent = ref inputPlayersPool.Add(playerEntity);

            // move
            var movablePool = ecsWord.GetPool<MovableComponent>();
            ref var movableComponent = ref movablePool.Add(playerEntity);
            movableComponent.transform = playerGO.transform;
            movableComponent.rigidbody = playerGO.GetComponent<Rigidbody2D>();
            movableComponent.speed = config.Speed;

            //net
            var netSendPool = ecsWord.GetPool<NetSendMarker>();
            netSendPool.Add(playerEntity);
        }

        private void DestroyLocalPlayer()
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
            public GameObject otherPlayerPrefab;
        }
    }
}
