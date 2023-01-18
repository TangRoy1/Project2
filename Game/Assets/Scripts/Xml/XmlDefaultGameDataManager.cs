using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XmlDefaultGameDataManager : XmlManager<GameData>
{
    public override string Path { get; set; } = Application.dataPath + "/Xml/DefaultGameData.xml";
}
