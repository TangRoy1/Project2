using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorsData
{
    public List<WarriorData> heros;
    public List<WarriorData> enemies;

    public WarriorsData(List<WarriorData> heros, List<WarriorData> enemies)
    {
        this.heros = heros;
        this.enemies = enemies;
    }
}
