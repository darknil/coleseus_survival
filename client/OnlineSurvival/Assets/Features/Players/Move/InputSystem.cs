using Leopotam.EcsLite;
using Game.ECS.Input;
using Game.Input;
using UnityEngine;

namespace Game.ECS.Players.Move
{
    public sealed class InputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter filter;
        private EcsPool<PlayerInputComponent> playersInput;

        private readonly GameInput gameInput;

        public InputSystem(GameInput gameInput) 
        { 
            this.gameInput = gameInput;
        }

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            gameInput.Player.Enable();

            playersInput = world.GetPool<PlayerInputComponent>();

            filter = world.Filter<PlayerInputComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter)
            {
                ref var playerInput = ref playersInput.Get(entity);
                playerInput.inputVector = gameInput.Player.Move.ReadValue<Vector2>();
            }
        }
    }
}
