using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretCooldown : MonoBehaviour
{
	private TurretBlueprint turretBlueprint;
	public GameObject cooldownUI;
	public Image cooldownCircle;
    public TextMeshProUGUI cooldownText;
	private float cooldownTime;
	private float currentTime = 0f;
	public bool startCooldown;

	private CanvasGroup canvasGroup;

	void Start()
	{
		startCooldown = false;
		canvasGroup = GetComponent<CanvasGroup>();
	}

	void Update()
	{
		if (startCooldown)
		{
			canvasGroup.interactable = false;
			currentTime -= Time.deltaTime;
			cooldownText.text = currentTime.ToString("0.0");
			cooldownCircle.fillAmount = 1f - (currentTime / cooldownTime);

            if (currentTime <= 0)
			{
				cooldownUI.SetActive(false);
				startCooldown = false;

				if (turretBlueprint.cost > PlayerStats.Money)
				{
                    canvasGroup.interactable = false;
                }
				else
				{
					canvasGroup.interactable = true;
				}
			}
		}
	}

	public void StartCooldown(int cooldown, TurretBlueprint turretBlueprint)
	{
		cooldownUI.SetActive(true);
		currentTime = cooldown;
		cooldownTime = cooldown;
		startCooldown = true;
		this.turretBlueprint = turretBlueprint;
	}
}
