using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ARController : MonoBehaviour
{
    [SerializeField] private PlaceIndicator placeIndicator;
    [SerializeField] private WeponController weponController;

    [SerializeField] private Transform targetPoint;

    [SerializeField] private TextMeshProUGUI debugText;
    private void Awake()
    {
        placeIndicator = FindObjectOfType<PlaceIndicator>();
        weponController = transform.GetChild(0).GetComponent<WeponController>();
        weponController.gameObject.SetActive(false);
        targetPoint = Camera.main.transform;

        StartCoroutine(HoldTutorials());
    }

    private void Update()
    {
        transform.position = targetPoint.position;
      //  transform.position = Vector3.Lerp(transform.position, Camera.main, smooth * Time.deltaTime);
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (placeIndicator.enabled && placeIndicator.gameObject.activeInHierarchy)
                {
                    placeIndicator.enabled = false;
                    weponController.gameObject.SetActive(true);
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




    IEnumerator HoldTutorials()
    {
        while (!GameController.instance.isGameStart)
        {
           

            switch (GameController.instance.gameState)
            {
                case GameController.GameState.idl:
                    debugText.text = "Point at floor to find surface";
                    break;

                case GameController.GameState.findSurface:
                    debugText.text = "Move and Tilt Device";
                    break;
                case GameController.GameState.findMarker:
                    debugText.text = "Hold Device Closer to floor";
                    break;
                case GameController.GameState.placeMarker:
                    debugText.text = "Tap to place portal";
                    break;
            }

        }
        yield return null;
    }

}
