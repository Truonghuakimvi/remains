using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPreview : MonoBehaviour
{
    private BuildManager buildManager;
    public bool isOnNode;
    public GameObject turret;

    void Start()
    {
        buildManager = BuildManager.instance;      
    }

    void Update()
    {
        if (buildManager.turretIsSelected && !isOnNode)
        {
            TrackMouse();
        }
    }

    private void TrackMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            transform.position = hitInfo.point;
        }
    }
}
