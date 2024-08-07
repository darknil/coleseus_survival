using Colyseus.Schema;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 position;
    private Rigidbody2D rb;
    private NetworkManager _networkManager;

    private bool _moving;


    public async void Start()
    {
        _networkManager = gameObject.AddComponent<NetworkManager>();
        await _networkManager.JoinOrCreateGame();

        // Assigning listener for incoming messages
        _networkManager.GameRoom.OnMessage<string>("welcomeMessage", message =>
        {
            Debug.Log(message);
        });

        // Set player's new position after synchronized the mouse click's position with the Colyseus server.
        _networkManager.GameRoom.State.OnChange(() =>
        {
            var player = _networkManager.GameRoom.State.players[_networkManager.GameRoom.SessionId];
            position = new Vector2(player.x, player.y);
            _moving = true;
        });

        _networkManager.GameRoom.State.players.OnAdd((key, player) =>
        {
            Debug.Log($"Player {key} has joined the Game!");
        });
    }

    void FixedUpdate()
    {
        // Перемещаем персонажа в соответствии с вводом
        rb.velocity = position * moveSpeed;
        _networkManager.PlayerPosition(rb.velocity);

        if (_moving && (Vector2)transform.position != position)
        {
            var step = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, position, step);
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
