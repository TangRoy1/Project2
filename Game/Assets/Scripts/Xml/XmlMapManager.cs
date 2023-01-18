using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;

public class XmlMapManager : XmlManager<Store<int,ArrayList>>
{
    public override string Path { get; set; } = Application.dataPath + "/Xml/Map.xml";

    public XmlMapManager()
    {
        Type[] types = new Type[]
        {
            typeof(VisualUnitData),
            typeof(TileData),
            typeof(TowerData),
            typeof(WaterData),
            typeof(TowerData),
            typeof(MineData),
            typeof(CharacterData),
            typeof(HealerData)
        };
        xmlSerializer = new XmlSerializer(typeof(Store<int, ArrayList>), types);
    }

    public void SaveData(ArrayList data)
    {
        Store<int, ArrayList> datas;

        if (base.ExistsData())
        {
            datas = base.LoadData();
            if (ExistsData())
            {
                datas[MySqlManager.userId] = data;
            }
            else
            {
                datas.AddData(MySqlManager.userId, data);
            }
        }
        else
        {
            datas = new Store<int, ArrayList>();
            datas.AddData(MySqlManager.userId, data);
        }
        base.SaveData(datas);
    }

    public ArrayList LoadData()
    {
        Store<int, ArrayList> datas = base.LoadData();
        return datas[MySqlManager.userId];
    }

    public bool ExistsData()
    {
        if (base.ExistsData())
        {
            Store<int, ArrayList> datas = base.LoadData();
            if (datas.ContainsKey(MySqlManager.userId))
            {
                return true;
            }
        }
        return false;
    }

    public void DeleteCharacterWithPos(Vector2Int pos)
    {
        ArrayList map = LoadData();
        for(int i=0;i < map.Count; i++)
        {
            VisualUnitData visualUnitData = (VisualUnitData)map[i];
            if(visualUnitData.typeUnit == TypeUnit.Character)
            {
                if(visualUnitData.x == pos.x && visualUnitData.y == pos.y)
                {
                    map.RemoveAt(i);
                }
            }
        }
        SaveData(map);
    }

    public void DeleteCharacterWithPos(List<Vector2Int> arrayPos)
    {
        ArrayList map = LoadData();
        for (int k = 0; k < arrayPos.Count; k++)
        {
            for (int i = 0; i < map.Count; i++)
            {
                VisualUnitData visualUnitData = (VisualUnitData)map[i];
                if (visualUnitData.typeUnit == TypeUnit.Character)
                {
                    if (visualUnitData.x == arrayPos[k].x && visualUnitData.y == arrayPos[k].y)
                    {
                        map.RemoveAt(i);
                    }
                }
            }
        }
        SaveData(map);
    }

    public void ChangeCharacterDataWithPos(Vector2Int pos, float health)
    {
        ArrayList map = LoadData();

        for (int i = 0; i < map.Count; i++)
        {
            VisualUnitData visualUnitData = (VisualUnitData)map[i];
            if (visualUnitData.typeUnit == TypeUnit.Character)
            {
                if (visualUnitData.x == pos.x && visualUnitData.y == pos.y)
                {
                    CharacterData characterData = (CharacterData)visualUnitData;
                    characterData.health = health;
                }
            }
        }

        SaveData(map);
    }

    public void ChangeTowerDataWithPos(Vector2Int pos, int nextLevel)
    {
        ArrayList map = LoadData();

        for (int i = 0; i < map.Count; i++)
        {
            VisualUnitData visualUnitData = (VisualUnitData)map[i];
            if(visualUnitData.tag == GameManager.TAG_TOWER)
            {
                TowerData towerData = (TowerData)visualUnitData;
               if(towerData.currentLevel < nextLevel)
                {
                    towerData.currentLevel = nextLevel;
                }
            }
        }

        SaveData(map);
    }

    public void DeleteData(int key)
    {
        Store<int, ArrayList> datas = base.LoadData();
        datas.DeleteKey(key);
        base.SaveData(datas);
    }
}
