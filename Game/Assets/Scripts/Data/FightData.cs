using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightData
{
    public TypeLocation typeLocation;
    public WarriorsData warriorsData;
    public List<Vector2Int> arrayPosHeros;
    public Vector2Int posEnemy;

    public FightData(TypeLocation typeLocation, WarriorsData warriorsData, List<Vector2Int> arrayPosHeros, Vector2Int posEnemy)
    {
        this.typeLocation = typeLocation;
        this.warriorsData = warriorsData;
        this.arrayPosHeros = arrayPosHeros;
        this.posEnemy = posEnemy;
    }
}
