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
        /* switch (enemyMode)
         {
             case EnemyMode.Helicopter:
                 this.gameObject.AddComponent<HelicopterFlyingScript>();
                 break;
             case EnemyMode.Tank:
                // this.gameObject.AddComponent<HelicopterFlyingScript>();
                 break;

         }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
