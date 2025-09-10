using UnityEngine;

[CreateAssetMenu(menuName = "Character", fileName = "New Character")]
public class TurretBlueprint : ScriptableObject
{
    public GameObject prefab;
    public int cost;
    public bool isGroundTurret;
    public int cooldown;
    public Sprite icon;
    public Sprite classIcon;
    public Color classColor;
}
