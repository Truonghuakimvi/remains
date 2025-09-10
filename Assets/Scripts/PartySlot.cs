using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartySlot : MonoBehaviour
{
    public TurretBlueprint turret;
    public Image icon;
    public Button button;
    public TextMeshProUGUI costText;
    public Image classIcon;
    public Image classColor;
    public GameObject cooldownUI;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI cooldownText;
    public Image cooldownCircle;
    private HorizontalLayoutGroup layoutGroup;
    private float currentTime;

    void Start()
    {
        cooldownUI.SetActive(false);
        layoutGroup = transform.parent.GetComponent<HorizontalLayoutGroup>();
    }

    void Update()
    {
        if (!BuildManager.instance.turretIsSelected)
        {
            layoutGroup.enabled = true;
        }

        if (turret.cost > PlayerStats.Money || cooldownUI.activeSelf)
        {
            canvasGroup.interactable = false;
        }
        else
        {
            canvasGroup.interactable = true;
        }

        if (cooldownUI.activeSelf)
        {
            currentTime -= Time.deltaTime;
            cooldownText.text = currentTime.ToString("0.0");
            cooldownCircle.fillAmount = 1f - (currentTime / turret.cooldown);

            if (currentTime <= 0)
            {
                cooldownUI.SetActive(false);
                currentTime = turret.cooldown;
            }
        }
    }

    public void AddItem (TurretBlueprint newTurret)
    {
        turret = newTurret;
        icon.sprite = turret.icon;
        icon.enabled = true;
        costText.text = turret.cost.ToString();
        classIcon.sprite = turret.classIcon;
        classColor.color = turret.classColor;
        currentTime = turret.cooldown;
    }

    public void ClearSlot()
    {
        gameObject.SetActive(false);
    }

    public void SelectTurret()
    {
        if (BuildManager.instance.GetTurretToBuild() == turret)
        {
            BuildManager.instance.SelectTurretToBuild(null, null);
            return;
        }

        StartCoroutine(DelayAdjustment());
    }

    IEnumerator DelayAdjustment()
    {
        layoutGroup.enabled = true;

        yield return null;

        transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        gameObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 30f);

        BuildManager.instance.SelectTurretToBuild(turret, gameObject);
        BuildManager.instance.PreviewTurretOn();
    }
}
