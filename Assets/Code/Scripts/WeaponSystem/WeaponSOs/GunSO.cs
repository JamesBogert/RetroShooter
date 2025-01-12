using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu (fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunSO : ScriptableObject
{
    public GunType type;
    public string gunName;
    public GameObject modelPrefab;
    public Vector3 spawnPoint;
    public Vector3 spawnRotation;
    public string animatorFireName;

    public DamageConfigurationSO DamageConfig;
    public ShootConfigurationSO ShootConfig;
    public TrailConfigSO TrailConfig;

    private Animator animator;
    private MonoBehaviour activeMonobehavior;
    private GameObject model;
    private float lastShootTime;
    private ParticleSystem shootSystem;
    private ObjectPool<TrailRenderer> trailPool;

    public void Spawn(Transform parent, MonoBehaviour activeMonoBehavior)
    {

        this.activeMonobehavior = activeMonoBehavior;
        lastShootTime = 0f;
        trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        model = Instantiate(modelPrefab);
        model.transform.SetParent(parent, false);
        model.transform.localPosition = spawnPoint;
        model.transform.localRotation = Quaternion.Euler(spawnRotation);

        animator = model.GetComponentInChildren<Animator>();
        shootSystem = model.GetComponentInChildren<ParticleSystem>();
    }

    public void Shoot()
    {
        if (Time.time > ShootConfig.fireRate + lastShootTime)
        {
            lastShootTime = Time.time;
            shootSystem.Play();
            animator.Play(animatorFireName);

            Vector3 spread = ShootConfig.GetSpread();
            Vector3 shootDirection = model.transform.parent.forward + spread;
            shootDirection.Normalize();

            if (Physics.Raycast(model.transform.parent.position, shootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.hitMask))
            {
                activeMonobehavior.StartCoroutine(PlayTrail(shootSystem.transform.position, hit.point, hit));


            }
            else
            {
                 activeMonobehavior.StartCoroutine(PlayTrail(shootSystem.transform.position, shootSystem.transform.position + (shootDirection * TrailConfig.missDistance), hit));
            }
            
        }
    }

    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        TrailRenderer instance = trailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        yield return null;

        instance.emitting = true;
        float distance = Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        { 
            instance.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance/distance)));
            remainingDistance -= TrailConfig.simulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = endPoint;

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(DamageConfig.GetDamage(distance));
                damageable.Impact(hit.point, hit.normal);
            }
        }

        yield return new WaitForSeconds(TrailConfig.duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        trailPool.Release(instance);
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.color;
        trail.material = TrailConfig.material;
        trail.widthCurve = TrailConfig.widthCurve;
        trail.time = TrailConfig.duration;
        trail.minVertexDistance = TrailConfig.minVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }
}
