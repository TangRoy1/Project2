using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XmlShopManager : XmlManager<List<ProductData>>
{
    public override string Path { get; set; } = Application.dataPath + "/Xml/Shop_Moscow.xml";
}
