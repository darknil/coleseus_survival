using System;
using Leopotam.EcsLite;
using Game.Input;
using Game.Muliplayer;
using Zenject;

namespace Game.ECS
{
    public sealed class ESCInstaller : IInitializable, ITickable, ILateTickable, IDisposable
    {
        // Инициализация окружения.

        private readonly EcsWorld world;
        private IEcsSystems updateSystems;
        private IEcsSystems lateUpdateSystems;

        private readonly GameInput inputs;
        private readonly IReadOnlyConnector connector;

        public ESCInstaller(EcsWorld world, GameInput inputs, IReadOnlyConnector connector)
        {
            this.world = world;
            this.inputs = inputs;
            this.connector = connector;
        }


        public void Initialize()
        {
            // Update
            updateSystems = new EcsSystems(world);
            updateSystems
                .Add(new Players.Move.InputSystem(inputs))
                .Add(new Players.Move.NetSendSystem(connector))
#if UNITY_EDITOR
                // Регистрируем отладочные системы по контролю за состоянием каждого отдельного мира:
                //.Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                // Регистрируем отладочные системы по контролю за текущей группой систем. 
                .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem())
#endif
                .Init();


            // FixedUpdate
            lateUpdateSystems = new EcsSystems(world);
            lateUpdateSystems
                .Add(new Players.Move.MovementSystem())
                .Init();
        }

        public void Tick()
        {
            // Отладочные системы являются обычными ECS-системами и для их корректной работы
            // требуется выполнять их запуск через EcsSystems.Run(), иначе имена сущностей
            // не будут обновляться, к тому же это приведет к постоянным внутренним аллокациям.
            updateSystems?.Run();
        }

        public void LateTick()
        {
            lateUpdateSystems?.Run();
        }

        // Очистка окружения.
        public void Dispose()
        {
            updateSystems?.Destroy();
            lateUpdateSystems?.Destroy();
            world?.Destroy();
            updateSystems = null;
        }
    }
}
