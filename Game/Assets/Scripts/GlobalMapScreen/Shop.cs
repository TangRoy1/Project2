using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;
    [SerializeField]
    List<Transform> products;
    [SerializeField]
    Text crystalsText;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        LoadProducts();
        GameManager.instance.gameDataManager.OnChangedGameData += OnChangedGameData;
    }

    void OnChangedGameData()
    {
        crystalsText.text = GameManager.instance.gameDataManager.Crystals.ToString();
    }

    void LoadProducts()
    {
        List<ProductData> productDatas = GameManager.instance.xmlShopManager.LoadData();
        for(int i=0;i < products.Count; i++)
        {
            products[i].GetChild(0).GetComponent<Text>().text = $"+{productDatas[i].prize} CRYSTALS";
            products[i].GetChild(2).GetChild(0).GetComponent<Text>().text = productDatas[i].price + "$";
            products[i].GetChild(2).GetComponent<ProductButton>().prize = productDatas[i].prize;
        }
    }

    public void BuyProduct(int prize)
    {
        GameManager.instance.gameDataManager.ChangeCrystals(Operation.Add, prize);
    }
}
