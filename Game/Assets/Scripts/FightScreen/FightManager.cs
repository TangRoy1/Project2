using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour,IActionHandler
{
    public static FightManager instance;
    [SerializeField]
    GameObject tilePrefab;
    [SerializeField]
    Transform startPos;
    [SerializeField]
    Transform parent; 
    GameObject[,] map;
    [SerializeField]
    GameObject heroPrefab;
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    GameObject rip;
    const int MAX_X = 13;
    const int MAX_Y = 7;
    Vector2Int maxPos;
    Vector2Int minPos;
    GameObject clickedHero;
    GameObject currentHero;
    List<Tile> selectedTiles;
    List<GameObject> activeEnemies;
    List<GameObject> activeHeros;
    List<GameObject> movedCharacters;
    bool clickIsAvailable;
    bool isMoved;
    FightData fightData;
    MissionData missionData;
    [SerializeField]
    GameObject winWindow;
    [SerializeField]
    GameObject failWindow;
    bool isMission;

    private void Awake()
    {
        instance = this;
    }

    void SetDefaultValues()
    {
        map = new GameObject[MAX_X, MAX_Y];
        maxPos = new Vector2Int(MAX_X , MAX_Y);
        minPos = new Vector2Int(0, 0);
        clickedHero = null;
        currentHero = null;
        selectedTiles = new List<Tile>();
        activeEnemies = new List<GameObject>();
        activeHeros = new List<GameObject>();
        movedCharacters = new List<GameObject>();
        clickIsAvailable = true;
        isMoved = false;
        fightData = null;
        missionData = null;
        isMission = false;
    }

    public void StartScript(FightData fightData)
    {
        SetDefaultValues();
        this.fightData = fightData;
        CreateMap(fightData.typeLocation);
        CreateHeros(fightData.warriorsData.heros);
        CreateEnemies(fightData.warriorsData.enemies);
        ScreenManager.instance.OnEndedDifficultProcess();
    }

    public void StartScript(MissionData missionData)
    {
        SetDefaultValues();
        this.missionData = missionData;
        CreateMap(missionData.typeLocation);
        CreateHeros(missionData.warriorsData.heros);
        CreateEnemies(missionData.warriorsData.enemies);
        ScreenManager.instance.OnEndedDifficultProcess();
        isMission = true;
    }

    void CreateMap(TypeLocation typeLocation)
    {
        Vector3 basePos = startPos.position;
        Vector3 pos = basePos;
        int increase = 0;
        for (int i = 0; i < MAX_X; i++)
        {
            for (int j = 0; j < MAX_Y; j++)
            {
                GameObject clon = Instantiate(tilePrefab, pos, Quaternion.identity, parent);
                clon.name = "Tile";
                clon.GetComponent<SpriteRenderer>().sortingOrder = increase++;
                map[i, j] = clon;
                int randNum = Random.Range(1, 4);
                clon.AddComponent<Tile>();
                clon.GetComponent<Tile>().typeLocation = TypeLocation.Forest;
                clon.GetComponent<Tile>().number = randNum;
                clon.GetComponent<Tile>().x = i;
                clon.GetComponent<Tile>().y = j;
                clon.GetComponent<Tile>().typeUnit = TypeUnit.Tile;
                clon.GetComponent<Tile>().tag = GameManager.TAG_TILE;
                Sprite sprite = Resources.Load<Sprite>($"Tiles/{typeLocation}/Tile{randNum}");
                clon.GetComponent<SpriteRenderer>().sprite = sprite;
                ScreenManager.instance.AddActiveObject(clon);

                if (j % 2 != 0)
                {
                    pos = new Vector3(basePos.x, pos.y - 1.6f, 0);
                }
                else
                {
                    pos += new Vector3(0.92f, -1.6f, 0);
                }
            }
            basePos += new Vector3(0.92f * 2, 0, 0);
            pos = basePos;
            increase = 0;
        }
    }

    void CreateHeros(List<WarriorData> warriors)
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>()
        {
            new Vector2Int(0,2),
            new Vector2Int(0,4),
            new Vector2Int(1,3)
        };

        for(int i=0;i < warriors.Count; i++)
        {
            Vector3 pos = map[availablePositions[i].x, availablePositions[i].y].transform.position + PositionCalculator.additionPos;
            GameObject clon = Instantiate(heroPrefab, new Vector3(pos.x,pos.y,-2), new Quaternion(0, 190, 0, 0), parent);
            clon.GetComponent<Warrior>().health = warriors[i].health;
            clon.GetComponent<Warrior>().speed = warriors[i].speed;
            clon.GetComponent<Warrior>().distance = warriors[i].distance;
            clon.GetComponent<Warrior>().level = warriors[i].level;
            clon.GetComponent<Warrior>().typeLocation = warriors[i].typeLocation;
            clon.GetComponent<Warrior>().number = warriors[i].number;
            clon.GetComponent<Warrior>().typeCharacter = TypeCharacter.Hero;
            clon.GetComponent<Warrior>().typeUnit = TypeUnit.Character;
            clon.GetComponent<Warrior>().x = warriors[i].x;
            clon.GetComponent<Warrior>().y = warriors[i].y;
            clon.name = "Hero";
            Sprite sprite = Resources.Load<Sprite>($"Warriors/{warriors[i].typeLocation}/{warriors[i].typeLocation}{warriors[i].number}");
            clon.GetComponent<SpriteRenderer>().sprite = sprite;
            activeHeros.Add(clon);
            ScreenManager.instance.AddActiveObject(clon);
        }
    }

    void CreateEnemies(List<WarriorData> warriors)
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>()
        {
            new Vector2Int(12,2),
            new Vector2Int(12,4),
            new Vector2Int(11,3)
        };

        for (int i = 0; i < warriors.Count; i++)
        {
            Vector3 pos = map[availablePositions[i].x, availablePositions[i].y].transform.position + PositionCalculator.additionPos;
            GameObject clon = Instantiate(enemyPrefab, new Vector3(pos.x,pos.y,2), Quaternion.identity, parent);
            clon.GetComponent<Warrior>().health = warriors[i].health;
            clon.GetComponent<Warrior>().speed = warriors[i].speed;
            clon.GetComponent<Warrior>().distance = warriors[i].distance;
            clon.GetComponent<Warrior>().level = warriors[i].level;
            clon.GetComponent<Warrior>().typeLocation = warriors[i].typeLocation;
            clon.GetComponent<Warrior>().number = warriors[i].number;
            clon.GetComponent<Warrior>().typeCharacter = TypeCharacter.Enemy;
            clon.GetComponent<Warrior>().typeUnit = TypeUnit.Character;
            clon.name = "Enemy";
            Sprite sprite = Resources.Load<Sprite>($"Warriors/{warriors[i].typeLocation}/{warriors[i].typeLocation}{warriors[i].number}");
            clon.GetComponent<SpriteRenderer>().sprite = sprite;
            activeEnemies.Add(clon);
            ScreenManager.instance.AddActiveObject(clon);
        }
    }

    public void OnBecameVisibleTile(Tile tile)
    {
      // throw new System.NotImplementedException();
    }

    public void OnCreatedTile(Tile tile)
    {
      //  throw new System.NotImplementedException();
    }

    public void OnClickedHero(GameObject hero)
    {
        if (!clickIsAvailable) return;
        if (isMoved) return;
        int speed = hero.GetComponent<Character>().speed;
        float distance = hero.GetComponent<Warrior>().distance;
        if (movedCharacters.IndexOf(hero) != -1) return;

        if (clickedHero != null)
        {
            if (clickedHero == hero)
            {
                clickedHero = null;
                UnSelectTiles();
            }
            else
            {
                UnSelectTiles();
                Vector3 realPos = PositionCalculator.GetPosTileFromPosCharacter(hero.transform.position);
                Collider2D[] cols = Physics2D.OverlapCircleAll(realPos, speed);
                currentHero = hero;
                clickedHero = hero;
                SelectTilesOfColliders(cols);
                SelectTilesOfEnemies(distance);
            }
        }
        else
        {
            Vector3 realPos = PositionCalculator.GetPosTileFromPosCharacter(hero.transform.position);
            Collider2D[] cols = Physics2D.OverlapCircleAll(realPos, speed);
            currentHero = hero;
            clickedHero = hero;
            SelectTilesOfColliders(cols);
            SelectTilesOfEnemies(distance);
        }
    }

    public void OnClickedTile(Tile tile)
    {
        if (tile.isSelected)
        {
            Vector3 realPos = PositionCalculator.GetPosTileFromPosCharacter(currentHero.transform.position);
            Vector2Int startPos = PositionCalculator.GetPosArrayFromRealPos(realPos, map);
            Vector2Int endPos = PositionCalculator.GetPosArrayFromRealPos(tile.transform.position, map);
            float distance = currentHero.GetComponent<Warrior>().distance;
            if (tile.character != null)
            {
                if (AttackIsAvailable(startPos, endPos, distance))
                {
                    currentHero.GetComponent<Warrior>().Attack(tile.character.GetComponent<Warrior>());
                    OnMovedHero(currentHero);
                }
                else
                {
                    List<Vector2Int> bestArrayPos = PositionCalculator.GetBestArrayPosToTarget(startPos, endPos, minPos, maxPos, map);
                    bestArrayPos.RemoveAt(bestArrayPos.Count - 1);
                    Vector2Int endPos2 = bestArrayPos[bestArrayPos.Count - 1];
                    List<Vector3> realPositions = PositionCalculator.GetRealPositionsFromArrayPos(bestArrayPos, map);
                    currentHero.GetComponent<Warrior>().target = tile.character.GetComponent<Warrior>();
                    currentHero.GetComponent<IMove>().ArrayPos = realPositions;
                    currentHero.GetComponent<IMove>().StartMove();
                    map[startPos.x, startPos.y].GetComponent<Tile>().DeleteCharacter();
                    map[endPos2.x, endPos2.y].GetComponent<Tile>().SetCharacter(currentHero);
                    isMoved = true;
                }
            }
            else
            {
                List<Vector2Int> bestArrayPos = PositionCalculator.GetBestArrayPosToTarget(startPos, endPos, minPos, maxPos, map);
                List<Vector3> realPositions = PositionCalculator.GetRealPositionsFromArrayPos(bestArrayPos, map);
                currentHero.GetComponent<IMove>().ArrayPos = realPositions;
                currentHero.GetComponent<IMove>().StartMove();
                map[startPos.x, startPos.y].GetComponent<Tile>().DeleteCharacter();
                map[endPos.x, endPos.y].GetComponent<Tile>().SetCharacter(currentHero);
                isMoved = true;
            }
        }
        UnSelectTiles();
        clickedHero = null;
    }

    public void OnMovedHero(GameObject hero)
    {
        
            movedCharacters.Add(hero);
            if (movedCharacters.Count == activeHeros.Count)
            {
                movedCharacters = new List<GameObject>();
                clickIsAvailable = false;
                ActivateEnemies();
            }
        
        isMoved = false;
    }

    public void OnMovedEnemy(GameObject enemy)
    {
        movedCharacters.Add(enemy);
        if (movedCharacters.Count == activeEnemies.Count)
        {
            movedCharacters = new List<GameObject>();
            clickIsAvailable = true;
        }
        else
        {
            ActivateEnemies();
        }
    }

    void SelectTilesOfColliders(Collider2D[] cols)
    {
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].tag == GameManager.TAG_TILE)
            {
                Tile tile = cols[i].GetComponent<Tile>();
                if (tile.character != null && tile.character.GetComponent<Character>().typeCharacter == TypeCharacter.Hero) continue;
                Vector3 realPos1 = PositionCalculator.GetPosTileFromPosCharacter(currentHero.transform.position);
                Vector2Int startPos = PositionCalculator.GetPosArrayFromRealPos(realPos1, map);
                Vector2Int endPos = PositionCalculator.GetPosArrayFromRealPos(tile.transform.position, map);
                List<Vector2Int> arrayPosToTarget = PositionCalculator.GetArrayPosToTarget(startPos, endPos, minPos, maxPos, map);
                if (arrayPosToTarget == null) continue;
                tile.Select();
                selectedTiles.Add(tile);
            }
        }
    }

    void SelectTilesOfEnemies(float distance)
    {
        for(int i=0;i < activeEnemies.Count; i++)
        {
            Vector3 realPos1 = PositionCalculator.GetPosTileFromPosCharacter(currentHero.transform.position);
            Vector2Int startPos = PositionCalculator.GetPosArrayFromRealPos(realPos1, map);
            Vector3 realPos2 = PositionCalculator.GetPosTileFromPosCharacter(activeEnemies[i].transform.position);
            Vector2Int endPos = PositionCalculator.GetPosArrayFromRealPos(realPos2, map);
            if (AttackIsAvailable(startPos, endPos, distance))
            {
                Tile tile = map[endPos.x, endPos.y].GetComponent<Tile>();
                tile.Select();
                selectedTiles.Add(tile);
            }
        }
    }

    void UnSelectTiles()
    {
        for (int i = 0; i < selectedTiles.Count; i++)
        {
            selectedTiles[i].Unselect();
        }
        selectedTiles = new List<Tile>();
    }

    bool AttackIsAvailable(Vector2Int startPos, Vector2Int endPos, float distance)
    {
        if (Vector2Int.Distance(startPos, endPos) <= distance)
        {
            return true;
        }
        return false;
    }

    void ActivateEnemies()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (movedCharacters.IndexOf(activeEnemies[i]) == -1)
            {
                SendEnemy(activeEnemies[i]);
                return;
            }
        }
    }

    void SendEnemy(GameObject enemy)
    {
        Vector3 realPos = PositionCalculator.GetPosTileFromPosCharacter(enemy.transform.position);
        Vector2Int startPos = PositionCalculator.GetPosArrayFromRealPos(realPos, map);
        GameObject nearHero = GetNearHero(startPos);
        if (nearHero == null)
        {
            OnMovedEnemy(enemy);
            return;
        }
        Vector3 realPos2 = PositionCalculator.GetPosTileFromPosCharacter(nearHero.transform.position);
        Vector2Int endPos = PositionCalculator.GetPosArrayFromRealPos(realPos2, map);
        float distance = enemy.GetComponent<Warrior>().distance;
        int speed = enemy.GetComponent<Character>().speed;
        if (AttackIsAvailable(startPos, endPos, distance))
        {
            enemy.GetComponent<Warrior>().Attack(nearHero.GetComponent<Warrior>());
            OnMovedEnemy(enemy);
        }
        else
        {
            List<Vector2Int> bestArrayPos = PositionCalculator.GetBestArrayPosToTarget(startPos, endPos, minPos, maxPos, map);
            if (bestArrayPos.Count <= speed)
            {
                bestArrayPos.RemoveAt(bestArrayPos.Count - 1);
                Vector2Int endPos2 = bestArrayPos[bestArrayPos.Count - 1];
                List<Vector3> realPositions = PositionCalculator.GetRealPositionsFromArrayPos(bestArrayPos, map);
                enemy.GetComponent<Warrior>().target = nearHero.GetComponent<Warrior>();
                enemy.GetComponent<IMove>().ArrayPos = realPositions;
                enemy.GetComponent<IMove>().StartMove();
                map[startPos.x, startPos.y].GetComponent<Tile>().DeleteCharacter();
                map[endPos2.x, endPos2.y].GetComponent<Tile>().SetCharacter(enemy);
            }
            else
            {
                List<Vector2Int> bestArrayPos2 = bestArrayPos.GetRange(0, speed);
                Vector2Int endPos2 = bestArrayPos2[bestArrayPos2.Count - 1];
                List<Vector3> realPositions = PositionCalculator.GetRealPositionsFromArrayPos(bestArrayPos2, map);
                enemy.GetComponent<IMove>().ArrayPos = realPositions;
                enemy.GetComponent<IMove>().StartMove();
                map[startPos.x, startPos.y].GetComponent<Tile>().DeleteCharacter();
                map[endPos2.x, endPos2.y].GetComponent<Tile>().SetCharacter(enemy);
            }
        }
    }

    GameObject GetNearHero(Vector2Int startPos)
    {
        List<GameObject> heros = new List<GameObject>();
        heros.AddRange(activeHeros);

        while (heros.Count > 0)
        {
            GameObject nearHero = null;
            float bestDistance = 1000;
            for (int i = 0; i < heros.Count; i++)
            {
                Vector3 realPos1 = PositionCalculator.GetPosTileFromPosCharacter(heros[i].transform.position);
                Vector2Int endPos = PositionCalculator.GetPosArrayFromRealPos(realPos1, map);
                if (Vector2Int.Distance(startPos, endPos) < bestDistance)
                {
                    bestDistance = Vector2Int.Distance(startPos, endPos);
                    nearHero = heros[i];
                }
            }

            Vector3 realPos2 = PositionCalculator.GetPosTileFromPosCharacter(nearHero.transform.position);
            Vector2Int endPos2 = PositionCalculator.GetPosArrayFromRealPos(realPos2, map);
            List<Vector2Int> arrayPosToTarget = PositionCalculator.GetArrayPosToTarget(startPos, endPos2, minPos, maxPos, map);
            if (arrayPosToTarget == null)
            {
                heros.Remove(nearHero);
            }
            else
            {
                return nearHero;
            }
        }
        return null;
    }

    public void RemoveHero(GameObject hero)
    {
        activeHeros.Remove(hero);
        if(activeHeros.Count == 0)
        {
            if (isMission)
            {

            }
            else
            {
                GameManager.instance.xmlMapManager.DeleteCharacterWithPos(fightData.arrayPosHeros);
            }
            GameManager.instance.ChangeStateWindow(failWindow);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        if (activeEnemies.Count == 0)
        {
            if (isMission)
            {
                GameManager.instance.xmlMapManager.ChangeTowerDataWithPos(missionData.posTower, missionData.nextLevel);
            }
            else
            {
                GameManager.instance.xmlMapManager.DeleteCharacterWithPos(fightData.posEnemy);
                UpdateHpHerosInFile();
                GameManager.instance.gameDataManager.ChangeCrystals(Operation.Add, 50);
                GameManager.instance.gameDataManager.SaveData();
            }
            GameManager.instance.ChangeStateWindow(winWindow);
        }
    }

    void UpdateHpHerosInFile()
    {
        for(int i=0;i < activeHeros.Count; i++)
        {
            Warrior warrior = activeHeros[i].GetComponent<Warrior>();
            GameManager.instance.xmlMapManager.ChangeCharacterDataWithPos(new Vector2Int(warrior.x, warrior.y),warrior.health);
        }
    }

    public void ComeToGlobalMap()
    {
        StartCoroutine(StartComingToGlobalMap());
    }

    IEnumerator StartComingToGlobalMap()
    {
        ScreenManager.instance.OnBeganDifficultProcess();
        yield return null;
        ScreenManager.instance.LoadScreen(ScreenManager.GLOBAL_MAP_SCREEN);
    }

    public void CreateRip(Vector3 pos)
    {
        GameObject clon = Instantiate(rip, pos, Quaternion.identity, parent);
        ScreenManager.instance.AddActiveObject(clon);
    }
}
