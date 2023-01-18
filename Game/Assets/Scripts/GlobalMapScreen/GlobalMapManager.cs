using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMapManager : MonoBehaviour,IActionHandler
{
    public static GlobalMapManager instance;
    [SerializeField]
    GameObject tilePrefab;
    [SerializeField]
    Transform startPos;
    [SerializeField]
    Transform parent;
    public GameObject bridgePrefab;
    public GameObject flag;
    public GameObject[,] map;
    [SerializeField]
    GameObject heroPrefab;
    [SerializeField]
    GameObject enemyPrefab;
    const int MAX_X = 100;
    const int MAX_Y = 100;
    Vector2Int maxPos;
    Vector2Int minPos;
    private GameDataManager gameDataManager { get => GameManager.instance.gameDataManager; }
    List<CharacterData> characterDatas;
    GameObject clickedHero;
    GameObject currentHero;
    List<Tile> selectedTiles;
    List<GameObject> activeEnemies;
    List<GameObject> activeHeros;
    List<GameObject> movedCharacters;
    TypeMove typeMove;
    bool clickIsAvailable;
    bool isMoved;
    Coroutine saver;
    [HideInInspector]
    public Tower clickedTower;
    [SerializeField]
    GameObject levelsWindow;
    [SerializeField]
    GameObject upgradeWindow;

    private void Awake()
    {
        instance = this;
    }

    public void StartScript()
    {
        SetDefaultValues();
        if (GameManager.instance.xmlGameDataManager.ExistsData())
        {
            gameDataManager.LoadData();
        }
        else
        {
            gameDataManager.LoadDefaultData();
        }
        if (GameManager.instance.xmlMapManager.ExistsData())
        {
            CreateMap(false);
            LoadMap();
            if (!ExistsHeros())
            {
                Vector2Int posHero = GetAvailablePosForHero(gameDataManager.PosTower);
                CharacterData heroData = GenerateCharacterData(TypeLocation.Forest, 1, TypeCharacter.Hero, posHero, 52, 5);
                AddCharacter(heroData);
            }
        }
        else
        {
            CreateMap(true);
            FillMap();
            Vector2Int posHero = GetAvailablePosForHero(gameDataManager.PosTower);
            CharacterData heroData = GenerateCharacterData(TypeLocation.Forest, 1, TypeCharacter.Hero, posHero, 52, 5);
            AddCharacter(heroData);
            SaveMap();
        }
        saver = StartCoroutine(StartSaving());
        ScreenManager.instance.OnEndedDifficultProcess();
    }

    void SetDefaultValues()
    {
        map = new GameObject[MAX_X, MAX_Y];
        maxPos = new Vector2Int(MAX_X - 10, MAX_Y - 10);
        minPos = new Vector2Int(10, 10);
        characterDatas = new List<CharacterData>();
        clickedHero = null;
        currentHero = null;
        selectedTiles = new List<Tile>();
        activeEnemies = new List<GameObject>();
        activeHeros = new List<GameObject>();
        movedCharacters = new List<GameObject>();
        typeMove = TypeMove.Free;
        clickIsAvailable = true;
        isMoved = false;
        clickedTower = null;
    }

    public void AddActiveEnemy(GameObject activeEnemy)
    {
        if(activeEnemies.Count == 0)
        {
            typeMove = TypeMove.Constraint;
        }
        activeEnemies.Add(activeEnemy);
    }

    public void RemoveActiveEnemy(GameObject activeEnemy)
    {
        activeEnemies.Remove(activeEnemy);
        if(activeEnemies.Count == 0)
        {
            typeMove = TypeMove.Free;
        }
    }

    public void AddActiveHero(GameObject activeHero)
    {
        activeHeros.Add(activeHero);
    }

    public void RemoveActiveHero(GameObject activeHero)
    {
        activeHeros.Remove(activeHero);
    }

    bool ExistsHeros()
    {
        for(int i=0;i < characterDatas.Count; i++)
        {
            if(characterDatas[i].typeCharacter == TypeCharacter.Hero)
            {
                return true;
            }
        }
        return false;
    }

    void CreateMap(bool createVisulUnit)
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
                clon.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                map[i, j] = clon;
                ScreenManager.instance.AddActiveObject(clon);

                if (createVisulUnit)
                {
                    clon.AddComponent<VisualUnit>();
                    clon.GetComponent<VisualUnit>().typeLocation = TypeLocation.Forest;
                    clon.GetComponent<VisualUnit>().number = 1;
                    clon.GetComponent<VisualUnit>().x = i;
                    clon.GetComponent<VisualUnit>().y = j;
                    clon.GetComponent<VisualUnit>().typeUnit = TypeUnit.Tile;
                    clon.GetComponent<VisualUnit>().tag = GameManager.TAG_VISUAL_UNIT;
                    Sprite sprite = Resources.Load<Sprite>("Tiles/Forest/VisualUnit1");
                    clon.GetComponent<SpriteRenderer>().sprite = sprite;
                }

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
    void FillMap()
    {
        List<TypeLocation> locations = new List<TypeLocation>()
        {
            TypeLocation.Desert,
            TypeLocation.Hell,
            TypeLocation.North
        };

        int x = 10;
        int y = 10;
        int add = 10;
        bool createdMainLocation = false;

        while (true)
        {
            int randNum = Random.Range(0, 5);
            Vector2Int maxPos = new Vector2Int(x + add, y + add);
            Vector2Int minPos = new Vector2Int(x, y);
            Tower tower;
            if (!createdMainLocation && randNum == 0)
            {
                createdMainLocation = true;
                tower = CreateTower(maxPos, minPos, TypeLocation.Forest,TypeFlag.Green);
                gameDataManager.PosTower = new Vector2Int(tower.x, tower.y);
                gameDataManager.SaveData();
                CreateWater(maxPos, minPos, TypeLocation.Forest, true);
                CreateWater(maxPos, minPos, TypeLocation.Forest, false);
                CreateMines(maxPos, minPos, TypeLocation.Forest, TypeMine.Gold, 1);
                CreateMines(maxPos, minPos, TypeLocation.Forest, TypeMine.Stone, 2);

                for (int i = x; i < x + add; i++)
                {
                    for (int j = y; j < y + add; j++)
                    {
                        if (map[i, j].tag == GameManager.TAG_VISUAL_UNIT)
                        {
                            Vector2Int pos = new Vector2Int(i, j);
                            CreateTile(pos, TypeLocation.Forest);
                        }
                    }
                }
            }
            else
            {
                int k = Random.Range(0, locations.Count);
                
                tower = CreateTower(maxPos, minPos, locations[k],TypeFlag.Red);
                tower.Territory.Add(new Vector2Int(tower.x, tower.y));
                Vector2Int pos1 = CreateWater(maxPos, minPos, locations[k], true);
                Vector2Int pos2 = CreateWater(maxPos, minPos, locations[k], false);
                tower.Territory.Add(pos1);
                tower.Territory.Add(pos2);
                List<Vector2Int> arrayPos1 = CreateMines(maxPos, minPos, locations[k], TypeMine.Gold, 1);
                List<Vector2Int> arrayPos2 = CreateMines(maxPos, minPos, locations[k], TypeMine.Stone, 2);
                tower.Territory.AddRange(arrayPos1);
                tower.Territory.AddRange(arrayPos2);

                for (int i = x; i < x + add; i++)
                {
                    for (int j = y; j < y + add; j++)
                    {
                        if (map[i, j].tag == GameManager.TAG_VISUAL_UNIT)
                        {
                            Vector2Int pos = new Vector2Int(i, j);
                            CreateTile(pos, locations[k]);
                            tower.Territory.Add(pos);
                        }
                    }
                }
                if (locations[k] == TypeLocation.North)
                {
                    int randNum2 = Random.Range(0, locations.Count);
                    CreateEnemy(locations[randNum2], maxPos, minPos);
                }
                else
                {
                    CreateEnemy(locations[k], maxPos, minPos);
                }
            }
            x += 10;
            if (x >= MAX_X-10)
            {
                x = 10;
                y += 10;
                if(y>= MAX_Y - 10)
                {
                    break;
                }
            }
        }
    }

    Tower CreateTower(Vector2Int maxPos, Vector2Int minPos, TypeLocation typeLocation,TypeFlag typeFlag)
    {
        Vector2Int availablePos = GetAvailablePos(maxPos, minPos);
        Destroy(map[availablePos.x, availablePos.y].GetComponent<VisualUnit>());
        int maxLevel = Random.Range(4, 11);
        map[availablePos.x, availablePos.y].AddComponent<Tower>();
        map[availablePos.x, availablePos.y].GetComponent<Tower>().x = availablePos.x;
        map[availablePos.x, availablePos.y].GetComponent<Tower>().y = availablePos.y;
        map[availablePos.x, availablePos.y].GetComponent<Tower>().typeLocation = typeLocation;
        map[availablePos.x, availablePos.y].GetComponent<Tower>().typeUnit = TypeUnit.Tile;
        map[availablePos.x, availablePos.y].GetComponent<Tower>().tag = GameManager.TAG_TOWER;
        map[availablePos.x, availablePos.y].GetComponent<Tower>().number = 1;
        map[availablePos.x, availablePos.y].GetComponent<Tower>().currentLevel = 0;
        map[availablePos.x, availablePos.y].GetComponent<Tower>().maxLevel = maxLevel;
        Sprite sprite = Resources.Load<Sprite>($"Tiles/{typeLocation}/Tower1");
        map[availablePos.x, availablePos.y].GetComponent<SpriteRenderer>().sprite = sprite;
        map[availablePos.x, availablePos.y].GetComponent<Tower>().SetFlag(typeFlag);
        return map[availablePos.x, availablePos.y].GetComponent<Tower>();
    }

    Vector2Int CreateWater(Vector2Int maxPos, Vector2Int minPos, TypeLocation typeLocation, bool isBridge)
    {
        Vector2Int availablePos = GetAvailablePos(maxPos, minPos);
        Destroy(map[availablePos.x, availablePos.y].GetComponent<VisualUnit>());
        map[availablePos.x, availablePos.y].AddComponent<Water>();
        map[availablePos.x, availablePos.y].GetComponent<Water>().x = availablePos.x;
        map[availablePos.x, availablePos.y].GetComponent<Water>().y = availablePos.y;
        map[availablePos.x, availablePos.y].GetComponent<Water>().typeLocation = typeLocation;
        map[availablePos.x, availablePos.y].GetComponent<Water>().typeUnit = TypeUnit.Tile;
        map[availablePos.x, availablePos.y].GetComponent<Water>().tag = GameManager.TAG_WATER;
        map[availablePos.x, availablePos.y].GetComponent<Water>().number = 1;
        map[availablePos.x, availablePos.y].GetComponent<Water>().isBridge = isBridge;
        Sprite sprite = Resources.Load<Sprite>($"Tiles/{typeLocation}/Water1");
        map[availablePos.x, availablePos.y].GetComponent<SpriteRenderer>().sprite = sprite;
        return availablePos;
    }

    List<Vector2Int> CreateMines(Vector2Int maxPos, Vector2Int minPos, TypeLocation typeLocation, TypeMine typeMine,int number)
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        for (int i = 0; i < 4; i++)
        {
            Vector2Int availablePos = GetAvailablePos(maxPos, minPos);
            Destroy(map[availablePos.x, availablePos.y].GetComponent<VisualUnit>());
            map[availablePos.x, availablePos.y].AddComponent<Mine>();
            map[availablePos.x, availablePos.y].GetComponent<Mine>().x = availablePos.x;
            map[availablePos.x, availablePos.y].GetComponent<Mine>().y = availablePos.y;
            map[availablePos.x, availablePos.y].GetComponent<Mine>().typeLocation = typeLocation;
            map[availablePos.x, availablePos.y].GetComponent<Mine>().typeUnit = TypeUnit.Tile;
            map[availablePos.x, availablePos.y].GetComponent<Mine>().tag = GameManager.TAG_MINE;
            map[availablePos.x, availablePos.y].GetComponent<Mine>().number = number;
            map[availablePos.x, availablePos.y].GetComponent<Mine>().typeMine = typeMine;
            Sprite sprite = Resources.Load<Sprite>($"Tiles/{typeLocation}/Mine{number}");
            map[availablePos.x, availablePos.y].GetComponent<SpriteRenderer>().sprite = sprite;
            positions.Add(availablePos);
        }
        return positions;
    }

    void CreateTile(Vector2Int pos,TypeLocation typeLocation)
    {
        int randNum = Random.Range(1, 4);
        Vector2Int availablePos = pos;
        Destroy(map[availablePos.x, availablePos.y].GetComponent<VisualUnit>());
        map[availablePos.x, availablePos.y].AddComponent<Tile>();
        map[availablePos.x, availablePos.y].GetComponent<Tile>().x = availablePos.x;
        map[availablePos.x, availablePos.y].GetComponent<Tile>().y = availablePos.y;
        map[availablePos.x, availablePos.y].GetComponent<Tile>().typeLocation = typeLocation;
        map[availablePos.x, availablePos.y].GetComponent<Tile>().typeUnit = TypeUnit.Tile;
        map[availablePos.x, availablePos.y].GetComponent<Tile>().tag = GameManager.TAG_TILE;
        map[availablePos.x, availablePos.y].GetComponent<Tile>().number = randNum;
        Sprite sprite = Resources.Load<Sprite>($"Tiles/{typeLocation}/Tile{randNum}");
        map[availablePos.x, availablePos.y].GetComponent<SpriteRenderer>().sprite = sprite;
    }

    Vector2Int GetAvailablePos(Vector2Int maxPos, Vector2Int minPos)
    {
        int count = 0;
        while (count < 10000)
        {
            int randX = Random.Range(minPos.x, maxPos.x);
            int randY = Random.Range(minPos.y, maxPos.y);
            if(map[randX,randY].tag == GameManager.TAG_VISUAL_UNIT)
            {
                return new Vector2Int(randX, randY);
            }
            count++;
        }
        throw new System.Exception("Доступная позиция не найдена!");
    }

    void SaveMap()
    {
        ArrayList mapData = new ArrayList();
        for(int i=0;i < MAX_X; i++)
        {
            for(int j=0;j < MAX_Y; j++)
            {
                VisualUnit visualUnit = map[i, j].GetComponent<VisualUnit>();
                if(visualUnit.tag == GameManager.TAG_VISUAL_UNIT)
                {
                    VisualUnitData visualUnitData = new VisualUnitData()
                    {
                        x = i,
                        y=j,
                        typeLocation=visualUnit.typeLocation,
                        tag=visualUnit.tag,
                        typeUnit=visualUnit.typeUnit,
                        number=visualUnit.number
                    };
                    mapData.Add(visualUnitData);
                }else if(visualUnit.tag == GameManager.TAG_TILE)
                {
                    Tile tile = visualUnit.GetComponent<Tile>();
                    TileData tileData = new TileData()
                    {
                        x = i,
                        y = j,
                        typeLocation = tile.typeLocation,
                        tag = tile.tag,
                        typeUnit = tile.typeUnit,
                        number = tile.number
                    };
                    mapData.Add(tileData);
                }
                else if (visualUnit.tag == GameManager.TAG_WATER)
                {
                    Water water = visualUnit.GetComponent<Water>();
                    WaterData waterData = new WaterData()
                    {
                        x = i,
                        y = j,
                        typeLocation = water.typeLocation,
                        tag = water.tag,
                        typeUnit = water.typeUnit,
                        number = water.number,
                        isBridge= water.isBridge
                    };
                    mapData.Add(waterData);
                }
                else if (visualUnit.tag == GameManager.TAG_MINE)
                {
                    Mine mine = visualUnit.GetComponent<Mine>();
                    MineData mineData = new MineData()
                    {
                        x = i,
                        y = j,
                        typeLocation = mine.typeLocation,
                        tag = mine.tag,
                        typeUnit = mine.typeUnit,
                        number = mine.number,
                       typeMine =mine.typeMine
                    };
                    mapData.Add(mineData);
                }
                else if (visualUnit.tag == GameManager.TAG_TOWER)
                {
                    Tower tower = visualUnit.GetComponent<Tower>();
                    TowerData towerData = new TowerData()
                    {
                        x = i,
                        y = j,
                        typeLocation = tower.typeLocation,
                        tag = tower.tag,
                        typeUnit = tower.typeUnit,
                        number = tower.number,
                        maxLevel = tower.maxLevel,
                        currentLevel = tower.currentLevel,
                        territory = tower.Territory,
                        typeFlag = tower.TypeFlag,
                        isHealer = tower.isHealer
                    };
                    mapData.Add(towerData);
                }else if(visualUnit.tag == GameManager.TAG_HEALER)
                {
                    Healer healer = visualUnit.GetComponent<Healer>();
                    HealerData healerData = new HealerData()
                    {
                        x = i,
                        y = j,
                        typeLocation = healer.typeLocation,
                        tag = healer.tag,
                        typeUnit = healer.typeUnit,
                        number = healer.number
                    };
                    mapData.Add(healerData);
                }

            }
        }
        for(int i=0;i < characterDatas.Count; i++)
        {
            mapData.Add(characterDatas[i]);
        }
        GameManager.instance.xmlMapManager.SaveData(mapData);
    }
    void LoadMap()
    {
        ArrayList mapData = GameManager.instance.xmlMapManager.LoadData();
        for(int i=0;i < mapData.Count; i++)
        {
            VisualUnitData visualUnitData = (VisualUnitData)mapData[i];
            if(visualUnitData.typeUnit == TypeUnit.Character)
            {
                characterDatas.Add((CharacterData)visualUnitData);
            }
            else
            {
                if(visualUnitData.tag == GameManager.TAG_VISUAL_UNIT)
                {
                    map[visualUnitData.x, visualUnitData.y].AddComponent<VisualUnit>();
                }else if(visualUnitData.tag == GameManager.TAG_TILE)
                {
                    map[visualUnitData.x, visualUnitData.y].AddComponent<Tile>();
                }else if(visualUnitData.tag == GameManager.TAG_MINE)
                {
                    MineData mineData = (MineData)visualUnitData;
                    map[visualUnitData.x, visualUnitData.y].AddComponent<Mine>();
                    map[visualUnitData.x, visualUnitData.y].GetComponent<Mine>().typeMine = mineData.typeMine;
                }
                else if (visualUnitData.tag == GameManager.TAG_WATER)
                {
                    WaterData waterData = (WaterData)visualUnitData;
                    map[visualUnitData.x, visualUnitData.y].AddComponent<Water>();
                    map[visualUnitData.x, visualUnitData.y].GetComponent<Water>().isBridge = waterData.isBridge;
                }
                else if (visualUnitData.tag == GameManager.TAG_TOWER)
                {
                    TowerData towerData = (TowerData)visualUnitData;
                    map[visualUnitData.x, visualUnitData.y].AddComponent<Tower>();
                    map[visualUnitData.x, visualUnitData.y].GetComponent<Tower>().maxLevel = towerData.maxLevel;
                    map[visualUnitData.x, visualUnitData.y].GetComponent<Tower>().currentLevel = towerData.currentLevel;
                    map[visualUnitData.x, visualUnitData.y].GetComponent<Tower>().SetFlag(towerData.typeFlag);
                    map[visualUnitData.x, visualUnitData.y].GetComponent<Tower>().Territory = towerData.territory;
                    map[visualUnitData.x, visualUnitData.y].GetComponent<Tower>().isHealer = towerData.isHealer;
                }else if(visualUnitData.tag == GameManager.TAG_HEALER)
                {
                    map[visualUnitData.x, visualUnitData.y].AddComponent<Healer>();
                }
                map[visualUnitData.x, visualUnitData.y].GetComponent<VisualUnit>().x = visualUnitData.x;
                map[visualUnitData.x, visualUnitData.y].GetComponent<VisualUnit>().y = visualUnitData.y;
                map[visualUnitData.x, visualUnitData.y].GetComponent<VisualUnit>().typeLocation = visualUnitData.typeLocation;
                map[visualUnitData.x, visualUnitData.y].GetComponent<VisualUnit>().typeUnit = visualUnitData.typeUnit;
                map[visualUnitData.x, visualUnitData.y].GetComponent<VisualUnit>().number = visualUnitData.number;
                map[visualUnitData.x, visualUnitData.y].GetComponent<VisualUnit>().tag = visualUnitData.tag;
                Sprite sprite = Resources.Load<Sprite>($"Tiles/{visualUnitData.typeLocation}/{visualUnitData.tag}{visualUnitData.number}");
                map[visualUnitData.x, visualUnitData.y].GetComponent<SpriteRenderer>().sprite = sprite;
            }
        }
    }

    public void OpenLevels(Tower tower)
    {
        clickedTower = tower;
        GameManager.instance.ChangeStateWindow(levelsWindow);
    }

    public void PrepareToMission(int level)
    {
        WarriorsData warriorsData = GetWarriorsData(clickedTower.transform.position, clickedTower.typeLocation);
        MissionData missionData = new MissionData(warriorsData, clickedTower.typeLocation, new Vector2Int(clickedTower.x, clickedTower.y), level);
        StartCoroutine(StartMission(missionData));
    }

    IEnumerator StartMission(MissionData missionData)
    {
        StopSaver();
        SaveMap();
        gameDataManager.SaveData();
        ScreenManager.instance.OnBeganDifficultProcess();
        yield return null;
        ScreenManager.instance.LoadScreen(ScreenManager.FIGHT_MAP_SCREEN, missionData);
    }

    CharacterData GenerateCharacterData(TypeLocation typeLocation,int number,TypeCharacter typeCharacter,Vector2Int pos,float health,int speed)
    {
        CharacterData characterData = new CharacterData()
        {
            x=pos.x,
            y=pos.y,
            typeLocation=typeLocation,
            typeCharacter=typeCharacter,
            health=health,
            speed=speed,
            number=number,
            level=1,
            typeUnit= TypeUnit.Character
        };
        return characterData;
    }

    void AddCharacter(CharacterData characterData)
    {
        characterDatas.Add(characterData);
    }

    Vector2Int GetAvailablePosForHero(Vector2Int basePos)
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>()
        {
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(-1,-1),
            new Vector2Int(1,1),
            new Vector2Int(1,-1),
            new Vector2Int(-1,1),
            new Vector2Int(0,2),
            new Vector2Int(0,-2),
            new Vector2Int(2,0),
            new Vector2Int(-2,0),
            new Vector2Int(-2,-2),
            new Vector2Int(2,2),
            new Vector2Int(2,-2),
            new Vector2Int(-2,2),
        };

        for(int i=0; i < availablePositions.Count; i++)
        {
            int x = basePos.x + availablePositions[i].x;
            int y = basePos.y + availablePositions[i].y;
            if(map[x,y].tag == GameManager.TAG_TILE)
            {
                CharacterData characterData = GetCharacterDataWithPos(new Vector2Int(x, y));
                if(characterData == null)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        throw new System.Exception("Доступная позиция для героя не найдена!");
    }

    public CharacterData GetCharacterDataWithPos(Vector2Int pos)
    {
        for(int i=0;i < characterDatas.Count; i++)
        {
            if(characterDatas[i].x == pos.x && characterDatas[i].y == pos.y)
            {
                return characterDatas[i];
            }
        }
        return null;
    }

    public CharacterData GetCharacterDataWithPos(int x,int y)
    {
        for (int i = 0; i < characterDatas.Count; i++)
        {
            if (characterDatas[i].x == x && characterDatas[i].y == y)
            {
                return characterDatas[i];
            }
        }
        return null;
    }

    public void OnBecameVisibleTile(Tile tile)
    {
        CharacterData characterData = GetCharacterDataWithPos(tile.x, tile.y);
        if (characterData != null)
        {
            if (characterData.typeCharacter == TypeCharacter.Enemy)
            {
                GameObject enemy = CreateEnemy(characterData);
            }
        }
    }

    public void OnCreatedTile(Tile tile)
    {
        CharacterData characterData = GetCharacterDataWithPos(tile.x, tile.y);
        if(characterData != null)
        {
            if(characterData.typeCharacter == TypeCharacter.Hero)
            {
                GameObject hero = CreateHero(characterData);
                CameraController.instance.Move(hero.transform.position);
            }
        }
    }

    GameObject CreateHero(CharacterData characterData)
    {
        Vector3 pos = map[characterData.x, characterData.y].transform.position + PositionCalculator.additionPos;
        GameObject clon = Instantiate(heroPrefab, new Vector3(pos.x,pos.y,-2), new Quaternion(0,190,0,0), parent);
        clon.name = "Hero";
        clon.GetComponent<HeroGroup>().typeLocation = characterData.typeLocation;
        clon.GetComponent<HeroGroup>().typeCharacter = characterData.typeCharacter;
        clon.GetComponent<HeroGroup>().typeUnit = TypeUnit.Character;
        clon.GetComponent<HeroGroup>().number = characterData.number;
        clon.GetComponent<HeroGroup>().speed = characterData.speed;
        clon.GetComponent<HeroGroup>().level = characterData.level;
        clon.GetComponent<HeroGroup>().health = characterData.health;
        Sprite sprite = Resources.Load<Sprite>($"Warriors/{characterData.typeLocation}/{characterData.typeLocation}{characterData.number}");
        clon.GetComponent<SpriteRenderer>().sprite = sprite;
        clon.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        map[characterData.x, characterData.y].GetComponent<Tile>().SetCharacter(clon);
        AddActiveHero(clon);
        ScreenManager.instance.AddActiveObject(clon);
        return clon;
    }

    GameObject CreateEnemy(CharacterData characterData)
    {
        Vector3 pos = map[characterData.x, characterData.y].transform.position + PositionCalculator.additionPos;
        GameObject clon = Instantiate(enemyPrefab, new Vector3(pos.x,pos.y,2), Quaternion.identity, parent);
        clon.name = "Enemy";
        clon.GetComponent<EnemyGroup>().typeLocation = characterData.typeLocation;
        clon.GetComponent<EnemyGroup>().typeCharacter = characterData.typeCharacter;
        clon.GetComponent<EnemyGroup>().typeUnit = TypeUnit.Character;
        clon.GetComponent<EnemyGroup>().number = characterData.number;
        clon.GetComponent<EnemyGroup>().speed = characterData.speed;
        clon.GetComponent<EnemyGroup>().level = characterData.level;
        clon.GetComponent<EnemyGroup>().health = characterData.health;
        Sprite sprite = Resources.Load<Sprite>($"Warriors/{characterData.typeLocation}/{characterData.typeLocation}{characterData.number}");
        clon.GetComponent<SpriteRenderer>().sprite = sprite;
        clon.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        ScreenManager.instance.AddActiveObject(clon);
        map[characterData.x, characterData.y].GetComponent<Tile>().SetCharacter(clon);
        return clon;
    }

    public void OnClickedHero(GameObject hero)
    {
        if (!clickIsAvailable) return;
        if (isMoved) return;
        int speed;
        if (typeMove == TypeMove.Constraint)
        {
            speed = hero.GetComponent<Character>().speed;
            if (movedCharacters.IndexOf(hero) != -1) return;
        }
        else
        {
            speed = 7;
        }
        if(clickedHero != null)
        {
            if(clickedHero == hero)
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
            }
        }
        else
        {
            Vector3 realPos = PositionCalculator.GetPosTileFromPosCharacter(hero.transform.position);
            Collider2D[] cols = Physics2D.OverlapCircleAll(realPos, speed);
            currentHero = hero;
            clickedHero = hero;
            SelectTilesOfColliders(cols);
        }
    }

    void SelectTilesOfColliders(Collider2D[] cols)
    {
        for(int i=0; i < cols.Length; i++)
        {
            if(cols[i].tag == GameManager.TAG_TILE || (cols[i].tag == GameManager.TAG_WATER && cols[i].GetComponent<Water>().isBridge))
            {
                Tile tile = cols[i].GetComponent<Tile>();
                CharacterData characterData = GetCharacterDataWithPos(tile.x, tile.y);
                if (characterData != null && characterData.typeCharacter == TypeCharacter.Hero) continue;
                Vector3 realPos1 = PositionCalculator.GetPosTileFromPosCharacter(currentHero.transform.position);
                Vector2Int startPos = PositionCalculator.GetPosArrayFromRealPos(realPos1, map);
                Vector2Int endPos = PositionCalculator.GetPosArrayFromRealPos(tile.transform.position, map);
                List<Vector2Int> arrayPosToTarget = PositionCalculator.GetArrayPosToTarget(startPos, endPos, minPos,maxPos, map);
                if (arrayPosToTarget == null) continue;
                tile.Select();
                selectedTiles.Add(tile);
            }
        }
    }

    void UnSelectTiles()
    {
        for(int i=0;i < selectedTiles.Count; i++)
        {
            selectedTiles[i].Unselect();
        }
        selectedTiles = new List<Tile>();
    }

    bool AttackIsAvailable(Vector2Int startPos,Vector2Int endPos,float distance)
    {
        if (Vector2Int.Distance(startPos, endPos) <= distance)
        {
            return true;
        }
        return false;
    }

    public void OnClickedTile(Tile tile)
    {
        if (tile.isSelected)
        {
            Vector3 realPos = PositionCalculator.GetPosTileFromPosCharacter(currentHero.transform.position);
            Vector2Int startPos = PositionCalculator.GetPosArrayFromRealPos(realPos,map);
            Vector2Int endPos = PositionCalculator.GetPosArrayFromRealPos(tile.transform.position, map);
            float distance = 1f;        
            if (tile.character != null)
            {
                TypeLocation typeLocationEnemy = tile.character.GetComponent<Character>().typeLocation;
                if (AttackIsAvailable(startPos, endPos, distance))
                {
                    
                    PrepareToFight(tile.typeLocation, typeLocationEnemy, realPos, endPos);
                }
                else
                {
                    List<Vector2Int> bestArrayPos = PositionCalculator.GetBestArrayPosToTarget(startPos, endPos, minPos, maxPos, map);
                    bestArrayPos.RemoveAt(bestArrayPos.Count - 1);
                    
                    Vector2Int endPos2 = bestArrayPos[bestArrayPos.Count - 1];
                    PreparingFightData fightData = new PreparingFightData(tile.typeLocation, typeLocationEnemy, endPos, tile.transform.position);
                    List<Vector3> realPositions = PositionCalculator.GetRealPositionsFromArrayPos(bestArrayPos, map);
                    currentHero.GetComponent<HeroGroup>().fightData = fightData;
                    currentHero.GetComponent<IMove>().ArrayPos = realPositions;
                    currentHero.GetComponent<IMove>().StartMove();
                    map[startPos.x, startPos.y].GetComponent<Tile>().DeleteCharacter();
                    map[endPos2.x, endPos2.y].GetComponent<Tile>().SetCharacter(currentHero);
                    isMoved = true;
                    ChangeCharacterDataWithPos(startPos, endPos2);
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
                isMoved=true;
                ChangeCharacterDataWithPos(startPos, endPos);
            }
        }
        UnSelectTiles();
        clickedHero = null;
    }

    public void OnMovedHero(GameObject hero)
    {
        if (typeMove==TypeMove.Constraint)
        {
            movedCharacters.Add(hero);
            if(movedCharacters.Count == activeHeros.Count)
            {
                movedCharacters = new List<GameObject>();
                clickIsAvailable = false;
                ActivateEnemies();
            }
        }
        isMoved = false;
    }

    public void NextUnit()
    {
        if (!clickIsAvailable) return;
        if (typeMove == TypeMove.Free) return;
        GameObject hero = GetDontMovedHero();
        CameraController.instance.MoveLerp(hero.transform.position);
    }

    GameObject GetDontMovedHero()
    {
        for(int i=0;i < activeHeros.Count; i++)
        {
            if (movedCharacters.IndexOf(activeHeros[i]) == -1)
            {
                return activeHeros[i];
            }
        }
        return null;
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

    void ActivateEnemies()
    {
        for(int i=0;i < activeEnemies.Count; i++)
        {
            if(movedCharacters.IndexOf(activeEnemies[i]) == -1)
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
        if(nearHero == null)
        {
            OnMovedEnemy(enemy);   
            return;
        }
        Vector3 realPos2 = PositionCalculator.GetPosTileFromPosCharacter(nearHero.transform.position);
        Vector2Int endPos = PositionCalculator.GetPosArrayFromRealPos(realPos2, map);
        float distance = 1f;
        int speed = enemy.GetComponent<Character>().speed;
        TypeLocation typeLocation = map[startPos.x, startPos.y].GetComponent<Tile>().typeLocation;
        TypeLocation typeLocationEnemy = enemy.GetComponent<EnemyGroup>().typeLocation;
        if (AttackIsAvailable(startPos, endPos, distance))
        {
            PrepareToFight(typeLocation, typeLocationEnemy, realPos2, startPos);
        }
        else
        {
            List<Vector2Int> bestArrayPos = PositionCalculator.GetBestArrayPosToTarget(startPos, endPos, minPos, maxPos, map);
            if(bestArrayPos.Count <= speed)
            {
                bestArrayPos.RemoveAt(bestArrayPos.Count - 1);
                Vector2Int endPos2 = bestArrayPos[bestArrayPos.Count - 1];
                PreparingFightData preparingFightData = new PreparingFightData(typeLocation, typeLocationEnemy, endPos2, realPos2);
                List<Vector3> realPositions = PositionCalculator.GetRealPositionsFromArrayPos(bestArrayPos, map);
                enemy.GetComponent<EnemyGroup>().fightData = preparingFightData;
                enemy.GetComponent<IMove>().ArrayPos = realPositions;
                enemy.GetComponent<IMove>().StartMove();
                map[startPos.x, startPos.y].GetComponent<Tile>().DeleteCharacter();
                map[endPos2.x, endPos2.y].GetComponent<Tile>().SetCharacter(enemy);
                ChangeCharacterDataWithPos(startPos, endPos2);
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
                ChangeCharacterDataWithPos(startPos, endPos2);
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
            for(int i=0;i < heros.Count; i++)
            {
                Vector3 realPos1 = PositionCalculator.GetPosTileFromPosCharacter(heros[i].transform.position);
                Vector2Int endPos = PositionCalculator.GetPosArrayFromRealPos(realPos1, map);
                if(Vector2Int.Distance(startPos,endPos) < bestDistance)
                {
                    bestDistance = Vector2Int.Distance(startPos, endPos);
                    nearHero = heros[i];
                }
            }

            Vector3 realPos2 = PositionCalculator.GetPosTileFromPosCharacter(nearHero.transform.position);
            Vector2Int endPos2 = PositionCalculator.GetPosArrayFromRealPos(realPos2, map);
            List<Vector2Int> arrayPosToTarget = PositionCalculator.GetArrayPosToTarget(startPos, endPos2, minPos, maxPos, map);
            if(arrayPosToTarget == null)
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

    Vector2Int GetAvailablePosForEnemy(Vector2Int maxPos,Vector2Int minPos)
    {
        int count = 0;
        while (count < 10000)
        {
            int randX = Random.Range(minPos.x, maxPos.x);
            int randY = Random.Range(minPos.y, maxPos.y);
            if (map[randX, randY].tag == GameManager.TAG_TILE)
            {
                CharacterData characterData = GetCharacterDataWithPos(randX, randY);
                if(characterData == null)
                {
                    return new Vector2Int(randX, randY);
                }
            }
            count++;
        }
        throw new System.Exception("Доступная позиция не найдена!");
    }

    void CreateEnemy(TypeLocation typeLocation,Vector2Int maxPos,Vector2Int minPos)
    {
        Vector2Int availablePos = GetAvailablePosForEnemy(maxPos, minPos);
        int randNum = Random.Range(1, 8);
        CharacterData characterData = GenerateCharacterData(typeLocation, randNum, TypeCharacter.Enemy, availablePos, 40, 5);
        AddCharacter(characterData);
    }

    public void PrepareToFight(TypeLocation typeLocation,TypeLocation typeLocationEnemy,Vector3 attackPoint, Vector2Int posEnemy)
    {
        List<Vector2Int> arrayPosHeros = new List<Vector2Int>();
        Collider2D[] cols = Physics2D.OverlapCircleAll(attackPoint, 5);
        for(int i=0;i < cols.Length; i++)
        {
            if(cols[i].tag == GameManager.TAG_TILE || (cols[i].tag == GameManager.TAG_WATER))
            {
                Tile tile = cols[i].GetComponent<Tile>();
                CharacterData characterData = GetCharacterDataWithPos(tile.x, tile.y);
                if(characterData != null)
                {
                    if (characterData.typeCharacter == TypeCharacter.Hero)
                    {
                        arrayPosHeros.Add(new Vector2Int(tile.x, tile.y));
                    }
                }
            }
        }

        WarriorsData warriorsData = GetWarriorsData(attackPoint, typeLocationEnemy);
        FightData fightData = new FightData(typeLocation, warriorsData, arrayPosHeros, posEnemy);
        StartCoroutine(StartFight(fightData));
    }

    public void PrepareToFight(PreparingFightData preparingFightData)
    {
        List<Vector2Int> arrayPosHeros = new List<Vector2Int>();
        Collider2D[] cols = Physics2D.OverlapCircleAll(preparingFightData.attackPoint, 5);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].tag == GameManager.TAG_TILE || (cols[i].tag == GameManager.TAG_WATER))
            {
                Tile tile = cols[i].GetComponent<Tile>();
                CharacterData characterData = GetCharacterDataWithPos(tile.x, tile.y);
                if (characterData != null)
                {
                    if (characterData.typeCharacter == TypeCharacter.Hero)
                    {
                        arrayPosHeros.Add(new Vector2Int(tile.x, tile.y));
                    }
                }
            }
        }

        WarriorsData warriorsData = GetWarriorsData(preparingFightData.attackPoint, preparingFightData.typeLocationEnemy);
        FightData fightData = new FightData(preparingFightData.typeLocation, warriorsData, arrayPosHeros, preparingFightData.posEnemy);
        StartCoroutine(StartFight(fightData));
    }

    IEnumerator StartFight(FightData fightData)
    {
        StopSaver();
        SaveMap();
        gameDataManager.SaveData();
        ScreenManager.instance.OnBeganDifficultProcess();
        yield return null;
        ScreenManager.instance.LoadScreen(ScreenManager.FIGHT_MAP_SCREEN, fightData);
    }

    void StopSaver()
    {
        if(saver != null)
        {
            StopCoroutine(saver);
        }
    }

    WarriorsData GetWarriorsData(Vector3 attackPoint,TypeLocation typeLocationEnemy)
    {
        List<WarriorData> heros = new List<WarriorData>();
        List<WarriorData> enemies = new List<WarriorData>();
        Collider2D[] cols = Physics2D.OverlapCircleAll(attackPoint, 5);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].tag == GameManager.TAG_TILE || (cols[i].tag == GameManager.TAG_WATER))
            {
                Tile tile = cols[i].GetComponent<Tile>();
                CharacterData characterData = GetCharacterDataWithPos(tile.x, tile.y);
                if (characterData != null && characterData.typeCharacter == TypeCharacter.Hero)
                {
                    WarriorData warriorData = GameManager.instance.xmlWarriorDataManager.GetWarriorData(characterData.typeLocation,characterData.number);
                    warriorData.level = characterData.level;
                    warriorData.health = characterData.health;
                    warriorData.x = characterData.x;
                    warriorData.y = characterData.y;
                    if (heros.Count == 3) break;
                    heros.Add(warriorData);
                }
            }
        }

        int randCount = Random.Range(heros.Count, heros.Count + 1);
        
        for(int i=0;i < randCount; i++)
        {
            int randNumber = Random.Range(1, 8);
            float health = Random.Range(40, 60);
            WarriorData warriorData = GameManager.instance.xmlWarriorDataManager.GetWarriorData(typeLocationEnemy, randNumber);
            warriorData.level = 1;
            warriorData.health = health;
            if (enemies.Count == 3) break;
            enemies.Add(warriorData);
        }

        WarriorsData warriorsData = new WarriorsData(heros, enemies);
        return warriorsData;
    }

    void ChangeCharacterDataWithPos(Vector2Int pos,Vector2Int newPos)
    {
        for(int i=0;i < characterDatas.Count; i++)
        {
            if(characterDatas[i].x == pos.x && characterDatas[i].y == pos.y)
            {
                characterDatas[i].x = newPos.x;
                characterDatas[i].y = newPos.y;
            }
        }
    }

    IEnumerator StartSaving()
    {
        while (true)
        {
            SaveMap();
            gameDataManager.SaveData();
            yield return new WaitForSeconds(5f);
        }
    }

    public void ComeToMenuScreen()
    {
        StartCoroutine(StartComingToMenuScreen());
    }

    IEnumerator StartComingToMenuScreen()
    {
        StopSaver();
        SaveMap();
        gameDataManager.SaveData();
        ScreenManager.instance.OnBeganDifficultProcess();
        yield return null;
        ScreenManager.instance.LoadScreen(ScreenManager.MENU_SCREEN);
    }

    public void BuyHealer(Vector2Int basePos)
    {
        if (gameDataManager.Stone - 15 >= 0)
        {
            Vector2Int availablePos = GetAvailablePosForHero(basePos);
            Tile tile = map[availablePos.x, availablePos.y].GetComponent<Tile>();
            map[availablePos.x, availablePos.y].AddComponent<Healer>();
            map[availablePos.x, availablePos.y].GetComponent<Healer>().typeLocation = tile.typeLocation;
            map[availablePos.x, availablePos.y].GetComponent<Healer>().typeUnit = tile.typeUnit;
            map[availablePos.x, availablePos.y].GetComponent<Healer>().x = tile.x;
            map[availablePos.x, availablePos.y].GetComponent<Healer>().y = tile.y;
            map[availablePos.x, availablePos.y].GetComponent<Healer>().number = 1;
            map[availablePos.x, availablePos.y].GetComponent<Healer>().tag = GameManager.TAG_HEALER;
            Sprite sprite = Resources.Load<Sprite>($"Tiles/{tile.typeLocation}/Healer1");
            map[availablePos.x, availablePos.y].GetComponent<SpriteRenderer>().sprite = sprite;
            gameDataManager.ChangeStone(Operation.Reduce, 15);
            Destroy(tile);
        }
    }

    public void BuyHero(int number, int cost, int speed)
    {
        if (gameDataManager.Money - cost >= 0)
        {
            if (GetAmountHeros() >= 5) return;
            Vector2Int posHero = GetAvailablePosForHero(gameDataManager.PosTower);
            CharacterData heroData = GenerateCharacterData(TypeLocation.Forest, number, TypeCharacter.Hero, posHero, 52, speed);
            AddCharacter(heroData);
            CreateHero(heroData);
            gameDataManager.ChangeMoney(Operation.Reduce, cost);
            UpdateIndicator();
            GameUI.instance.ReduceMoney(cost);
        }
    }

    void UpdateIndicator()
    {
        GameUI.instance.ChangeIndicator(GetAmountHeros());
    }

    public int GetAmountHeros()
    {
        int count = 0;
        for(int i=0;i < characterDatas.Count; i++)
        {
            if(characterDatas[i].typeCharacter == TypeCharacter.Hero)
            {
                count++;
            }
        }
        return count;
    }

    public void OpenUpgradeWindow()
    {
        GameManager.instance.ChangeStateWindow(upgradeWindow);
    }
}
