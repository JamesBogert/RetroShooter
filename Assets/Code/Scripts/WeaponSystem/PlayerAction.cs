using UnityEngine;
using UnityEngine.InputSystem;


[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector gunSelector;

    private Controls input;

    private void Awake()
    {
        input = new Controls();
        input.Enable();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed && gunSelector.ActiveGun != null)
        {
            gunSelector.ActiveGun.Shoot();
        }
    }
}
