using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;

    public InventorySlot[] inventorySlots;

    public TeamSlot[] partySlots;

    public float timeElapsed;

    void Start()
    {
        inventory = Inventory.Instance;

        inventorySlots = GetComponentsInChildren<InventorySlot>();
        partySlots = GetComponentsInChildren<TeamSlot>();

        gameObject.SetActive(false);
    }

    public void UpdateInventoryUI()
    {
        UpdatePartyUI();
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < levelReached)
            {
                inventorySlots[i].AddItem(inventory.turrets[i]);
            }
            else
            {
                inventorySlots[i].ClearItem();
            }
        }
    }

    public void UpdatePartyUI()
    {
        for (int i = 0; i < partySlots.Length; i++)
        {
            if (i < inventory.partyTurrets.Count)
            {
                partySlots[i].AddItem(inventory.partyTurrets[i]);
            }
            else
            {
                partySlots[i].ClearItem();
            }
        }
    }
}
