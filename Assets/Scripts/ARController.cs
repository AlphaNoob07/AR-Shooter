using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARController : MonoBehaviour
{
    [SerializeField] private PlaceIndicator placeIndicator;


    private void Awake()
    {
        placeIndicator = FindObjectOfType<PlaceIndicator>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (placeIndicator.enabled)
                {
                    placeIndicator.enabled = false;
                }
            }

        }
    }
}
