using System;
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
    public List<GameObject> enemyList = new List<GameObject>();
    public int enemyListNumber = 20;
    public int totalEnemy =7;
    public int currentEnemy =0;


   [HideInInspector] public bool isSetup = false;

    [Header("Canvase Panel")]
    public GameObject mainuePanel;
    public WinPanel winPanel;
    /*public GameObject failPanel;*/

    [Header("Health Bar")]
    public HealthBar playerHanthBar;
    public HealthBar AIHealthBar;

    public int levelNumber = 1;
    public int score =0;


    public event Action<int> OnResetLevelEvent;
    private void Awake()
    {
       
        instance = this;
        winPanel = FindAnyObjectByType<WinPanel>();
        winPanel.gameObject.SetActive(false);
        mainuePanel.SetActive(false);
        genarateLocation = FindFirstObjectByType<AIGenarateLocation>();
        GenerateEnemy();
    }


    public void GenerateEnemy()
    {
       for(int i =0; i< enemyListNumber;i++)
        {
            int randomPick = UnityEngine.Random.Range(0, enemys.Length);
            GameObject _enemy = Instantiate(enemys[randomPick], genarateLocation.transform);
            enemyList.Add(_enemy);
            _enemy.SetActive(false);


        }
    }



    private void ReshuffleEnemyList()
    {
        int n = enemyList.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            GameObject value = enemyList[k];
            enemyList[k] = enemyList[n];
            enemyList[n] = value;
        }
    }

    public void StartGame()
    {
        mainuePanel.SetActive(true);
        CancelInvoke("RestartGame");
        Debug.Log("StartGame");
        ReshuffleEnemyList(); // Before Active Suffle
        for (int i = 0; i < totalEnemy; i++)
        {
            enemyList[i].SetActive(true);
        }
       
    }

    private void LevelComplete()
    {
        levelNumber++;
        winPanel.gameObject.SetActive(true);
        OnResetLevelEvent?.Invoke(levelNumber);
    }

    public void LevelFail()
    {
        gameState = GameState.levelFail;
        winPanel.gameObject.SetActive(true);
    }


    public void RestartGame()
    {
        currentEnemy = 0;
        Invoke("StartGame", 3.0f);
        foreach (GameObject enemy in enemyList)
        {
            enemy.SetActive(false);
        }
    }

    public void UpgradInfo()
    {
    

        if (currentEnemy >= totalEnemy && gameState != GameController.GameState.levelFail)
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
