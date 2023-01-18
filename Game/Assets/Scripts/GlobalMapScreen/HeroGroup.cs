using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroGroup : CharacterGroup
{
    bool isEntered = false;
    public PreparingFightData fightData;

    private void OnMouseUpAsButton()
    {
        GameManager.instance.GetActionHandler().OnClickedHero(gameObject);
    }

    protected override IEnumerator StartCoroutineMove()
    {
        while (Vector3.Distance(rb2D.position, _arrayPos[posId] + PositionCalculator.additionPos) > 0.00001f)
        {
            rb2D.MovePosition(Vector2.MoveTowards(rb2D.position, _arrayPos[posId] + PositionCalculator.additionPos, 0.3f));
            CameraController.instance.Move(rb2D.position);
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
                GameManager.instance.GetActionHandler().OnMovedHero(gameObject);
            }
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            coroutineMove = StartCoroutine(StartCoroutineMove());
        }
    }

    public void IncreaseHp(float value)
    {
        float maxHealth = 50 + 2 * level;
        if (health + value <= maxHealth)
        {
            health += value;
            HpManager.instance.ShowDamage(value, transform.position);
        }
    }

    private void OnMouseEnter()
    {
        isEntered = true;
    }

    private void OnMouseExit()
    {
        isEntered = false;
    }

    private void Update()
    {
        if(isEntered && Input.GetKeyDown(KeyCode.Mouse1))
        {
            GlobalMapManager.instance.OpenUpgradeWindow();
        }
    }
}
