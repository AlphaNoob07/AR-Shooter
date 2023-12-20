using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunScripts : MonoBehaviour
{
    [SerializeField] private BulletScripts  bulletPrefab;
    [SerializeField] private float shootForce = 70.0f;
    [SerializeField] private int attackPointNumber = 0;
    [SerializeField] private Transform[] attackPoints;

    public void Shoot(Vector3 targetPoint,float speed)
    {
        // callculation direction attackPoints to targetPoint
        Vector3 directionWithOutSpeed = targetPoint - attackPoints[attackPointNumber].position;

        // Calculation spread
        float x = Random.Range(-speed, speed);
        float y = Random.Range(-speed, speed);

        Vector3 directionWithSpread = directionWithOutSpeed + new Vector3(x, y, 0); // just add speed

        BulletScripts currentBullet = Instantiate(bulletPrefab, attackPoints[attackPointNumber].position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        // Add  force to bullets
        currentBullet.rb.AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        attackPointNumber++;
        if (attackPointNumber >= attackPoints.Length)
        {
            attackPointNumber = 0;
        }
    }
}
