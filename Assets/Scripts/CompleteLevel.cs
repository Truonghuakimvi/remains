using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
    public string levelToLoad = "LevelSelect";

    public SceneFader sceneFader;

    public int levelToUnlock;

    public void Menu()
    {
        if (PlayerPrefs.GetInt("levelReached", 1) < levelToUnlock)
        {
            PlayerPrefs.SetInt("levelReached", levelToUnlock);
        }
        sceneFader.FadeTo(levelToLoad);
    }
}
