using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Weapon Sway Setting")]
    [SerializeField] private float smooth;
    [SerializeField] private float multiplier;
    [SerializeField] private Transform target;

    private void Awake()
    {
        target = Camera.main.transform;
    }
    // Update is called once per frame
    void Update()
    {
       
        transform.rotation = Quaternion.Slerp(transform.localRotation, target.rotation, smooth * Time.deltaTime);
    }
}
