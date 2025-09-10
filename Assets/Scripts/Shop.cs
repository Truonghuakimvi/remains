using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<TurretBlueprint> standardTurret;

    public PartySlot[] slots;

    void Start()
    {
        slots = GetComponentsInChildren<PartySlot>();

        standardTurret = Inventory.Instance.partyTurrets;

        UpdateParty();
    }

    void UpdateParty()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < standardTurret.Count)
            {
                slots[i].AddItem(standardTurret[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
