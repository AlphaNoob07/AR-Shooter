using System.Collections;
using UnityEngine;

public class HelicopterFlyingScript : MonoBehaviour
{
    public Transform targetPlayer;
    public Transform targetPose;
    public float minDistance = 10f;
    public float maxDistance = 20f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float waitTime = 5f; // Time to wait before moving to a new random position
    public float currentWaitTime = 0f;
    public float shootRange = 15f; // Range to shoot at the target player
    public float distanceToTarget;
    public bool isShooting = false;
    public bool isLockingTarget = false;
    private Vector3 randomPosition;


     private EnemyGunScripts _enemyGun;
    private void Awake()
    {
        targetPose.transform.SetParent(null);
        StartCoroutine(FlyRoutine());

        if (_enemyGun == null)
            _enemyGun = GetComponent<EnemyGunScripts>();
    }
    void Update()
    {

        distanceToTarget = Vector3.Distance(transform.position, targetPlayer.position);

        // Call cullation angle
        Vector3 directionToPlayer = targetPlayer.position - transform.position;
        directionToPlayer.Normalize();
        SmoothLookAtTarget();
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);


        if (distanceToTarget >= 15 && distanceToTarget <= 30 && !isShooting && angleToPlayer <45)
        {
            isShooting = true;
            targetPose.position = targetPlayer.position;
            float randomLockTime = UnityEngine.Random.Range(7.0f, 15.0f);
            StartCoroutine(LockTargetForDuration(randomLockTime)); // Lock target for randomLockTime seconds
        }
        else
        {
            isShooting = false;
            MoveAway();
        }


        if (!isShooting) return;

  
        SmoothLookAtTarget();
        if (angleToPlayer < 45.0f)
        {
            Invoke("PowerShoot", waitTime);
        }

    }




  IEnumerator FlyRoutine()
    {
        while (true)
        {
            if (currentWaitTime <= 0f && !isLockingTarget)
            {
                MoveToNewRandomPosition();
                currentWaitTime = waitTime;
            }
            else
            {
              
                    currentWaitTime -= Time.deltaTime;
                
               
            }
            yield return StartCoroutine(MoveToPosition(targetPose.position));
           
           
            /*if (distanceToTarget > shootRange)
            {
                isShooting = false;
            }*/
            if (IsAttacked())
            {
                MoveAway();
            }
            yield return null;
        }
    }

    IEnumerator MoveToPosition(Vector3 position)
    {
        while (Vector3.Distance(transform.position, position) > 15 )
        {
            // Move towards the position
            transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
            SmoothLookAtTarget();
            yield return null;
        }
    }

    IEnumerator LockTargetForDuration(float duration)
    {
        isLockingTarget = true;
        yield return new WaitForSeconds(duration);
        isLockingTarget = false;
    }


    void SmoothLookAtTarget()
    {
        if (targetPose != null)
        {
            Vector3 targetDirection = targetPose.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void MoveToNewRandomPosition()
    {
        targetPose.position = GetRandomPositionAroundTarget();
        MoveToPosition(targetPose.position);
    }

    Vector3 GetRandomPositionAroundTarget()
    {
        float distance = Random.Range(minDistance, maxDistance);
        float angle = Random.Range(0f, 360f);
        Vector3 randomDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        return targetPlayer.position + randomDirection * distance;
    }

 /*   void MoveToPosition(Vector3 position)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
    }*/


    void PowerShoot()
    {
        CancelInvoke("PowerShoot");
        Debug.Log("Shooot ");
        _enemyGun.Shoot(targetPose.position,1.0f);
    }

    bool IsAttacked()
    {
        // Implement attack detection logic here
        // For example, you can use Input.GetKeyDown or other methods to detect attacks
        return Input.GetKeyDown(KeyCode.Space);
    }

    void MoveAway()
    {
        // Move away from the target player when attacked
        Vector3 awayDirection = transform.position - targetPlayer.position;
        Vector3 awayPosition = transform.position + awayDirection.normalized * minDistance;
       // MoveToPosition(awayPosition);
    }
}
