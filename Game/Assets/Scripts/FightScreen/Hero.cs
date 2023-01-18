using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Warrior
{
    private void Start()
    {
        maxHealth = 50 + 2 * level;
    }

    protected override IEnumerator StartCoroutineMove()
    {
        while (Vector3.Distance(rb2D.position, _arrayPos[posId] + PositionCalculator.additionPos) > 0.00001f)
        {
            rb2D.MovePosition(Vector2.MoveTowards(rb2D.position, _arrayPos[posId] + PositionCalculator.additionPos, 0.3f));
            yield return new WaitForSeconds(0.001f);
        }

        posId++;
        if (posId >= _arrayPos.Count)
        {
            coroutineMove = null;
            if(target != null)
            {
                Attack(target);
                target = null;
            }
            GameManager.instance.GetActionHandler().OnMovedHero(gameObject);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            coroutineMove = StartCoroutine(StartCoroutineMove());
        }
    }

    public override void TakeHealth(float value)
    {
        base.TakeHealth(value);
        if(health <= 0)
        {
            FightManager.instance.RemoveHero(gameObject);
            FightManager.instance.CreateRip(transform.position);
            Destroy(gameObject);
        }
    }

    private void OnMouseUpAsButton()
    {
        GameManager.instance.GetActionHandler().OnClickedHero(gameObject);
    }
}
