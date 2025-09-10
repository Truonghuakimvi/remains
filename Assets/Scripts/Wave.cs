using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject enemy;
    public int count;
    public float rate;
    public Waypoint waypoint;
    public Transform spawnpoint;
    public float timeout;
}
