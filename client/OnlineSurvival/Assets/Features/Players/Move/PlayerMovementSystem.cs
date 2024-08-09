using Leopotam.EcsLite;
using OS.Input;

namespace OS.Move
{
    public sealed class PlayerMovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter filter;
        private EcsPool<MovableComponent> movables;
        private EcsPool<PlayerInputComponent> inputPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            movables = world.GetPool<MovableComponent>();
            inputPool = world.GetPool<PlayerInputComponent>();

            // Создаем и кэшируем фильтр для сущностей с нужными компонентами
            filter = world.Filter<MovableComponent>().Inc<PlayerInputComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in filter)
            {
                ref var movable = ref movables.Get(entity);
                ref var input = ref inputPool.Get(entity);

                // Обновляем позицию на основе направления ввода и скорости
                movable.rigidbody.velocity = movable.speed * input.inputVector;
            }
        }
    }
}
