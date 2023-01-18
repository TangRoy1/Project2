using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XmlSettingsDataManager : XmlManager<SettingsData>
{
    public override string Path { get; set; } = Application.dataPath + "/Xml/Settings.xml";
}
