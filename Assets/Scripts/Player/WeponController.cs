using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyGunScripts))]
[RequireComponent(typeof(WeaponSway))]
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
      //  MyInput();
    }

    public void MyInput()
    {
        // check is allowed to holdown btn

/*        if (allowBtnHold) shooting = Input.GetKeyDown(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);*/

        // Shooting 

        if (readyToShoot)
        {
            bulletsShot = 0;
            Shoot();

           
        }
    }



    protected virtual void Shoot()
    {
        readyToShoot = false;

        // find the extract hit point from camera hit center positon 

        Ray ray = ARMainCamera.ViewportPointToRay(new Vector3(0.5f,0.5f, 0)); // 
        Debug.DrawRay(ray.origin, ray.direction * 10000, Color.red, 2.0f);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        gunScripts.Shoot(targetPoint, spread);

        attackPoint.position = targetPoint;


        if (allowInvoke)
        {
            Invoke("ResetShoot", timeBetweenShots);
            allowInvoke = false;
        }




        // calculation direction from attackpoint to targepoint;
        //Vector3 directionWithOutSpred = targetPoint - attackPoint.position;
     
        /*
                float x = Random.Range(-spread, spread);
                float y = Random.Range(-spread, spread);

                Vector3 directionWithspread = directionWithOutSpred = new Vector3(x, y, 0);*/
        //bulletsLeft--;
        // bulletsShot++;
    }

    private void ResetShoot()
    {
        CancelInvoke("ResetShoot");

        readyToShoot = true;
        allowInvoke = true;

        Debug.Log("Ready To Shoot");
    }
}
