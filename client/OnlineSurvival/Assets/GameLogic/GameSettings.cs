using OS.Players;
using UnityEngine;
using Zenject;

namespace OS.GameLogic
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "OS/GameSettings")]
    public sealed class GameSettings : ScriptableObjectInstaller
    {
        [Header("Игрок")]
        [SerializeField] PlayerConfig player;


        public override void InstallBindings()
        {
            //Связанное с игроком
            Container.BindInstance(player);

        }
    }
}
