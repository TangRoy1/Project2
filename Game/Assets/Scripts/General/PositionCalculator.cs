using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PositionCalculator
{
    public static Vector3 additionPos = new Vector3(-0.03f, 1.5f, 0);
    public static Vector2Int ERROR_POS = new Vector2Int(-1000, -1000);

    public static List<Vector2Int> GetArrayPosToTarget(Vector2Int startPos, Vector2Int endPos, Vector2Int baseMinPos, Vector2Int baseMaxPos, GameObject[,] map, int maxStep = 1000)
    {
        PosData currentPos = new PosData(startPos);
        int posId = 0;
        List<PosData> arrayPos = new List<PosData>() { currentPos};
        Vector2Int maxPos = GetMaxPos(startPos, endPos, baseMaxPos);
        Vector2Int minPos = GetMinPos(startPos, endPos, baseMinPos);

        while (true)
        {
            Vector2Int availablePos = GetAvailablePos(currentPos, endPos, minPos, maxPos, map);
            if (availablePos == endPos)
            {
                arrayPos.Add(new PosData(availablePos));
                arrayPos.RemoveAt(0);
                break;
            }
            else if (availablePos == ERROR_POS)
            {
                arrayPos.Remove(currentPos);
                posId--;
                if (posId < 0)
                {
                    return null;
                }
                currentPos = arrayPos[posId];
            }
            else
            {
                currentPos.AddUsedPos(availablePos);
                Vector2Int lastPos = currentPos.pos;
                currentPos = new PosData(availablePos);
                currentPos.AddUsedPos(lastPos);
                posId++;
                arrayPos.Add(currentPos);
            }
            if (arrayPos.Count > maxStep)
            {
                return null;
            }
        }
        List<Vector2Int> outputPositions = new List<Vector2Int>();
        for (int i = 0; i < arrayPos.Count; i++)
        {
            outputPositions.Add(arrayPos[i].pos);
        }
        return outputPositions;
    }

    static Vector2Int GetMaxPos(Vector2Int pos1, Vector2Int pos2, Vector2Int baseMaxPos)
    {
        Vector2Int maxPos = new Vector2Int();

        if (pos1.x > pos2.x)
        {
            if (pos1.x + 5 <= baseMaxPos.x)
            {
                maxPos.x = pos1.x + 5;
            }
            else
            {
                maxPos.x = baseMaxPos.x;
            }
        }
        else
        {
            if (pos2.x + 5 <= baseMaxPos.x)
            {
                maxPos.x = pos2.x + 5;
            }
            else
            {
                maxPos.x = baseMaxPos.x;
            }
        }

        if (pos1.y > pos2.y)
        {
            if (pos1.y + 5 <= baseMaxPos.y)
            {
                maxPos.y = pos1.y + 5;
            }
            else
            {
                maxPos.y = baseMaxPos.y;
            }
        }
        else
        {
            if (pos2.y + 5 <= baseMaxPos.y)
            {
                maxPos.y = pos2.y + 5;
            }
            else
            {
                maxPos.y = baseMaxPos.y;
            }
        }
        return maxPos;
    }

    static Vector2Int GetMinPos(Vector2Int pos1, Vector2Int pos2, Vector2Int baseMinPos)
    {
        Vector2Int minPos = new Vector2Int();

        if (pos1.x < pos2.x)
        {
            if (pos1.x - 5 >= baseMinPos.x)
            {
                minPos.x = pos1.x - 5;
            }
            else
            {
                minPos.x = baseMinPos.x;
            }
        }
        else
        {
            if (pos2.x - 5 >= baseMinPos.x)
            {
                minPos.x = pos2.x - 5;
            }
            else
            {
                minPos.x = baseMinPos.x;
            }
        }

        if (pos1.y < pos2.y)
        {
            if (pos1.y - 5 >= baseMinPos.y)
            {
                minPos.y = pos1.y - 5;
            }
            else
            {
                minPos.y = baseMinPos.y;
            }
        }
        else
        {
            if (pos2.y - 5 >= baseMinPos.y)
            {
                minPos.y = pos2.y - 5;
            }
            else
            {
                minPos.y = baseMinPos.y;
            }
        }
        return minPos;
    }

    static Vector2Int GetAvailablePos(PosData currentPos, Vector2Int endPos, Vector2Int minPos, Vector2Int maxPos, GameObject[,] map)
    {
        List<Vector2Int> directs = new List<Vector2Int>()
        {
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
        };
        List<Vector2Int> availablePositions = new List<Vector2Int>();
        for (int i = 0; i < directs.Count; i++)
        {
            int x = currentPos.pos.x + directs[i].x;
            int y = currentPos.pos.y + directs[i].y;
            Vector2Int pos = new Vector2Int(x, y);
            if (pos == endPos)
            {
                return pos;
            }
            if (x >= minPos.x && x < maxPos.x && y >= minPos.y && y < maxPos.y)
            {
                if (currentPos.arrayUsedPos.IndexOf(pos) == -1)
                {
                    if (map[x, y].tag == GameManager.TAG_TILE || (map[x, y].tag == GameManager.TAG_WATER && map[x, y].GetComponent<Water>().isBridge))
                    {
                        if (map[x, y].GetComponent<Tile>().character == null)
                        {
                            availablePositions.Add(pos);
                        }
                    }
                }
            }
        }

        if (availablePositions.Count == 0)
        {
            return ERROR_POS;
        }

        int randNum = Random.Range(0, availablePositions.Count);
        return availablePositions[randNum];
    }

    public static Vector3 GetPosTileFromPosCharacter(Vector3 posCharacter)
    {
        return posCharacter - additionPos;
    }

    public static Vector2Int GetPosArrayFromRealPos(Vector3 realPos,GameObject[,] map)
    {
        for(int i=0;i < map.GetUpperBound(0) + 1; i++)
        {
            for(int j=0;j < map.GetUpperBound(1) + 1; j++)
            {
                if(Vector2.Distance(map[i,j].transform.position,realPos) < 0.1f)
                {
                    return new Vector2Int(i, j);
                }
            }
        }
        throw new System.Exception("Позиция массива соотвествующая реальной позиции не найдена!");
    }

    public static List<Vector2Int> GetBestArrayPosToTarget(Vector2Int startPos,Vector2Int endPos, Vector2Int baseMinPos,Vector2Int baseMaxPos,GameObject[,] map)
    {
        List<List<Vector2Int>> allPositions = new List<List<Vector2Int>>();
        for(int i=0;i < 1000; i++)
        {
            List<Vector2Int> arrayPos = GetArrayPosToTarget(startPos, endPos, baseMinPos, baseMaxPos, map, 50);
            if (arrayPos == null) continue;
            allPositions.Add(arrayPos);
        }

        List<Vector2Int> bestArrayPos = new List<Vector2Int>();
        int bestCount = 1000;
        for(int i=0;i < allPositions.Count; i++)
        {
            if(allPositions[i].Count < bestCount)
            {
                bestCount = allPositions[i].Count;
                bestArrayPos = allPositions[i];
            }
        }
        return bestArrayPos;
    }

    public static List<Vector3> GetRealPositionsFromArrayPos(List<Vector2Int> arrayPos, GameObject[,] map)
    {
        List<Vector3> realPositions = new List<Vector3>();
        for(int i=0;i < arrayPos.Count; i++)
        {
            realPositions.Add(map[arrayPos[i].x, arrayPos[i].y].transform.position);
        }
        return realPositions;
    }
}