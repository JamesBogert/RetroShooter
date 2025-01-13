using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMove : MonoBehaviour
{
    [Header("BasicMove")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float inAirAcceleration = 30f;

    public AudioSource footSteps;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void Move()
    {
        Vector3 direction = transform.forward * InputManager.instance.MoveInput.y + transform.right * InputManager.instance.MoveInput.x;
        Vector3 target = direction * moveSpeed;
        if (playerController.grounded)
        {
            Vector3 velocityXZ = new Vector3(playerController.Velocity.x, 0f, playerController.Velocity.z);
            velocityXZ = Vector3.Lerp(velocityXZ, target, acceleration * Time.deltaTime);
            playerController.Velocity = new Vector3(velocityXZ.x, playerController.Velocity.y, velocityXZ.z);

            if (direction.magnitude > 0)
            {
                footSteps.enabled = true;
            } else
            {
                footSteps.enabled = false;
            }
        }
        else
        {
            if (direction != Vector3.zero)
            {
                Vector3 velocityXZ = new Vector3(playerController.Velocity.x, 0f, playerController.Velocity.z);
                velocityXZ = Vector3.Lerp(velocityXZ, target, inAirAcceleration * Time.deltaTime);
                playerController.Velocity = new Vector3(velocityXZ.x, playerController.Velocity.y, velocityXZ.z);
            }
            footSteps.enabled = false;
        }
    }
}
