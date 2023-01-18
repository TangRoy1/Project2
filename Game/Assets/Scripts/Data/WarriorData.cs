using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public class WarriorData
{
    public TypeLocation typeLocation;
    public int number;
    public float distance;
    public int speed;
    [XmlIgnore]
    public float health;
    [XmlIgnore]
    public int level;
    [XmlIgnore]
    public int x;
    [XmlIgnore]
    public int y;
}
