using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Level : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]
    int number;
    CanvasGroup canvasGroup;
    Image image;
    State state;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (state == State.On)
        {
            GlobalMapManager.instance.PrepareToMission(number);
        }
    }

    private void OnEnable()
    {
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        Tower tower = GlobalMapManager.instance.clickedTower;
        if(tower.maxLevel >= number)
        {
            ChangeState(State.On);
            canvasGroup.alpha = 1f;

            if(tower.currentLevel+1 >= number)
            {
                image.color = Color.white;
            }
            else
            {
                image.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
        else
        {
            ChangeState(State.Off);
            canvasGroup.alpha = 0.5f;
        }
    }

    public void ChangeState(State state)
    {
        this.state = state;
    }
}
