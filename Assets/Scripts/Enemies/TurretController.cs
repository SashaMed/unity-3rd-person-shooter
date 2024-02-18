using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    [SerializeField] private GameObject turretHead;
    [SerializeField] private float rotationSpeed = 20;
    [SerializeField] private float playerDetectedRotationSpeed = 40;
    [SerializeField] private float waitForPlayerTime = 2f;
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private GunSystem gun;

    private bool canRotate = true;
    private bool playerWasRecentlyDetected;
    private Vector3 lastPlayerPosition = Vector3.zero;
    private TurretDetection detection;
    private EnemyHealth healthSystem;

    // Start is called before the first frame update
    void Start()
    {
        healthSystem= GetComponent<EnemyHealth>();
        //gun = GetComponentInChildren<GunSystem>();
        detection = GetComponent<TurretDetection>();
        healthSystem.OnDeath += DeathHandler;
        healthSystem.OnTakeDamage += DamageHandler;
    }

    // Update is called once per frame
    void Update()
    {
        if (detection.PlayerDetected)
        {
            RotateToPlayer();
            if (!playerWasRecentlyDetected)
            {
                StopAllCoroutines();
                playerWasRecentlyDetected = true;
            }
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
        //Debug.Log("end of WaitForPlayerApearsAgainCoroutine");
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

    private void DeathHandler(Vector3 pos)
    {
        var turretHeadRB = turretHead.GetComponent<Rigidbody>();
        turretHeadRB.isKinematic = false;
        var direction = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        turretHeadRB.AddForce((direction + Vector3.up) * explosionForce, ForceMode.Impulse); 
        gameObject.SetActive(false);
    }



    private void DamageHandler(float damage,Vector3 pos)
    {
        ParticlePool.Instance.DamageablesHitParticlesPool.GetFromPool(pos);
    }

}
