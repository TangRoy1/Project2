using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparingFightData
{
    public TypeLocation typeLocation;
    public TypeLocation typeLocationEnemy;
    public Vector2Int posEnemy;
    public Vector3 attackPoint;

    public PreparingFightData(TypeLocation typeLocation, TypeLocation typeLocationEnemy, Vector2Int posEnemy, Vector3 attackPoint)
    {
        this.typeLocation = typeLocation;
        this.typeLocationEnemy = typeLocationEnemy;
        this.posEnemy = posEnemy;
        this.attackPoint = attackPoint;
    }
}
