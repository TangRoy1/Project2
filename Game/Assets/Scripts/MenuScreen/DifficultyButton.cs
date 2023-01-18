using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DifficultyButton : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]
    TypeDifficulty typeDifficulty;

    public void OnPointerClick(PointerEventData eventData)
    {
        MenuManager.instance.ChooseDifficulty(typeDifficulty);
    }
}
