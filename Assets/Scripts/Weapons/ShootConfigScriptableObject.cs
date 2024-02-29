using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Configuration")]
public class ShootConfigScriptableObject : ScriptableObject
{
    public AudioClip shootSound;
    public LayerMask hitMask;
    public bool isPenetrating = false;
    public float fireRate = 0.25f;
    public float maxSpreadTime = 1.0f;
    public float recoilRecoverySpeed = 1.0f;
    public int countOfBullets = 1;
    public BulletSpreadType bulletSpreadType = BulletSpreadType.Simple;
    public Vector3 simpleSpread = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 minSpread = Vector3.zero;

    public Vector3 GetSpread(float shootTime = 0)
    {
        var spread = Vector3.zero;
        if (bulletSpreadType == BulletSpreadType.Simple)
        {
            spread = Vector3.Lerp(
                new Vector3(
                    Random.Range(-minSpread.x, minSpread.x),
                    Random.Range(-minSpread.y, minSpread.y),
                    Random.Range(-minSpread.z, minSpread.z)
                    ),
                new Vector3(
                    Random.Range(-simpleSpread.x, simpleSpread.x),
                    Random.Range(-simpleSpread.z, simpleSpread.z),
                    Random.Range(-simpleSpread.y, simpleSpread.y)),
                Mathf.Clamp01(shootTime / maxSpreadTime));
        }
        //spread.Normalize();
        return spread;
    }


}
