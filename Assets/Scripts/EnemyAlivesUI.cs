using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAlivesUI : MonoBehaviour
{
    public WaveSpawner waveSpawner;

    private TextMeshProUGUI enemyText;

    void Start()
    {
        enemyText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        enemyText.text = WaveSpawner.enemyDefeat + "/ " + waveSpawner.totalEnemy.ToString();
    }
}
