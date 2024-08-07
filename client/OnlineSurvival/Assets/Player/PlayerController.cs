using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 position;
    private Rigidbody2D rb;

    private bool _moving;

    void FixedUpdate()
    {
        // Перемещаем персонажа в соответствии с вводом
        rb.velocity = position * moveSpeed;
        //_networkManager.PlayerPosition(rb.velocity);

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
