using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;

public class PlayerController : MonoBehaviour
{
    [Header("BasicMove")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float inAirAcceleration = 30f;
    [Header("Grounded&Jump")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;
    [Space]
    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    private bool dashing;
    [Space]
    [Header("References")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerInfoSO playerInfo;

    private Vector3 velocity = Vector3.zero;
    private Controls input;
    private Vector2 moveInput;
    private bool isJump;
    private bool jumpHeld;
    private bool isDash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        input = new Controls();
        input.Enable();
        dashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        grounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, whatIsGround);

        Move();
        CallDash();
        Gravity();
        Jump();
        controller.Move(velocity * Time.deltaTime);

        playerInfo.playerVelocity = velocity;
        playerInfo.playerPosition = transform.position;
    }

    void GetInput()
    {
        moveInput = input.Player.Move.ReadValue<Vector2>();
        isJump = input.Player.Jump.WasPressedThisFrame();
        jumpHeld = input.Player.Jump.IsPressed();
        isDash = input.Player.Dash.WasPressedThisFrame();
    }

    void Move()
    {
        Vector3 direction = transform.forward * moveInput.y + transform.right * moveInput.x;
        Vector3 target = direction * moveSpeed;
        if (grounded)
        {
            Vector3 velocityXZ = new Vector3(velocity.x, 0f, velocity.z);
            velocityXZ = Vector3.Lerp(velocityXZ, target, acceleration * Time.deltaTime);
            velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
        }
        else
        {
            if (direction != Vector3.zero)
            {
                Vector3 velocityXZ = new Vector3(velocity.x, 0f, velocity.z);
                velocityXZ = Vector3.Lerp(velocityXZ, target, inAirAcceleration * Time.deltaTime);
                velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
            }
        }
    }

    void Jump()
    {
        if (grounded && isJump)
        {
            velocity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
        }
    }

    void Gravity()
    {
        if (!grounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (grounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
    }

    void CallDash()
    {
        if (isDash && !dashing)
        {
            dashing = true;
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        float time = 0f;
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        if (moveDirection == Vector3.zero) moveDirection = transform.forward;
        while (time < dashTime)
        {
            controller.Move(moveDirection * dashSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        dashing = false;
    }
}
