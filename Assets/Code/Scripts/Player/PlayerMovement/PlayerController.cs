using System.Collections;
using UnityEngine;

public enum PlayerState
{ 
    Standing,
    Grappling,
    Dashing,
}

public class PlayerController : MonoBehaviour
{
    [Header("Grounded&Jump")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private float ceilingCheckDistance = 0.1f;
    [SerializeField] private float ceilingCheckCooldown = 0.2f;
    float ceilingTime;
    [SerializeField] private LayerMask whatIsGround;
    [HideInInspector] public bool grounded { get; private set; }
    [HideInInspector] public bool ceilingHit { get; private set; }
    [Space]
    [Header("References")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerInfoSO playerInfo;
    private GrappleHook grappleHook;
    private PlayerMove playerMove;
    private PlayerDash playerDash;
    //state stuff
    [HideInInspector] public PlayerState activeState;

    //
    [HideInInspector] public Vector3 Velocity;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        grappleHook = GetComponentInChildren<GrappleHook>();
        playerDash = GetComponent<PlayerDash>();
        playerMove = GetComponent<PlayerMove>();
    }

    private void Start()
    {
        Velocity = Vector3.zero;
        activeState = PlayerState.Standing;
    }
    

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, whatIsGround);

        if (ceilingTime > ceilingCheckCooldown)
            ceilingHit = Physics.Raycast(ceilingCheck.position, Vector3.up, ceilingCheckDistance, whatIsGround);

        if (activeState == PlayerState.Standing)
        {
            StandingStateLogic();
        }
        else if (activeState == PlayerState.Grappling)
        {
            GrapplingStateLogic();
        }
        else if (activeState == PlayerState.Dashing)
        {
            DashingStateLogic();
        }

        ceilingTime += Time.deltaTime;

        controller.Move(Velocity * Time.deltaTime);
    }

    private void LateUpdate()
    {
        playerInfo.playerVelocity = Velocity;
        playerInfo.playerPosition = transform.position;
    }

    void StandingStateLogic()
    {
        if (InputManager.instance.IsDash)
        {
            playerDash.CallDash();
            playerMove.footSteps.enabled = false;
        }
        if (InputManager.instance.IsGrapple)
        { 
            grappleHook.AttemptGrapple();
            playerMove.footSteps.enabled = false;
        }

        playerMove.Move();
        Gravity();
        Jump();
        OnCeilingCollision();
    }

    void GrapplingStateLogic()
    {
        grappleHook.Grappling();
        OnCeilingCollision();
    }

    void DashingStateLogic()
    { 
    
    }

    void Jump()
    {
        if (grounded && InputManager.instance.IsJump)
        {
            Velocity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
        }
    }

    void Gravity()
    {
        if (!grounded)
        {
            Velocity.y += gravity * Time.deltaTime;
        }
        else if (grounded && Velocity.y < 0f)
        {
            Velocity.y = -2f;
        }
    }

    void OnCeilingCollision()
    {
        if (ceilingHit)
        {
            Velocity.y = 0;
            ceilingTime = 0f;
            ceilingHit = false;
        }
    }
}
