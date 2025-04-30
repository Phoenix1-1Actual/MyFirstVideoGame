using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        // проверка работы земли:
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // сброс скорости по умолчанию:
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //получение данных:
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // создаем вектор движения:
        Vector3 move = transform.right * x + transform.forward * z;//вправо - красная ось, вперед - синяя ось:
        // действительно двигаем игрока:
        controller.Move(move * speed * Time.deltaTime);

        // проверяем может ли игрок прыгать:
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        lastPosition = gameObject.transform.position;
    }
}
