using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    private TextMeshProUGUI moneyText;

    void Start()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        moneyText.text = PlayerStats.Money.ToString();
    }
}
