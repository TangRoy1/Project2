using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionHandler
{
    void OnBecameVisibleTile(Tile tile);
    void OnCreatedTile(Tile tile);
    void OnClickedHero(GameObject hero);
    void OnClickedTile(Tile tile);
    void OnMovedHero(GameObject hero);
    void OnMovedEnemy(GameObject enemy);
}
