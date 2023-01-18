using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    Text money;
    Text crystals;
    Text stone;
    Transform indicator;
    [SerializeField]
    GameObject cost;
    [SerializeField]
    Transform parent;
    [SerializeField]
    Transform startPos;
    private GameDataManager gameDataManager { get => GameManager.instance.gameDataManager; }

    private void Awake()
    {
        instance = this;
    }

    public void SetDefaultValues()
    {
        money = GameObject.Find("Money").GetComponent<Text>();
        crystals = GameObject.Find("Crystals").GetComponent<Text>();
        stone = GameObject.Find("Stone").GetComponent<Text>();
        indicator = GameObject.Find("Indicator").transform;
    }

    public void StartScript()
    {
        SetDefaultValues();
        gameDataManager.OnChangedGameData += OnGameDataChanged;
        if (ScreenManager.instance.GetNameActiveScreen() == ScreenManager.GLOBAL_MAP_SCREEN)
        {
            ChangeIndicator(GlobalMapManager.instance.GetAmountHeros());
        }
    }

    public void OnGameDataChanged()
    {
        GameDataManager gameDataManager = GameManager.instance.gameDataManager;
        ChangeCrystals(gameDataManager.Crystals);
        ChangeMoney(gameDataManager.Money);
        ChangeStone(gameDataManager.Stone);
    }

    public void ChangeMoney(int value)
    {
        money.text = value.ToString();
    }

    public void ChangeCrystals(int value)
    {
        crystals.text = value.ToString();
    }

    public void ChangeStone(int value)
    {
        stone.text = value.ToString();
    }

    public void ChangeIndicator(int amount)
    {
        for(int i=0;i < indicator.childCount; i++)
        {
            if (i + 1 <= amount)
            {
                indicator.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                indicator.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void ReduceMoney(int value)
    {
        GameObject clon = Instantiate(cost, Vector3.zero, Quaternion.identity, parent);
        clon.GetComponent<Text>().text = "-" + value;
        clon.GetComponent<RectTransform>().anchoredPosition = startPos.GetComponent<RectTransform>().anchoredPosition;
        Vector2 pos = startPos.GetComponent<RectTransform>().anchoredPosition;
        Vector2 endPos = new Vector2(pos.x, pos.y-104);
        StartCoroutine(StartCoroutineMove(clon, endPos));
    }

    IEnumerator StartCoroutineMove(GameObject object1, Vector2 endPos)
    {
        RectTransform rect = object1.GetComponent<RectTransform>();
        while (Vector2.Distance(rect.anchoredPosition, endPos) > 0.0001f)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, endPos, 5f);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(object1);
    }
}
