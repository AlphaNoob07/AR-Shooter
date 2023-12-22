using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScripts : MonoBehaviour
{
    private enum BulletState { 
        player,
        enemy
    }
    [SerializeField] private BulletState bulletState = BulletState.player;
    public Rigidbody rb;
    public float lifeTime =3;
    public float damage =10;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && bulletState == BulletState.enemy || other.gameObject.CompareTag("Enemy") && bulletState == BulletState.player)
        {
            other.gameObject.GetComponent<HealthScript>().GetDamage(10);
        }
        Destroy(this.gameObject, 0);
    }
   

}
