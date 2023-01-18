using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XmlGameDataManager : XmlManager<Store<int,GameData>>
{
    public override string Path { get; set; } = Application.dataPath + "/Xml/GameData.xml";

    public void SaveData(GameData data)
    {
        Store<int, GameData> datas;

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
            datas = new Store<int, GameData>();
            datas.AddData(MySqlManager.userId, data);
        }
        base.SaveData(datas);
    }

    public GameData LoadData()
    {
        Store<int, GameData> datas = base.LoadData();
        return datas[MySqlManager.userId];
    }

    public bool ExistsData()
    {
        if (base.ExistsData())
        {
            Store<int, GameData> datas = base.LoadData();
            if (datas.ContainsKey(MySqlManager.userId))
            {
                return true;
            }
        }
        return false;
    }

    public void DeleteData(int key)
    {
        Store<int, GameData> datas = base.LoadData();
        datas.DeleteKey(key);
        base.SaveData(datas);
    }
}
