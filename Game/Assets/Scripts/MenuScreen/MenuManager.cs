using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void StartScript()
    {
        ScreenManager.instance.OnEndedDifficultProcess();
    }

    public void ChooseDifficulty(TypeDifficulty typeDifficulty)
    {
        if (MySqlManager.userId == -1)
        {
            if (GameManager.instance.xmlMapManager.ExistsData())
            {
                GameManager.instance.xmlMapManager.DeleteData(MySqlManager.userId);
                GameManager.instance.xmlGameDataManager.DeleteData(MySqlManager.userId);
            }
        }
        GameManager.instance.ChooseDifficulty(typeDifficulty);
        StartCoroutine(StartComingToGlobalMap());
    }

    IEnumerator StartComingToGlobalMap()
    {
        ScreenManager.instance.OnBeganDifficultProcess();
        yield return null;
        ScreenManager.instance.LoadScreen(ScreenManager.GLOBAL_MAP_SCREEN);
    }
}
