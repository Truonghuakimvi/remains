using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory Instance;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }

        Instance = this;
    }
    #endregion

    public LevelSelector levelSelector;
    public CanvasGroup canvasGroup;
    public AlertUI alertUI;

    public List<TurretBlueprint> turrets;
    public int inventorySpace = 10;

    public List<TurretBlueprint> partyTurrets;
    public int partySpace = 4;

    public void AddParty(TurretBlueprint turret)
    {
        if (partyTurrets.Count >= partySpace || partyTurrets.Contains(turret))
        {
            Debug.Log("Error");
            return;
        }

        partyTurrets.Add(turret);
    }

    public void RemoveParty(TurretBlueprint turret)
    {
        partyTurrets.Remove(turret);
    }

    public void CheckParty()
    {
        if (partyTurrets.Count == 0)
        {
            alertUI.Fade();
        }
        else
        {
            levelSelector.StartLevel();
            gameObject.SetActive(false);
            canvasGroup.interactable = false;
        }
    }
}
