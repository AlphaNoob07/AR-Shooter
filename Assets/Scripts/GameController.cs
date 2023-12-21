using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    
    public enum GameState {
        idl,
        findSurface,
        placeMarker,
        GameStart,
        levelComplete,
        levelFail
    }
    [Header("Game State")]
    public GameState gameState = GameState.idl;

    [Header("Enemy Reference")]
    public AIGenarateLocation genarateLocation;
    public GameObject[] enemys;
    public int totalEnemy =7;
    private int currentEnemy =0;

    public int levelNumber = 1;
   [HideInInspector] public bool isSetup = false;

    [Header("Canvase Panel")]
    public GameObject mainuePanel;
    public WinPanel winPanel;
    /*public GameObject failPanel;*/

    [Header("Health Bar")]
    public HealthBar playerHanthBar;
    public HealthBar AIHealthBar;

    public int score =0;
    private void Awake()
    {
        instance = this;
        winPanel = FindAnyObjectByType<WinPanel>();
        winPanel.gameObject.SetActive(false);
        genarateLocation = FindFirstObjectByType<AIGenarateLocation>();
    }


    public void GenerateEnemy()
    { 
        
    }

    public void StartGame()
    {
        Debug.Log("StartGame");
        // will latter add ai pull sytem
        for (int i = 0; i < totalEnemy; i++)
        {
           
            int pickRandom = UnityEngine.Random.Range(0, enemys.Length);
            Debug.Log("StartGame " + pickRandom);
            GameObject enemy = Instantiate(enemys[pickRandom], genarateLocation.transform);
            
        }
    }

    private void LevelComplete()
    {

        winPanel.gameObject.SetActive(true);
    }

    public void LevelFail()
    {
        gameState = GameState.levelFail;
        winPanel.gameObject.SetActive(true);
    }


    public void RestartGame()
    {
        currentEnemy = 0;
    }

    public void UpgradInfo()
    {
        currentEnemy++;
        score++;
        if (currentEnemy >= 0 && gameState != GameController.GameState.levelFail)
        {
            gameState = GameState.levelComplete;
            LevelComplete();
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartGame();
        }
    }

}
