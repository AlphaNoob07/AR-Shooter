using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI scoreText;
    public float updateSpeed = 5.0f;

    private int currentScore;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(ShowScoreSmoothly(7500));
        }
    }




    IEnumerator ShowScoreSmoothly(float score)
    {

        currentScore = 0;
        scoreText.enabled = true;
        titleText.text = "";
        float targetScore = score;
        float elapsedTime = 0f;

        switch (GameController.instance.gameState)
        {
            case GameController.GameState.levelComplete:
                titleText.text = "Level"+ "\n" + "Complete";
                break;
            case GameController.GameState.levelFail:
                titleText.text = "Level"+ "\n" + "Fail";
                break;
            default:
                titleText.text = "Hudha";
                break;
        }


      

        while (currentScore < targetScore)
        {
            // Calculate the progress of time
            float t = Mathf.Clamp01(elapsedTime / updateSpeed);

            // Update the current score based on the progress
            currentScore = Mathf.RoundToInt(Mathf.Lerp(0, targetScore, t));

            Debug.Log("Score " + currentScore);

            // Update the scoreText
            scoreText.text = "Score: " + "\n" + currentScore.ToString();
            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        scoreText.text = "Score: " + "\n" + targetScore.ToString();

        yield return new WaitForSeconds(5);
        scoreText.enabled = false;
        switch (GameController.instance.gameState)
        {
            case GameController.GameState.levelComplete:
                titleText.text = "Level" + GameController.instance.levelNumber.ToString() + "\n" + "Ready";
                break;
            case GameController.GameState.levelFail:
                titleText.text = "Level" + GameController.instance.levelNumber.ToString() + "\n" + "restart";
                break;
            default:
                titleText.text = "Hudai chhalaln";
                break;
        }
        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
        GameController.instance.RestartGame();
    }
    private void OnEnable()
    {
        if (GameController.instance != null)
        {
            float s = GameController.instance.score;
            StartCoroutine(ShowScoreSmoothly(s));
        }

    }

}
