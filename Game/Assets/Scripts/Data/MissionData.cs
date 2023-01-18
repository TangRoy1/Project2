using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionData
{
    public WarriorsData warriorsData;
    public TypeLocation typeLocation;
    public Vector2Int posTower;
    public int nextLevel;

    public MissionData(WarriorsData warriorsData, TypeLocation typeLocation, Vector2Int posTower, int nextLevel)
    {
        this.warriorsData = warriorsData;
        this.typeLocation = typeLocation;
        this.posTower = posTower;
        this.nextLevel = nextLevel;
    }
}
