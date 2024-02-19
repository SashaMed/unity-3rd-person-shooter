using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TurretController : MonoBehaviour
{
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private GameObject turretHead;
    [SerializeField] private float rotationSpeed = 20;
    [SerializeField] private float playerDetectedRotationSpeed = 40;
    [SerializeField] private float waitForPlayerTime = 2f;
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private GunSystem gun;
    [SerializeField] private AudioClip destroySound;

    private bool canRotate = true;
    private bool wasHitedRecently;
    private bool playerWasRecentlyDetected;
    private Vector3 hitPosition;
    private TurretDetection detection;
    private EnemyHealth healthSystem;


    void Start()
    {
        healthSystem= GetComponent<EnemyHealth>();
        detection = GetComponent<TurretDetection>();
        healthSystem.OnDeath += DeathHandler;
        healthSystem.OnTakeDamage += DamageHandler;
    }


    void Update()
    {
        if (detection.PlayerDetected)
        {
            RotateToPlayer();
            if (!playerWasRecentlyDetected)
            {
                StopAllCoroutines();
                wasHitedRecently = false;
                playerWasRecentlyDetected = true;
            }
        }
        else if (wasHitedRecently)
        {
            RotateToHit();
        }
        else
        {
            if (playerWasRecentlyDetected)
            {
                playerWasRecentlyDetected = false;
                StartCoroutine(WaitForPlayerApearsAgainCoroutine());    
            }

            RotateTurret();
        }
    }

    private IEnumerator WaitForPlayerApearsAgainCoroutine()
    {
        canRotate = false;
        yield return new WaitForSeconds(waitForPlayerTime);  
        var targetRotation = Quaternion.Euler(0, turretHead.transform.rotation.y, 0); 

        while (turretHead.transform.rotation != targetRotation)
        {
            turretHead.transform.rotation = Quaternion.RotateTowards(turretHead.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        canRotate = true;
    }

    private void RotateTurret()
    {
        if (canRotate)
        turretHead.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }


    private void RotateToPlayer()
    {
        var directionToPlayer = detection.PlayerPosition - turretHead.transform.position;
        var lookRotation = Quaternion.LookRotation(directionToPlayer);
        turretHead.transform.rotation = Quaternion.Lerp(turretHead.transform.rotation, lookRotation, Time.deltaTime * playerDetectedRotationSpeed);
        var angleToPlayer = Vector3.Angle(turretHead.transform.forward, directionToPlayer);
        if (angleToPlayer < 5f)
        {
            gun.Shoot();
        }
    }


    private void RotateToHit()
    {
        var directionToPlayer = hitPosition - turretHead.transform.position;
        directionToPlayer.y = 0;
        var lookRotation = Quaternion.LookRotation(directionToPlayer);
        turretHead.transform.rotation = Quaternion.Lerp(turretHead.transform.rotation, lookRotation, Time.deltaTime * playerDetectedRotationSpeed);
    }

    private void DeathHandler(Vector3 pos)
    {
        var turretHeadRB = turretHead.GetComponent<Rigidbody>();
        turretHeadRB.isKinematic = false;
        var direction = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        turretHeadRB.AddForce((direction + Vector3.up) * explosionForce, ForceMode.Impulse);
        explosionParticle.SetActive(true);
        SoundPool.SoundInstance.PlayVFXSound(destroySound, pos);
        gameObject.SetActive(false);
    }



    private void DamageHandler(float damage,Vector3 pos)
    {
        ParticlePool.Instance.DamageablesHitParticlesPool.GetFromPool(pos);
        if (!wasHitedRecently && !playerWasRecentlyDetected)
        {
            wasHitedRecently = true;
            hitPosition= pos;
            StartCoroutine(ResetRecentlyHitCoroutine());
        }
    }

    private IEnumerator ResetRecentlyHitCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        wasHitedRecently = false;
        hitPosition = Vector3.zero;
    }

}
