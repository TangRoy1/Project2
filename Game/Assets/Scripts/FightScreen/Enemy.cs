using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Warrior
{

    private void Start()
    {
        maxHealth = health;
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
            if (target != null)
            {
                Attack(target);
                target = null;
            }
            GameManager.instance.GetActionHandler().OnMovedEnemy(gameObject);
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
        if (health <= 0)
        {
            FightManager.instance.RemoveEnemy(gameObject);
            FightManager.instance.CreateRip(transform.position);
            Destroy(gameObject);
        }
    }

    public override void Attack(Warrior warrior)
    {
        float minDamage = level * 2;
        float maxDamage = minDamage + minDamage * 0.1f;
        float damage = Random.Range(minDamage, maxDamage)*GameManager.instance.GetPercentDamage();
        warrior.TakeHealth(damage);
    }
}
