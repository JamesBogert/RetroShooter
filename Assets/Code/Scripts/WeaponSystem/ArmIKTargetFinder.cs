using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ArmIKTargetFinder : MonoBehaviour
{
    private TwoBoneIKConstraint iKConstraint;
    [SerializeField] private GameObject weaponHolder;
    private PlayerGunSelector gunSelector;

    private GameObject target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        iKConstraint = GetComponent<TwoBoneIKConstraint>();
        gunSelector = GetComponentInParent<PlayerGunSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindTarget();
        }
    }

    void FindTarget()
    {
        if (gunSelector.ActiveGun != null)
        {
            target = weaponHolder.GetComponentInChildren<IKTarget>().gameObject;
            if (target != null)
            {
                iKConstraint.data.target = target.transform;
            }
        }
    }
}
