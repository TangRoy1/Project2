using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualUnit : MonoBehaviour
{
    public int x;
    public int y;
    public int number;
    public TypeLocation typeLocation;
    public TypeUnit typeUnit;

    public void ChangeVisualUnit(TypeLocation typeLocation)
    {
        this.typeLocation = typeLocation;
        Sprite sprite = Resources.Load<Sprite>($"Tiles/{typeLocation}/{tag}{number}");
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
