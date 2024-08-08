using OS.Muliplayer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OS.PlayerSystem
{
    public class PlayerController : Player
    {
        public float moveSpeed = 5f;

        public Rigidbody2D RB;

        private Vector2 _targetPosition;
        private Vector2 position;

        private bool _moving;


        void FixedUpdate()
        {
            // Перемещаем персонажа в соответствии с вводом
            RB.velocity = position * moveSpeed;

            if (_moving && (Vector2)transform.position != position)
            {
                var step = moveSpeed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, position, step);
                ColyseusConnector.Instance.Room.Send("move", new { position.x, position.y });
            }
            else
            {
                _moving = false;
            }
        }


        // Метод, который будет вызван системой ввода при движении
        public void OnMove(InputValue value)
        {
            position = value.Get<Vector2>();
        }
    }
}
