using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu (fileName = "InputManagerSO",menuName = "Input Manager/Input Manager")]
public class InputManager : ScriptableObject
{
    public static InputManager instance;

    public Vector2 MoveInput;
    public bool IsJump;
    public bool JumpHeld;

    public bool IsDash;
    public bool IsGrapple;
    public bool GrappleHeld;

    public bool IsSlide;
    public bool SlideHeld;

    public bool IsInteract;
    public bool InteractHeld;

    public bool IsFire1;
    public bool Fire1Held;
    public bool IsFire2;
    public bool Fire2Held;

    public float ScrollWheelInput;

    public static Controls input;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }

        input = new Controls();
        input.Enable();
    }

    public void UpdateInput()
    { 
        MoveInput =  input.Player.Move.ReadValue<Vector2>();
        IsJump = input.Player.Jump.WasPressedThisFrame();
        JumpHeld = input.Player.Jump.IsPressed();

        IsDash = input.Player.Dash.WasPressedThisFrame();
        IsGrapple = input.Player.Grapple.WasPressedThisFrame();
        GrappleHeld = input.Player.Grapple.IsPressed();

        IsSlide = input.Player.Slide.WasPressedThisFrame();
        SlideHeld = input.Player.Slide.IsPressed();

        IsInteract = input.Player.Interact.WasPressedThisFrame();
        InteractHeld = input.Player.Interact.IsPressed();

        IsFire1 = input.Player.Fire1.WasPressedThisFrame();
        Fire1Held = input.Player.Fire1.IsPressed();
        IsFire2 = input.Player.Fire2.WasPressedThisFrame();
        Fire2Held = input.Player.Fire2.IsPressed();

        ScrollWheelInput = input.Player.ScrollWheel.ReadValue<float>();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
