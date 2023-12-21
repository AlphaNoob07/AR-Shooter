using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    private float maxHealth, minHealth, currentHealth;
    public float damageReductionSpeed = 10.0f;
    private HealthBar healthBar;
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            healthBar = GameController.instance.playerHanthBar;
        }
        else
        {
            healthBar = GameController.instance.AIHealthBar;
            healthBar.gameObject.SetActive(false);
        }

        
        healthBar.healtSlider.fillAmount = 1;
        maxHealth = 100;
        minHealth = 0;
        currentHealth = 100;
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetDamage(20f);
        }
    }
    public void GetDamage(float damage)
    {
        Debug.Log("GetDamage" + damage);
        StartCoroutine(ReduceHealthSmoothly(damage));
       
    }

    IEnumerator ReduceHealthSmoothly(float damage)
    {
        healthBar.gameObject.SetActive(true);
        float targetHealth = Mathf.Clamp(currentHealth - damage, minHealth, maxHealth);

        while (currentHealth > targetHealth)
        {
            currentHealth = Mathf.MoveTowards(currentHealth, targetHealth, damageReductionSpeed * Time.deltaTime);
            healthBar.healtSlider.fillAmount = currentHealth / maxHealth;
            yield return null;
        }


        if (gameObject.CompareTag("Player"))
        {
            if (currentHealth <= 0)
                GameController.instance.LevelFail();
        }
        else
        {
            GameController.instance.UpgradInfo();
            Destroy(gameObject, 0.1f);
      
           healthBar.gameObject.SetActive(false);
        }
                
        Debug.Log("Health Reduced to: " + currentHealth);
    }
}
