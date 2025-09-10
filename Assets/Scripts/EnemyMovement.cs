using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Enemy enemy;

    private Transform target;

    public Waypoint waypoint;

    private int wavepointIndex = 0;

    void Start()
    {
        enemy = GetComponent<Enemy>();

        target = waypoint.points[0];
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;

        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= waypoint.points.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = waypoint.points[wavepointIndex];
    }

    void EndPath()
    {
        PlayerStats.Lives--;
        if (PlayerStats.Lives > 0)
        BuildManager.instance.PlayErrorSound();
        GetComponent<Enemy>().DeathAnimation();
    }
}
