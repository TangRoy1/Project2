using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XmlWarriorDataManager : XmlManager<List<WarriorData>>
{
    public override string Path { get; set; } = Application.dataPath + "/Xml/WarriorData.xml";

    public WarriorData GetWarriorData(TypeLocation typeLocation, int number)
    {
        List<WarriorData> warriorDatas = LoadData();
        for(int i=0;i < warriorDatas.Count; i++)
        {
            if(warriorDatas[i].typeLocation == typeLocation && warriorDatas[i].number == number)
            {
                return warriorDatas[i];
            }
        }
        return null;
    }
}
