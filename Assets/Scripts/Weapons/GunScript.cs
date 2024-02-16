using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

//[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunScript : MonoBehaviour
{
    //public ImpactType ImpactType;
    public GunType type;
    public string gunName;
    //public GameObject modelPrefab;
    //public Vector3 spawnPoint;
    private Vector3 spawnRotation;


    public ShootConfigScriptableObject shootConfig;
    public TrailConfigScriptableObject trailConfig;
    private float lastShootTime;
    private float initialClickTime;
    private float stopShootingTime;
    private bool lastFrameWantedToShoot;

    private ParticleSystem shootSystem;
    private ObjectPool<TrailRenderer> trailPool;



    public void Start()
    {
        //activeMonoBehaviour = ActiveMonoBehaviour;

        //model = Instantiate(modelPrefab);
        //model.transform.SetParent(Parent, false);
        //model.transform.localPosition = spawnPoint;
        //model.transform.localRotation = Quaternion.Euler(spawnRotation);
        var spawnRotationQuaternion = transform.rotation;
        spawnRotation = new Vector3(spawnRotationQuaternion.x, spawnRotationQuaternion.y, spawnRotationQuaternion.z);
        trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        shootSystem = GetComponentInChildren<ParticleSystem>();
    }



    public void Tick(bool wantsToShoot)
    {
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            Quaternion.Euler(spawnRotation),
            Time.deltaTime * shootConfig.recoilRecoverySpeed
        );

        if (wantsToShoot)
        {
            lastFrameWantedToShoot = true;
            Shoot();
        }

        if (!wantsToShoot && lastFrameWantedToShoot)
        {
            stopShootingTime = Time.time;
            lastFrameWantedToShoot = false;
        }
    }


    public void Shoot()
    {
        if (Time.time - lastShootTime - shootConfig.fireRate > Time.deltaTime)
        {
            float lastDuration = Mathf.Clamp( 0, (stopShootingTime - initialClickTime),shootConfig.maxSpreadTime);
            float lerpTime = (shootConfig.recoilRecoverySpeed - (Time.time - stopShootingTime))/shootConfig.recoilRecoverySpeed;
            initialClickTime = Time.time - Mathf.Lerp(0, lastDuration, Mathf.Clamp01(lerpTime));
        }


        if (Time.time < lastShootTime + shootConfig.fireRate)
        {
            return;
        }
        lastShootTime = Time.time;
        //shootSystem.Play();
        var spreadAmount = shootConfig.GetSpread(Time.time - initialClickTime);
        transform.forward += transform.TransformDirection(spreadAmount);
        var shootDirection = shootSystem.transform.forward + spreadAmount;
        if (Physics.Raycast(shootSystem.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, shootConfig.hitMask))
        {
            StartCoroutine(PlayTrail(shootSystem.transform.position, hit.point, hit));
        }
        else
        {
            var point = shootSystem.transform.position + shootDirection * trailConfig.missDistance;
            StartCoroutine(PlayTrail(shootSystem.transform.position, point, new RaycastHit()));
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
            instance.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));
            remainingDistance -= trailConfig.simulationSpeed * Time.deltaTime;
            yield return null;
        }
        instance.transform.position = endPoint;
        yield return new WaitForSeconds(trailConfig.duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        trailPool.Release(instance);
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = trailConfig.color;
        trail.material = trailConfig.material;
        trail.widthCurve = trailConfig.widthCurve;
        trail.time = trailConfig.duration;
        trail.minVertexDistance = trailConfig.minVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

}
