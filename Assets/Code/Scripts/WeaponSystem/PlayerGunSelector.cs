using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField] private GunType Gun;
    [SerializeField] private Transform gunParent;
    [SerializeField] private List<GunSO> guns;

    [Space]
    [Header("RunTime Filled")]
    public GunSO ActiveGun;

    private void Awake()
    {
        GunSO _gun = guns.Find(_gun => _gun.type == Gun);

        if (_gun == null)
        {
            Debug.LogError($"No GunSO found for guntype: {_gun}");
            return;
        }

        ActiveGun = _gun;
        _gun.Spawn(gunParent, this);
    }
}
