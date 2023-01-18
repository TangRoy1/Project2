using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Tile
{
    public bool isBridge;

    private void Start()
    {
        if (isBridge)
        {
            CreateBridge();
        }
    }

    void CreateBridge()
    {
        GameObject bridgePrefab = GlobalMapManager.instance.bridgePrefab;
        GameObject clon = Instantiate(bridgePrefab, Vector3.zero, Quaternion.identity, transform);
        clon.transform.localPosition = new Vector3(0, 0.18f,0);
        clon.name = "Bridge";
    }
}
