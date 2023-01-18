using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XmlDefaultSettingsDataManager : XmlManager<SettingsData>
{
    public override string Path { get; set; } = Application.dataPath + "/Xml/DefaultSettings.xml";
}
