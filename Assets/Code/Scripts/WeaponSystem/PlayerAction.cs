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
        if (InputManager.instance.IsFire1 && gunSelector.ActiveGun != null)
        {
            gunSelector.ActiveGun.Shoot();

            if (gunSelector.ActiveGun.type == GunType.Revolver && Time.timeScale > 0)
            {
                SoundFXManager.instance.PlaySFXClip("RevolverShoot", transform.position);
            }
        }
    }
}
