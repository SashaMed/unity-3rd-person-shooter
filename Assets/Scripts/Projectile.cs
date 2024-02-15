using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private GameObject decal;


    [SerializeField] private float speed = 50;
    [SerializeField] private float timeToLive = 20f;

    public Vector3 Target { get; set; }
    public bool Hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToLive);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target, speed * Time.deltaTime );
        if (!Hit && Vector3.Distance(Target, transform.position) < 0.01f )
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var contact = collision.GetContact(0);
        Instantiate(decal, contact.point + contact.normal * 0.001f, Quaternion.LookRotation(contact.normal));
        Destroy(gameObject);
    }


}
