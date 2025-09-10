using UnityEngine;

public class NodeUI : MonoBehaviour
{
    private Node target;
    public GameObject ui;
    public GameObject rangeUI;

    void Update()
    {
        if (GameManager.gameEnded)
        {
            Hide();
        }
    }

    public void SetTarget(Node node)
    {
        target = node;
        transform.position = target.transform.position;

        Turret turret = target.turret.GetComponent<Turret>();
        if (turret != null)
        {
            SetRangeUI(turret.rangeUI);
        }
        else
        {
            Healer healer = target.turret.GetComponent<Healer>();
            if (healer != null)
            {
                SetRangeUI(healer.rangeUI);
            }
        }

        ui.SetActive(true);
        rangeUI.SetActive(true);
        Time.timeScale = 0.25f;
    }

    private void SetRangeUI(GameObject newRangeUIObject)
    {
        RectTransform newRangeUI = newRangeUIObject.GetComponent<RectTransform>();
        rangeUI.GetComponent<RectTransform>().sizeDelta = newRangeUI.sizeDelta;
        rangeUI.GetComponent<RectTransform>().anchoredPosition = newRangeUI.anchoredPosition;
    }

    public void Hide()
    {
        ui.SetActive(false);
        rangeUI.SetActive(false);
        Time.timeScale = PlayerStats.gameSpeed;
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }
}