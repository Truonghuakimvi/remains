using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameEnded;
    public static bool gamePaused;

    public GameObject gameOverUI;
    public GameObject gameOverTextUI;

    public GameObject completeLevelUI;

    public GameObject ffUI;
    public GameObject normalSpeedUI;

    void Start()
    {
        gameEnded = false;
        gamePaused = false;
    }

    void Update()
    {
        if (gameEnded)
        {
            Time.timeScale = 1.0f;
            BuildManager.instance.GetComponent<AudioSource>().Stop();
            return;
        }
 
        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
    }
    void EndGame()
    {
        gameEnded = true;
        gameOverUI.SetActive(true);
        gameOverTextUI.SetActive(true);
    }

    public void WinGame()
    {
        gameEnded = true;
        completeLevelUI.SetActive(true);
    }

    public void ToggleGameSpeed()
    {
        BuildManager.instance.SelectTurretToBuild(null, null);

        if (PlayerStats.gameSpeed == 1)
        {
            PlayerStats.gameSpeed = 2;
            Time.timeScale = 2;
            normalSpeedUI.SetActive(false);
            ffUI.SetActive(true);
        }
        else
        {
            PlayerStats.gameSpeed = 1;
            Time.timeScale = 1;
            normalSpeedUI.SetActive(true);
            ffUI.SetActive(false);
        }
    }
}
