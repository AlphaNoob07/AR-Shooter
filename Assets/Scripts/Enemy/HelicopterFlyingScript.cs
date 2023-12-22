using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(EnemyGunScripts))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(HealthScript))]
public class HelicopterFlyingScript : MonoBehaviour
{
    [Header("Helicopter Requirment Porperties")]
    public Transform targetPlayer;
    public Transform targetPose;
    public float flyingHeight = 30f;
    public float minDistance = 10f;
    public float maxDistance = 20f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float waitTime = 5f; // Time to wait before moving to a new random position
    public float currentWaitTime = 0f;

    [Header("Helicopter Fire Porperties")]
    public float shootRange = 15f; // Range to shoot at the target player
    public float fireRate = 0.3f;
    public float distanceToTarget;
    public bool isShooting = false;
    public bool isLockingTarget = false;
    private Vector3 randomPosition;

    public AIGenarateLocation genarateLocation; //  ARPortal Postion
    public ARPlaneManager arPlaneManager;
    private EnemyGunScripts _enemyGun;
    private void Awake()
    {
        targetPlayer = Camera.main.transform;

        targetPose.transform.SetParent(null);

        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        genarateLocation = FindObjectOfType<AIGenarateLocation>();

        if (_enemyGun == null)
            _enemyGun = GetComponent<EnemyGunScripts>();
    }

    private void Start()
    {
        StartCoroutine(FlyRoutine());
    }
    void Update()
    {

        distanceToTarget = Vector3.Distance(transform.position, targetPlayer.position);

        // Call cullation angle
        Vector3 directionToPlayer = targetPlayer.position - transform.position;
        directionToPlayer.Normalize();
        SmoothLookAtTarget();
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        isShooting = (distanceToTarget >= 10 && distanceToTarget <= 20 && !isShooting && angleToPlayer < 45);
        //  check shooting range and angle
        if (isShooting)
        {
            isShooting = true;
            targetPose.position = targetPlayer.position;
            float randomLockTime = UnityEngine.Random.Range(3.0f, 7.0f);
            StartCoroutine(LockTargetForDuration(randomLockTime)); // Lock target for randomLockTime seconds
        }
        else
        {
            if (isShooting || isLockingTarget)
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
        Vector3 adjustHeight = new Vector3(position.x, flyingHeight, position.z);
        while (Vector3.Distance(transform.position, adjustHeight) >= (minDistance - 3))
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
    /*
        Vector3 GetRandomPositionAroundTarget()
        {
            flyingHeight = Random.Range(1, 3);
            float distance = Random.Range(-maxDistance, maxDistance);
            float angle = Random.Range(0f, 360f);
            Vector3 randomDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            return targetPlayer.position + randomDirection * distance;
        }*/


    // Upgrade Scripts
    Vector3 GetRandomPositionAroundTarget()
    {

        /* float distance = Random.Range(-maxDistance, maxDistance);
         float angle = Random.Range(0f, 360f);
         Vector3 randomDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
         return genarateLocation.transform.position + randomDirection * distance;*/
        flyingHeight = Random.Range(2.5f, 5.0f);
        float distance = Random.Range(15, 20);
        float angle = Random.Range(0f, 360f);
        Vector3 randomDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;

        Vector3 floorPosition = GetDetectedFloorPosition();
        if (floorPosition == Vector3.zero)
        {
            //Debug.LogError("No tracked planes found.");
            return genarateLocation.transform.position + randomDirection * distance;
        }


        distance = Random.Range(15, 20);
        angle = Random.Range(0f, 360f);
        randomDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;

        // Offset the position from the AR plane
        Vector3 randomPosition = floorPosition + randomDirection * distance;

        return randomPosition;

    }

    private Vector3 GetDetectedFloorPosition()
    {
        //  the first one as the floor
        foreach (var plane in arPlaneManager.trackables)
        {
            if (plane.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                return plane.transform.position;
            }
        }

        //  fallback
        return Vector3.zero;
    }


    void PowerShoot()
    {
        fireRate = Random.Range(0.7f, 3.0f);
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
