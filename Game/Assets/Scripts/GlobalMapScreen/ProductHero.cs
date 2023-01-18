using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductHero : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]
    int number;
    [SerializeField]
    int cost;
    [SerializeField]
    int speed;

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalMapManager.instance.BuyHero(number, cost, speed);
    }
}
