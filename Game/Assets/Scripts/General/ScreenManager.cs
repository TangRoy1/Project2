using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    private string activeScreen = MENU_SCREEN;
    public const string MENU_SCREEN = "MenuScreen";
    public const string GLOBAL_MAP_SCREEN = "GlobalMapScreen";
    public const string FIGHT_MAP_SCREEN = "FightMapScreen";
    List<GameObject> openedWindows;
    List<GameObject> activeObjects;
    [SerializeField]
    GameObject menuScreen;
    [SerializeField]
    GameObject globalMapScreen;
    [SerializeField]
    GameObject fightMapScreen;
    [SerializeField]
    GameObject loadingScreen;

    private void Awake()
    {
        openedWindows = new List<GameObject>();
        activeObjects = new List<GameObject>();
        instance = this;
    }

    public void AddOpenedWindow(GameObject window)
    {
        openedWindows.Add(window);
    }

    public void RemoveOpenedWindow(GameObject window)
    {
        openedWindows.Remove(window);
    }

    public void AddActiveObject(GameObject activeObject)
    {
        activeObjects.Add(activeObject);
    }

    public void RemoveActiveObject(GameObject activeObject)
    {
        activeObjects.Remove(activeObject);
    }

    void ActiveScreen(string name, object obj=null)
    {
        if (name == MENU_SCREEN)
        {
            menuScreen.SetActive(true);
            MenuManager.instance.StartScript();
        }
        else if (name == GLOBAL_MAP_SCREEN)
        {
            CameraController.instance.SetSize(5f);
            globalMapScreen.SetActive(true);
            GlobalMapManager.instance.StartScript();
            GameUI.instance.StartScript();
        }
        else if (name == FIGHT_MAP_SCREEN)
        {
            fightMapScreen.SetActive(true);
            CameraController.instance.SetSize(7.1f);
            if ((obj as FightData) != null)
            {
                FightManager.instance.StartScript((FightData)obj);
            }
            else
            {
                FightManager.instance.StartScript((MissionData)obj);
            }
            GameUI.instance.StartScript();
            CameraController.instance.Move(Vector3.zero);
        }
    }

    void DeactiveScreen(string name)
    {
        if (name == MENU_SCREEN)
        {
            menuScreen.SetActive(false);
        }
        else if (name == GLOBAL_MAP_SCREEN)
        {
            globalMapScreen.SetActive(false);
        }
        else if (name == FIGHT_MAP_SCREEN)
        {
            fightMapScreen.SetActive(false);
        }
    }

    void CloseOpenedWindows()
    {
        for(int i=0;i < openedWindows.Count; i++)
        {
            openedWindows[i].SetActive(false);
        }
        openedWindows = new List<GameObject>();
    }

    void DestroyAllActiveObjects()
    {
        for (int i = 0; i < activeObjects.Count; i++)
        {
            Destroy(activeObjects[i]);
        }
        activeObjects = new List<GameObject>();

    }

   void ChangeActiveScreen(string name)
    {
        activeScreen = name;
    }

    public void LoadScreen(string name)
    {
        CloseOpenedWindows();
        DestroyAllActiveObjects();
        DeactiveScreen(activeScreen);
        ChangeActiveScreen(name);
        ActiveScreen(name);
    }

    public void LoadScreen(string name,object obj)
    {
        CloseOpenedWindows();
        DestroyAllActiveObjects();
        DeactiveScreen(activeScreen);
        ChangeActiveScreen(name);
        ActiveScreen(name,obj);
    }

    public void OnBeganDifficultProcess()
    {
        loadingScreen.SetActive(true);
    }

    public void OnEndedDifficultProcess()
    {
        loadingScreen.SetActive(false);
    }

    public string GetNameActiveScreen()
    {
        return activeScreen;
    }
}
