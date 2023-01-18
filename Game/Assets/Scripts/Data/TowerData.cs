using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : VisualUnitData
{
    public int maxLevel;
    public int currentLevel;
    public List<Vector2Int> territory;
    public TypeFlag typeFlag;
    public bool isHealer;
}
