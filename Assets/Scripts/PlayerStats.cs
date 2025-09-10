using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public static int Lives;
    public static int gameSpeed;
    public int startMoney = 400;
    public int startLives = 20;
    private float elapsedTime;
    public Image moneyBar;

    void Start()
    {
        gameSpeed = 1;
        Money = startMoney;
        Lives = startLives;
        elapsedTime = 1f;
        moneyBar.fillAmount = 0;
    }

    void Update()
    {
        if (GameManager.gameEnded)
            return;

        elapsedTime -= Time.deltaTime;

        moneyBar.fillAmount = 1 - (elapsedTime / 1);

        if (elapsedTime <= 0f) 
        { 
            elapsedTime = 1f;
            Money++;
            moneyBar.fillAmount = 0;
        }
    }
}
