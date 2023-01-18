using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public XmlSettingsDataManager xmlSettingsDataManager;
    public XmlDefaultSettingsDataManager xmlDefaultSettingsDataManager;
    public XmlMapManager xmlMapManager;
    public XmlGameDataManager xmlGameDataManager;
    public XmlWarriorDataManager xmlWarriorDataManager;
    public XmlDefaultGameDataManager xmlDefaultGameDataManager;
    public XmlShopManager xmlShopManager;
    [HideInInspector]
    public GameDataManager gameDataManager;
    private TypeDifficulty typeDifficulty;
    public const string TAG_VISUAL_UNIT = "VisualUnit";
    public const string TAG_TILE = "Tile";
    public const string TAG_TOWER = "Tower";
    public const string TAG_MINE = "Mine";
    public const string TAG_WATER = "Water";
    public const string TAG_HEALER = "Healer";

    private void Awake()
    {
        instance = this;
        MySqlManager.CreateConnection();
        xmlSettingsDataManager = new XmlSettingsDataManager();
        xmlDefaultSettingsDataManager = new XmlDefaultSettingsDataManager();
        xmlMapManager = new XmlMapManager();
        gameDataManager = new GameDataManager();
        xmlGameDataManager = new XmlGameDataManager();
        xmlWarriorDataManager = new XmlWarriorDataManager();
        xmlDefaultGameDataManager = new XmlDefaultGameDataManager();
        xmlShopManager = new XmlShopManager();
    }

    public void ChangeStateWindow(GameObject window)
    {
        if (window.activeSelf)
        {
            window.SetActive(false);
            ScreenManager.instance.RemoveOpenedWindow(window);
        }
        else
        {
            window.SetActive(true);
            ScreenManager.instance.AddOpenedWindow(window);
        }
    }

    public IActionHandler GetActionHandler()
    {
        if(ScreenManager.instance.GetNameActiveScreen() == ScreenManager.GLOBAL_MAP_SCREEN)
        {
            return GlobalMapManager.instance;
        }
        else
        {
            return FightManager.instance;
        }
    }
    public void ChooseDifficulty(TypeDifficulty typeDifficulty)
    {
        this.typeDifficulty = typeDifficulty;
    }

    public float GetPercentDamage()
    {
        if(typeDifficulty == TypeDifficulty.Easy)
        {
            return 0.5f;
        }
        else if(typeDifficulty == TypeDifficulty.Normal)
        {
            return 1f;
        }else if(typeDifficulty == TypeDifficulty.Hard)
        {
            return 1.2f;
        }
        return 1f;
    }
}
