using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutorialsScripts : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;


    private void Update()
    {

        if (!GameController.instance.isSetup)
        {
            HoldTutorials();
        }
    }
    public void HoldTutorials()
    {
        switch (GameController.instance.gameState)
        {
            case GameController.GameState.idl:
                debugText.text = "Point at floor to find surface";
                break;

            case GameController.GameState.findSurface:
                debugText.text = "Move and Tilt Device";
                break;
            case GameController.GameState.placeMarker:
                debugText.text = "Tap to place portal";
                break;
            case GameController.GameState.GameStart:
                debugText.text = "";
                GameController.instance.isSetup = true;
                break;
        }
    }
}
