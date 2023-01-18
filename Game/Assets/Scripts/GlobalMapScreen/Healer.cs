using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer :VisualUnit
{
    List<HeroGroup> activeHeros;
    float additionHp = 0.1f;
    Coroutine increaser;

    private void Awake()
    {
        activeHeros = new List<HeroGroup>();
    }

    private void Start()
    {
        ActivateCollider();
    }

    void ActivateCollider()
    {
        Destroy(GetComponent<BoxCollider2D>());
        CircleCollider2D collider =  gameObject.AddComponent<CircleCollider2D>();
        collider.radius = 3f;
        
    }

    IEnumerator StartIncreasing()
    {
        while (activeHeros.Count > 0)
        {
            for(int i=0;i < activeHeros.Count; i++)
            {
                activeHeros[i].IncreaseHp(additionHp);
            }
            yield return new WaitForSeconds(1f);
        }
        increaser = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        VisualUnit visualUnit = (VisualUnit)collision.gameObject.GetComponent<VisualUnit>();
        if(visualUnit.typeUnit == TypeUnit.Character)
        {
            Character character = visualUnit.GetComponent<Character>();
            if(character.typeCharacter == TypeCharacter.Hero)
            {
                activeHeros.Add(character.GetComponent<HeroGroup>());
                if(increaser == null)
                {
                    increaser = StartCoroutine(StartIncreasing());
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        VisualUnit visualUnit = (VisualUnit)collision.gameObject.GetComponent<VisualUnit>();
        if (visualUnit.typeUnit == TypeUnit.Character)
        {
            Character character = visualUnit.GetComponent<Character>();
            if (character.typeCharacter == TypeCharacter.Hero)
            {
                activeHeros.Remove(character.GetComponent<HeroGroup>());
            }
        }
    }
}
