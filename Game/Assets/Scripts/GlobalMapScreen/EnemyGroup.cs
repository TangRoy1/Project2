using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : CharacterGroup
{
    public PreparingFightData fightData;

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
            if (fightData != null)
            {
                GlobalMapManager.instance.PrepareToFight(fightData);
            }
            else
            {
                GameManager.instance.GetActionHandler().OnMovedEnemy(gameObject);
            }
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            coroutineMove = StartCoroutine(StartCoroutineMove());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GlobalMapManager.instance.AddActiveEnemy(gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GlobalMapManager.instance.RemoveActiveEnemy(gameObject);
    }
}
