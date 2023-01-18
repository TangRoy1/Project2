using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosData
{
    public Vector2Int pos;
    public List<Vector2Int> arrayUsedPos;

    public PosData(Vector2Int pos)
    {
        this.pos = pos;
        arrayUsedPos = new List<Vector2Int>();
    }

    public void AddUsedPos(Vector2Int usedPos)
    {
        arrayUsedPos.Add(usedPos);
    }
}
