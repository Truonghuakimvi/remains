using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public TurretBlueprint turret;
    public Image icon;
    public InventoryUI inventoryUI;

    public void AddItem(TurretBlueprint newTurret)
    {
        turret = newTurret;
        icon.sprite = turret.icon;
       
        foreach (TeamSlot slot in inventoryUI.partySlots)
        {
            if (slot.turret == turret)
            {
                gameObject.SetActive(false);
                break;
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }

    public void ClearItem()
    {
        gameObject.SetActive(false);
    }

    public void AddToParty()
    {
        Inventory.Instance.AddParty(turret);
        inventoryUI.UpdatePartyUI();
    }
}
