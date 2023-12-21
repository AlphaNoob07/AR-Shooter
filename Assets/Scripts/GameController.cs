using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState { 
        idl,
        findSurface,
        findMarker,
        placeMarker,
        GameStart,
        levelComplete,
        levelFail
    }

    public GameState gameState = GameState.idl;
    public bool isGameStart;

    public static GameController instance;

    public int levelNumber = 1;

    private void Awake()
    {
        instance = this;
    }


    public void GenerateEnemy()
    { 
        
    }

    private void StartGame()
    { 
        
    }

    private void LevelComplete()
    { 
        
    }

    private void LevelFail()
    {

    }


    private void ReStartGame()
    { 
        
    }

    public void UpgradInfo()
    { 
        
    }




}
