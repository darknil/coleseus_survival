using Game.Muliplayer;
using Game.Players;
using UnityEngine;
using Zenject;

namespace Game.MainLogic
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "OS/GameSettings")]
    public sealed class GameSettings : ScriptableObjectInstaller
    {
        [Header("Игрок")]
        [SerializeField] PlayerConfig player;

        [Header("Мультиплеер")]
        public ColyseusConnector.Settings connector;

        public override void InstallBindings()
        {
            //Связанное с игроком
            Container.BindInstance(player);

            //Мультиплеерное
            Container.BindInstance(connector);
        }
    }
}
