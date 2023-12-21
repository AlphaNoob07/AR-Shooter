using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyGunScripts))]
public class WeponController : MonoBehaviour
{
    [Header("Gun Scripts")]
    public EnemyGunScripts gunScripts;

    [Header("Bullet Force")]
    public float smoothForce;
    public float upwardForce;

    [Header("Gun Satas")]
    public float timeBtweenShooting, spread, reload, timeBetweenShots;
    public int magazineSize, bulletPreTap;
    public bool allowBtnHold;

    int bulletsLeft, bulletsShot;

    [Header("Gun Bools")]
    public bool shooting, readyToShoot, reloading;

    [Header("Reference")]
    public Camera ARMainCamera;
    public Transform attackPoint;


    [SerializeField] bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        gunScripts = GetComponent<EnemyGunScripts>();
    }



    private void Update()
    {
        
    }

    protected void MyInput()
    {
        // check is allowed to holdown btn

        if (allowBtnHold) shooting = Input.GetKeyDown(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Shooting 

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }



    protected virtual void Shoot()
    {
        readyToShoot = false;

        // find the extract hit point from camera hit center positon 

       // Ray

        bulletsLeft--;
        bulletsShot++;
    }
}
