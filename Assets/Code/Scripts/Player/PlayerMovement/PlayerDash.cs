using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;

    //references
    private PlayerController playerController;
    private CharacterController controller;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();
    }

    public void CallDash()
    {
        playerController.activeState = PlayerState.Dashing;
        StartCoroutine(Dash());

        SoundFXManager.instance.PlaySFXClip("DashWoosh", transform.position);
    }

    private IEnumerator Dash()
    {
        float time = 0f;

        Vector3 initVelocity = playerController.Velocity;

        Vector3 moveDirection = transform.forward * InputManager.instance.MoveInput.y + transform.right * InputManager.instance.MoveInput.x;
        if (moveDirection == Vector3.zero) moveDirection = transform.forward;
        while (time < dashTime)
        {
            controller.Move(moveDirection.normalized * dashSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        playerController.Velocity = moveDirection.normalized * initVelocity.magnitude;
        playerController.activeState = PlayerState.Standing;
    }
}
