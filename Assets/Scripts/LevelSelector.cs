using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public SceneFader Fader;

    public GameObject[] levelButtons;

    public string levelToLoad;

    public string mainTitle = "MainMenu";

    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
            levelButtons[i].SetActive(false);
        }
    }

    public void Select(string level)
    {
        levelToLoad = level;
    }

    public void StartLevel()
    {
        Fader.FadeTo(levelToLoad);
    }

    public void ReturnToTitle()
    {
        Fader.FadeTo(mainTitle);
    }
}
