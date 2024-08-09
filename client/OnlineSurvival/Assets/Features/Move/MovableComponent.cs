using UnityEngine;

namespace Game.ECS.Move
{
    public struct MovableComponent
    {
        public Transform transform;
        public Rigidbody2D rigidbody;
        public float speed;
    }
}
