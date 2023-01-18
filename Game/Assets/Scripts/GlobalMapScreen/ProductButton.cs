using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductButton : MonoBehaviour,IPointerClickHandler
{
    [HideInInspector]
    public int prize;

    public void OnPointerClick(PointerEventData eventData)
    {
        Shop.instance.BuyProduct(prize);
    }
}
