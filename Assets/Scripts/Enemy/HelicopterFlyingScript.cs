using System.Collections;
using UnityEngine;

public class HelicopterFlyingScript : MonoBehaviour
{
    public Transform targetPlayer;
    public Transform targetPose;
    public float flyingHeight =30f;
    public float minDistance = 10f;
    public float maxDistance = 20f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float waitTime = 5f; // Time to wait before moving to a new random position
    public float currentWaitTime = 0f;
    public float shootRange = 15f; // Range to shoot at the target player
    public float fireRate = 0.3f;
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

        //  check shooting range and angle
        if (distanceToTarget >= 10 && distanceToTarget <= 25 && !isShooting && angleToPlayer <45)
        {
            isShooting = true;
            targetPose.position = targetPlayer.position;
            float randomLockTime = UnityEngine.Random.Range(3.0f, 7.0f);
            StartCoroutine(LockTargetForDuration(randomLockTime)); // Lock target for randomLockTime seconds
        }
        else
        {
            if(isShooting || isLockingTarget)
            isShooting = false;
            //MoveToNewRandomPosition();
        }


        if (!isShooting) return;
        SmoothLookAtTarget();
        Invoke("PowerShoot", fireRate);
        

    }




  IEnumerator FlyRoutine()
    {
        while (true)
        {
            if (currentWaitTime <= 0f && !isLockingTarget)
            {
                
                currentWaitTime = waitTime;
            }
            else
            {
                currentWaitTime -= Time.deltaTime;
                currentWaitTime = Mathf.Clamp(currentWaitTime, 0, currentWaitTime);
            }

                 MoveToNewRandomPosition();

            yield return StartCoroutine(MoveToPosition(targetPose.position));
      
            if (IsAttacked())
            {
               // shootRange = false;
                MoveAway();
            }
            yield return null;
        }
    }

    IEnumerator MoveToPosition(Vector3 position)
    {
        Vector3  adjustHeight =  new Vector3(position.x,flyingHeight,position.z);
        while (Vector3.Distance(transform.position, adjustHeight) >= (minDistance -3) )
        {
            transform.position = Vector3.MoveTowards(transform.position, adjustHeight, moveSpeed * Time.deltaTime);
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
        flyingHeight = Random.Range(3,7);
        float distance = Random.Range(minDistance, maxDistance);
        float angle = Random.Range(0f, 360f);
        Vector3 randomDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        return targetPlayer.position + randomDirection * distance;
    }

    void PowerShoot()
    {
        CancelInvoke("PowerShoot");
        _enemyGun.Shoot(targetPlayer.position, fireRate);
    }

    bool IsAttacked()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    void MoveAway()
    {
        Vector3 awayDirection = transform.position - targetPlayer.position;
        Vector3 awayPosition = transform.position + awayDirection.normalized * minDistance;

    }
}
