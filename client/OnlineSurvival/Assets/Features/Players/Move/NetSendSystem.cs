using Leopotam.EcsLite;
using Game.Ecs.Multiplayer;
using Game.ECS.Move;
using Game.Muliplayer;
using UnityEngine;

namespace Game.ECS.Players.Move
{
    public sealed class NetSendSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter filter;
        private EcsPool<MovableComponent> movables;

        private readonly IReadOnlyConnector connector;

        public NetSendSystem(IReadOnlyConnector connector)
        {
            this.connector = connector;
        }

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            movables = world.GetPool<MovableComponent>();

            filter = world.Filter<NetSendMarker>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in filter)
            {
                ref var movable = ref movables.Get(entity);

                if (movable.rigidbody.velocity != Vector2.zero)
                    connector.Room.Send("move", (Vector2)movable.transform.position);
            }
        }
    }
}
