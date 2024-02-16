using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Configuration")]
public class ShootConfigScriptableObject : ScriptableObject
{

    public LayerMask hitMask;
    public float fireRate = 0.25f;
    public float maxSpreadTime = 1.0f;
    public float recoilRecoverySpeed = 1.0f;
    public BulletSpreadType bulletSpreadType = BulletSpreadType.Simple;
    public Vector3 simpleSpread = new Vector3(0.1f, 0.1f, 0.1f);

    public Vector3 GetSpread(float shootTime = 0)
    {
        var spread = Vector3.zero;
        if (bulletSpreadType == BulletSpreadType.Simple)
        {
            spread = Vector3.Lerp(Vector3.zero, new Vector3(
                UnityEngine.Random.Range(-simpleSpread.x, simpleSpread.x),
                UnityEngine.Random.Range(-simpleSpread.z, simpleSpread.z),
                UnityEngine.Random.Range(-simpleSpread.y, simpleSpread.y)),
                Mathf.Clamp01(shootTime / maxSpreadTime));
        }
        //spread.Normalize();
        return spread;
    }


}
