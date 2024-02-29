using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

//[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunSystem : MonoBehaviour
{
    //public ImpactType ImpactType;
    public GunType type;
    public string gunName;
    public int rigBuilderLayerNumber;
    //public GameObject modelPrefab;
    //public Vector3 spawnPoint;
    private Vector3 spawnRotation;
    [SerializeField] private Vector3 aimOffset;
    [SerializeField] private float penetratingThreshold = 1f;
    //[SerializeField] private float minAimDistance = 3.0f;
    [SerializeField] private AudioClip reloadSound;

    [SerializeField] private GameObject gunModel;

    [SerializeField] private bool shouldRecoil = true;

    [SerializeField] private AmmoConfigScriptableObject ammoConfig;
    [SerializeField] private ShootConfigScriptableObject shootConfig;
    [SerializeField] private TrailConfigScriptableObject trailConfig;
    [SerializeField] private DamageConfigScriptableObject damageConfig;
    private float lastShootTime;
    private float initialClickTime;
    private float stopShootingTime;
    private bool lastFrameWantedToShoot;

    private ParticleSystem shootSystem;
    private ObjectPool<TrailRenderer> trailPool;



    public void Awake()
    {
        var spawnRotationQuaternion = transform.rotation;
        spawnRotation = new Vector3(spawnRotationQuaternion.x, spawnRotationQuaternion.y, spawnRotationQuaternion.z);
        trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        shootSystem = GetComponentInChildren<ParticleSystem>();
        ammoConfig.currentClipAmmo = ammoConfig.clipSize;
        ammoConfig.currentAmmo = ammoConfig.maxAmmo;
    }



    public void Tick(bool wantsToShoot)
    {
        if (shouldRecoil)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(spawnRotation), Time.deltaTime * shootConfig.recoilRecoverySpeed);
        }

        if (wantsToShoot)
        {
            lastFrameWantedToShoot = true;
            if (ammoConfig.CanShoot())
            {
                Shoot();
            }
        }

        if (!wantsToShoot && lastFrameWantedToShoot)
        {
            stopShootingTime = Time.time;
            lastFrameWantedToShoot = false;
        }
        AimAtScreenCenter();
    }


    private void AimAtScreenCenter()
    {
        var playerCamera = Camera.main;
        var screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        var ray = playerCamera.ScreenPointToRay(screenCenter);

        Vector3 targetPoint;

        targetPoint = transform.position + playerCamera.transform.forward * 1000;
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    //if ((hit.point - ray.origin).magnitude < minAimDistance)
        //    //{
        //    //    targetPoint = transform.position + playerCamera.transform.forward * 1000; 
        //    //}
        //    //else
        //    {
        //        //targetPoint = hit.point;
        //    }
        //}
        //else
        //{
        //    targetPoint = ray.GetPoint(1000); 
        //}

        var direction = targetPoint - transform.position;
        var lookRotation = Quaternion.LookRotation(direction);
        var offsetRotation = Quaternion.Euler(aimOffset);
        lookRotation *= offsetRotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10); 
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
        SoundPool.SoundInstance.PlayVFXSound(shootConfig.shootSound, transform.position);
        lastShootTime = Time.time;
        shootSystem.Play();
        ammoConfig.currentClipAmmo--;
        for (int i = 0; i < shootConfig.countOfBullets; i++)
        {
            var spreadAmount = shootConfig.GetSpread(Time.time - initialClickTime);
            if (shouldRecoil)
            {
                transform.forward += transform.TransformDirection(spreadAmount);
            }
            var shootDirection = shootSystem.transform.forward + spreadAmount;
            ShootRaycast(shootDirection);
        }
    }


    private void ShootRaycast(Vector3 shootDirection)
    {
        var point = shootSystem.transform.position + shootDirection * trailConfig.missDistance;
        if (shootConfig.isPenetrating)
        {
            RaycastHit[] hits = Physics.RaycastAll(shootSystem.transform.position, shootDirection, float.MaxValue, shootConfig.hitMask);
            StartCoroutine(PlayPenetratingTrail(shootSystem.transform.position, point, hits));
            return;
        }
        if (Physics.Raycast(shootSystem.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, shootConfig.hitMask))
        {
            StartCoroutine(PlayTrail(shootSystem.transform.position, hit.point, hit));
        }
        else
        {
            StartCoroutine(PlayTrail(shootSystem.transform.position, point, new RaycastHit()));
        }
    }

    private IEnumerator PlayPenetratingTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit[] hits)
    {
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        TrailRenderer instance = trailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        yield return null;

        instance.emitting = true;
        var threshold = penetratingThreshold;
        var distance = Vector3.Distance(startPoint, endPoint);
        var remainingDistance = distance;
        var index = 0;
        while (remainingDistance > 0)
        {
            if (index < hits.Length && 
                Vector3.Distance(instance.transform.position, hits[index].transform.position) < threshold)
            {
                CheckHit(instance.transform.position, hits[index]);
                index++;
            }
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

    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        TrailRenderer instance = trailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        yield return null;

        instance.emitting = true;

        var distance = Vector3.Distance(startPoint, endPoint);
        var remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));
            remainingDistance -= trailConfig.simulationSpeed * Time.deltaTime;
            yield return null;
        }
        instance.transform.position = endPoint;
        CheckHit(endPoint, hit);    
        yield return new WaitForSeconds(trailConfig.duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        trailPool.Release(instance);
    }

    private void CheckHit(Vector3 endPoint, RaycastHit hit)
    {
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(damageConfig.GetDamage(), endPoint);
            }
            else
            {
                ParticlePool.Instance.HitParticlesPool.GetFromPool(endPoint);
            }
        }
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
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        return trail;
    }




    public bool AddAmmo(int amount, GunType ammoType)
    {
        if (ammoType != type)
        {
            return false;
        }
        return ammoConfig.AddAmmo(amount);
    }

    public void Reload()
    {
        ammoConfig.Reload();
    }

    public void PlayReloadSound() => SoundPool.SoundInstance.PlayVFXSound(reloadSound, transform.position);

    public bool CanReload() => ammoConfig.CanReload();

    public bool ShouldAutoReload() => ammoConfig.currentClipAmmo == 0;

    public float GetReloadTime() => ammoConfig.reloadTime;

    public int GetCurrentClipAmmo() => ammoConfig.currentClipAmmo;

    public int GetClipSize() => ammoConfig.clipSize;

    public string GetAmmoAmoubtInfoString() =>  $"{ammoConfig.currentClipAmmo}/{ammoConfig.currentAmmo}";

    public void SetActiveGunModel(bool active) => gunModel.SetActive(active);
}
