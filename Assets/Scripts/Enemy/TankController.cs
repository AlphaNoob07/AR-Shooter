using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyGunScripts))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(HealthScript))]
public class TankController : MonoBehaviour
{
    [Header("Tank RequireComponent Porperties")]
    private Rigidbody rb;
    private EnemyGunScripts enemyGun;
    private BoxCollider boxCollider;

    [Header("Turrent Properties")]
    [SerializeField] private Transform turrentTranform;
    [SerializeField] private Transform turrentTarget;

    [SerializeField] private float turrenLegSpeed =0.2f;

    private Vector3 finalTurrentLookDir;

    [Header("Tank Movement Properties")]
    [SerializeField] private Transform targetPose;
    [SerializeField] private float tankSpeed;
    [SerializeField] private float tankTrunSpeed;

    [SerializeField] private float maxDistance, minDistance;

    [Header("Tank Fire Properties")]
    [SerializeField] private float spread;
    [SerializeField] private float shootRange;
    [SerializeField] private float reloadTime, currentWaitTime;
    [SerializeField] private bool isShooting;

    public AIGenarateLocation genarateLocation; //  ARPortal Postion
    public ARPlaneManager arPlaneManager;
    private void Awake()
    {
        enemyGun = GetComponent<EnemyGunScripts>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        genarateLocation = FindObjectOfType<AIGenarateLocation>();
        targetPose.SetParent(null);

        MoveToNewRandomPosition();
    }

    private void Update()
    {
      
       
        HandleTankMovement();
        HandleTurrent();
        HandShoot();
    }


    protected virtual void HandShoot()
    {
        float shootDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
        isShooting = (shootDistance>3 && shootDistance < shootRange);


        if (currentWaitTime <=0 )
        {
            if (!isShooting) return;
            enemyGun.Shoot(Camera.main.transform.position, spread);
            currentWaitTime = reloadTime;

            shootRange = Random.Range(7, 20);
        }
        else
        {
            currentWaitTime -= Time.deltaTime;
        }

       
    }
    protected virtual void HandleTankMovement()
    {

        if (Vector3.Distance(transform.position, targetPose.position) >0.1f)
        {
            Vector3 wantedPositon = transform.position + (transform.forward * tankSpeed * Time.deltaTime);
            rb.MovePosition(wantedPositon);
            SmoothLookAtTarget();
        }
        else
        {
            MoveToNewRandomPosition();
        }



    }
    protected virtual void HandleTurrent()
    {
        if (turrentTranform)
        {
            Vector3 turrentLookDir = Camera.main.transform.position - turrentTranform.position;
            turrentLookDir.y = 0;

            finalTurrentLookDir = Vector3.Lerp(finalTurrentLookDir, turrentLookDir, Time.deltaTime * turrenLegSpeed);
            turrentTranform.rotation = Quaternion.LookRotation(finalTurrentLookDir);
        }
    }

    void MoveToNewRandomPosition()
    {
        targetPose.position = GetRandomPositionAroundTarget();
        targetPose.position = new Vector3(targetPose.position.x,0, targetPose.position.z);
    }
    // Upgrade Scripts
    Vector3 GetRandomPositionAroundTarget()
    {



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

    void SmoothLookAtTarget()
    {
        if (targetPose != null)
        {
            Vector3 targetDirection = targetPose.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
           transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, tankTrunSpeed * Time.deltaTime);
        }
    }


}
