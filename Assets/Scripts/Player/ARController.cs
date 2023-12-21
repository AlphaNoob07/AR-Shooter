using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TutorialsScripts))]
[RequireComponent(typeof(HealthScript))]
[RequireComponent(typeof(CapsuleCollider))]
public class ARController : MonoBehaviour
{
    [HideInInspector]
    public PlaceIndicator placeIndicator;
    [HideInInspector]
    public WeponController weponController;
    [HideInInspector]
    public TutorialsScripts tutorialsScripts;
    [HideInInspector]
    private Transform targetPoint;


    private void Awake()
    {
        tutorialsScripts = GetComponent<TutorialsScripts>();
        placeIndicator = FindObjectOfType<PlaceIndicator>();
        weponController = transform.GetChild(0).GetComponent<WeponController>();
        weponController.gameObject.SetActive(false);
        targetPoint = Camera.main.transform;
    }
    private void Update()
    {
        transform.position = targetPoint.position;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (GameController.instance.gameState == GameController.GameState.placeMarker && !weponController.gameObject.activeSelf)
                {
                    placeIndicator.enabled = false;
                    weponController.gameObject.SetActive(true);
                    GameController.instance.gameState = GameController.GameState.GameStart;
                    GameController.instance.StartGame();


                }
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                if (weponController.gameObject.activeInHierarchy)
                {
                    weponController.MyInput();
                }
                
            }

        }

       
    }






}
