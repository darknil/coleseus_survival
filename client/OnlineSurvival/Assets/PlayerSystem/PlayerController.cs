using OS.Input;
using OS.Muliplayer;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace OS.PlayerSystem
{
    public class PlayerController : Player
    {
        public float moveSpeed = 5f;

        public Rigidbody2D RB;

        private IReadOnlyConnector readOnlyConnector;
        private GameInput input;
        private Vector2 inputVector;


        [Inject]
        public void Construct(GameInput input, IReadOnlyConnector readOnlyConnector)
        {
            this.input = input;
            this.readOnlyConnector = readOnlyConnector;
        }

        private void Start()
        {
            input.Player.Enable();
            input.Player.Move.performed += OnMove;
            input.Player.Move.canceled += OnMove;
        }

        private void FixedUpdate()
        {
            RB.velocity = inputVector * moveSpeed;

            if (inputVector != Vector2.zero)
            {
                readOnlyConnector.Room.Send("move", new { transform.position.x, transform.position.y });
            }
        }

        // Метод, который будет вызван системой ввода при движении
        public void OnMove(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    inputVector = context.ReadValue<Vector2>();
                    break;
                case InputActionPhase.Canceled:
                    inputVector = Vector2.zero;
                    break;
            }
        }
    }
}
