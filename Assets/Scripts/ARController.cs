using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARController : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField] private GameObject portalPose;

    private void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        
        portalPose.SetActive(false); //  to set enemy 
    }
    // Update is called once per frame
    void Update()
    {
       
        var ray = new Vector2(Screen.width / 2, Screen.height / 2);

        if (raycastManager.Raycast(ray, hits, TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;

            portalPose.transform.position = hitPose.position;
            portalPose.transform.rotation = hitPose.rotation;

            if (!portalPose.activeInHierarchy)
            {
                portalPose.SetActive(true);
            }
        }
    }
}
