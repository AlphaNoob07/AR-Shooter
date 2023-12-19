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
    public bool isShooting = false;
    public bool isLockingTarget = false;
    private Vector3 randomPosition;
    private void Awake()
    {
        targetPose.transform.SetParent(null);

        StartCoroutine(FlyRoutine());
    }
    void Update()
    {
       /* // Check if it's time to move to a new random position
        if (currentWaitTime <= 0f)
        {
            isShooting = false;
            MoveToNewRandomPosition();
            currentWaitTime = waitTime;
        }
        else
        {
            currentWaitTime -= Time.deltaTime;
        }



      *//*  float distanceToTarget = Vector3.Distance(transform.position, targetPlayer.position);
        if (isShooting && distanceToTarget >minDistance)
        {
            targetPose.position = targetPlayer.position;
        }
      
        if(!isShooting)*//*
        SmoothLookAtTarget();*/

    }




    IEnumerator FlyRoutine()
    {
        while (true)
        {
            // Check if it's time to move to a new random position
            if (currentWaitTime <= 0f && !isLockingTarget)
            {
                MoveToNewRandomPosition();
                currentWaitTime = waitTime;
            }
            else
            {
                currentWaitTime -= Time.deltaTime;
            }

            // Move towards the random position
            yield return StartCoroutine(MoveToPosition(targetPose.position));

            // Check if the target player is within the shoot range
            float distanceToTarget = Vector3.Distance(transform.position, targetPlayer.position);
            if (distanceToTarget <= shootRange && !isShooting && !isLockingTarget)
            {
                isShooting = true;
                PowerShoot();
                StartCoroutine(LockTargetForDuration(30f)); // Lock target for 30 seconds
            }
            else if (distanceToTarget > shootRange)
            {
                isShooting = false;
            }

            // Check if the helicopter is attacked (you can replace this with your actual attack detection logic)
            if (IsAttacked())
            {
                MoveAway();
            }

            // Wait for the next iteration
            yield return null;
        }
    }

    IEnumerator MoveToPosition(Vector3 position)
    {
        while (Vector3.Distance(transform.position, position) > 0.1f)
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
          //  transform.position = Vector3.Lerp(transform.position, targetPose.position, moveSpeed * Time.deltaTime);
            Vector3 targetDirection = targetPose.position - transform.position;

            // Calculate the rotation to face the target
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
        // Implement power shoot logic here
        Debug.Log("Power shoot at the target!");
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
