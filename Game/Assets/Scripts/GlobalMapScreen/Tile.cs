using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : VisualUnit
{
    public GameObject character;
    public bool isSelected = false;
    Color dark = new Color(0.5f, 0.5f, 0.5f);
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GameManager.instance.GetActionHandler().OnCreatedTile(this);
    }

    public void SetCharacter(GameObject character)
    {
        this.character = character;
    }

    public void DeleteCharacter()
    {
        this.character = null;
    }

    public void Select()
    {
        spriteRenderer.color = dark;
        isSelected = true;
    }

    public void Unselect()
    {
        spriteRenderer.color = Color.white;
        isSelected = false;
    }

    private void OnMouseUpAsButton()
    {
        GameManager.instance.GetActionHandler().OnClickedTile(this);
    }

    private void OnBecameVisible()
    {
        GameManager.instance.GetActionHandler().OnBecameVisibleTile(this);
    }
}
