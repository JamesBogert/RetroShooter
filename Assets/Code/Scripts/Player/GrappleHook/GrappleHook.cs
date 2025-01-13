using Unity.VisualScripting;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float grappleAcceleration = 10f;
    [SerializeField] private float grappleSpeed = 20f;
    [SerializeField] private float grappleMoveAcceleration = 5f;
    [SerializeField] private LayerMask hittableLayers;
    [SerializeField] private float maxGrappleDistance = 100f;
    [SerializeField] private float minGrappleDistance = 1f;

    private Vector3 grappleMoveVector;
    private Vector3 grappleHitPoint;
    [Space]
    [Header("References")]
    [SerializeField] private Transform grappleFirePoint;

    //References
    private PlayerController playerController;
    private LineRenderer lineRenderer;
    private Camera mainCam;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        mainCam = Camera.main;
    }

    private void Start()
    {
        lineRenderer.enabled = false;
    }


    public void AttemptGrapple()
    {
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, maxGrappleDistance, hittableLayers))
        {
            playerController.activeState = PlayerState.Grappling;
            grappleHitPoint = hit.point;

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, grappleFirePoint.position);
            lineRenderer.SetPosition(1, grappleHitPoint);
        }
    }

    public void Grappling()
    {
        if (InputManager.instance.GrappleHeld)
        {
            //accelerate at point of grapple
            Vector3 directionWithMagnitude = grappleHitPoint - playerController.transform.position;
            Vector3 targetVelocity = directionWithMagnitude.normalized * grappleSpeed;
            playerController.Velocity = Vector3.Lerp(playerController.Velocity, targetVelocity, grappleAcceleration * Time.deltaTime);

            //move with arrow keys while in grapple
            GrappleMove();
            playerController.Velocity += grappleMoveVector;

            //place lineRender where it needs to be next frame
            Vector3 nextFrameProjection = playerController.transform.position + (playerController.Velocity * Time.deltaTime);
            lineRenderer.SetPosition(0, grappleFirePoint.position + (playerController.Velocity * Time.deltaTime));
            lineRenderer.SetPosition(1, grappleHitPoint);

            if ((grappleHitPoint - mainCam.transform.position).magnitude < minGrappleDistance)
            {
                playerController.activeState = PlayerState.Standing;
                lineRenderer.enabled = false;
            }
        }
        else
        {
            playerController.activeState = PlayerState.Standing;
            lineRenderer.enabled = false;
        }
        
    }

    void GrappleMove()
    {
        Vector3 direction = playerController.transform.forward * 
            InputManager.instance.MoveInput.y + playerController.transform.right * InputManager.instance.MoveInput.x;

        Vector3 tangentDirection = Vector3.ProjectOnPlane(direction, (grappleHitPoint - playerController.transform.position).normalized);
        grappleMoveVector = tangentDirection * grappleMoveAcceleration * Time.deltaTime;
    }
}
