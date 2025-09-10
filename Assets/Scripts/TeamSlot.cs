using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TeamSlot : MonoBehaviour
{
    public TurretBlueprint turret;
    public Image icon;
    public InventoryUI inventoryUI;
    public GameObject button;

    public void AddItem(TurretBlueprint newTurret)
    {
        turret = newTurret;

        foreach(InventorySlot slot in inventoryUI.inventorySlots)
        {
            if (slot.turret == turret)
            {
                button = slot.gameObject;
                break;
            }
        }

        button.SetActive(false);
        icon.sprite = turret.icon;
        icon.enabled = true;
    }

    public void ClearItem()
    {
        icon.enabled = false;
        turret = null;
        button = null;
    }

    public void RemoveFromParty()
    {
        StartCoroutine(RemoveWithDelay());
    }

    IEnumerator RemoveWithDelay()
    {
        if (button != null)
            button.SetActive(true);

        yield return null;

        Inventory.Instance.RemoveParty(turret);
        inventoryUI.UpdatePartyUI();
    }
}
