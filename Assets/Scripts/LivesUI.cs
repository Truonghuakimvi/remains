using TMPro;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    private TextMeshProUGUI livesText;

    void Start()
    {
        livesText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        livesText.text = PlayerStats.Lives.ToString();
    }
}
