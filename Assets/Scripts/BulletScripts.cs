using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScripts : MonoBehaviour
{
    public Rigidbody rb;
    public float lifeTime =3;
    public float damage =10;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
              collision.gameObject.GetComponent<HealthScript>().GetDamage(damage);
            Destroy(this.gameObject, 0);
        }
        Destroy(this.gameObject, 0);
    }

}
