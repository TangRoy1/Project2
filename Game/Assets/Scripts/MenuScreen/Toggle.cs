using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Toggle : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]
    Sprite spriteOn;
    [SerializeField]
    Sprite spriteOff;
    State state = State.On;
    [SerializeField]
    TypeToggle typeToggle;
    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        if(typeToggle == TypeToggle.Music)
        {
            SettingsManager.instance.OnChangedMusic += ChangeState;
        }
        else
        {
            SettingsManager.instance.OnChangedSound += ChangeState;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(state == State.On)
        {
            ChangeState(State.Off);
        }
        else
        {
            ChangeState(State.On);
        }
        if (typeToggle == TypeToggle.Music)
        {
            SettingsManager.instance.ChangeMusic(state);
        }
        else
        {
            SettingsManager.instance.ChangeSound(state);
        }
    }

    public void ChangeState(State newState)
    {
        state = newState;
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if(state == State.Off)
        {
            image.sprite = spriteOff;
        }
        else
        {
            image.sprite = spriteOn;
        }
    }
}
