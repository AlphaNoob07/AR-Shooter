using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScripts : MonoBehaviour
{
    private enum EnemyMode
    { 
        Tank,
        Helicopter
    
    }
    [SerializeField] private EnemyMode enemyMode = EnemyMode.Helicopter;

    private void Awake()
    {
     
    }
}
