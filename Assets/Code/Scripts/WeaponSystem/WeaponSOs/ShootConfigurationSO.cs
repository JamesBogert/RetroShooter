using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/ShootConfiguration")]
public class ShootConfigurationSO : ScriptableObject
{
    public LayerMask hitMask;
    public float fireRate = 0.7f;
    public int bulletsPerShot = 1;
    public bool automatic = false;
    public Vector3 spread = new Vector3(0.1f, 0.1f, 0.1f);

    public Vector3 GetSpread()
    {
        Vector3 spreadAmount = new Vector3(Random.Range(-spread.x, spread.x), Random.Range(-spread.y, spread.y), Random.Range(-spread.z, spread.z));
        return spreadAmount;
    }
}
