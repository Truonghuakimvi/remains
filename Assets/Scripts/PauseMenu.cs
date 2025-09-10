using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject retreatButton;
    public GameObject quitAlert;
    public GameObject ui;

    public SceneFader sceneFader;

    public string sceneToLoad = "LevelSelect";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        quitAlert.SetActive(false);
        ui.SetActive(!ui.activeSelf);
        BuildManager.instance.SelectTurretToBuild(null, null);

        if (ui.activeSelf)
        {
            Time.timeScale = 0;
            GameManager.gamePaused = true;
            BuildManager.instance.GetComponent<AudioSource>().Pause();       
        }
        else
        {
            Time.timeScale = PlayerStats.gameSpeed;
            GameManager.gamePaused = false;
            BuildManager.instance.GetComponent<AudioSource>().UnPause();
            retreatButton.SetActive(true);
        }
    }

    public void Retry()
    {
        Toggle();
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Toggle();
        sceneFader.FadeTo(sceneToLoad);
    }

    public void Retreat()
    {
        PlayerStats.Lives = 0;
        Toggle();
    }
}
