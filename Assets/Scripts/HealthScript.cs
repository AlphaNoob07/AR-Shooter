using System.Collections;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    private float maxHealth, minHealth, currentHealth;
    public float damageReductionSpeed = 10.0f;
    private HealthBar healthBar;



    private void Start()
    {
        SetHealth();
        GameController.instance.OnResetLevelEvent += OnResetEventHandler;

    }


    private void OnEnable()
    {
        SetHealth();
        
    }
    private void OnDisable()
    {
        GameController.instance.OnResetLevelEvent -= OnResetEventHandler;
    }


    public void OnResetEventHandler(int i)
    {
        Debug.Log(" ResetLevel");
        SetHealth();
    }

    public void SetHealth()
    {
        if (gameObject.CompareTag("Player"))
        {
            healthBar = GameController.instance.playerHanthBar;
        }
        else
        {
            healthBar = GameController.instance.AIHealthBar;
            // healthBar.gameObject.SetActive(false);
        }


        healthBar.healtSlider.fillAmount = 1;
        maxHealth = 100;
        minHealth = 0;
        currentHealth = 100;

    }

    /*    void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetDamage(20f);
            }
        }*/


    public void GetDamage(float _damage)
    {
        healthBar.gameObject.SetActive(true);
        Debug.Log("GetDamage" + _damage);
        StartCoroutine(ReduceHealthSmoothly(_damage));

    }

    IEnumerator ReduceHealthSmoothly(float damage)
    {

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
            {
                GameController.instance.score += Mathf.FloorToInt(damage);
                GameController.instance.LevelFail();
            }
        

        }
        else
        {
            if (currentHealth <= 0)
            {
                healthBar.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
               GameController.instance.currentEnemy++;
            }
            GameController.instance.UpgradInfo();
        }

        GameController.instance.UpgradInfo();
        Debug.Log("Health Reduced to: " + currentHealth);
    }
}
