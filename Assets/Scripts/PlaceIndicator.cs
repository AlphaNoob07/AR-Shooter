using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(AIGenarateLocation))]
public class PlaceIndicator : MonoBehaviour
{

    [Header(" AR Porperties")]
    [HideInInspector] private ARRaycastManager raycastManager;
    [HideInInspector] private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [HideInInspector] public GameObject indicator;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        indicator = transform.GetChild(0).gameObject;
        indicator.SetActive(false);


       
    }

    void Update()
    {

            var ray = new Vector2(Screen.width / 2, Screen.height / 2);

            if (raycastManager.Raycast(ray, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                transform.position = hitPose.position;
                transform.rotation = hitPose.rotation;

                GameController.instance.gameState = GameController.GameState.findSurface;

                if (!indicator.activeInHierarchy)
                {
                    indicator.SetActive(true);
                }
                else
                {
                    GameController.instance.gameState = GameController.GameState.placeMarker;
                }
            }
        
       
    }
}
