using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Character
{
    public float distance;
    public Warrior target;
    public float maxHealth;

    public virtual void Attack(Warrior warrior)
    {
        float minDamage = level * 2;
        float maxDamage = minDamage + minDamage * 0.1f;
        float damage = Random.Range(minDamage, maxDamage);
        warrior.TakeHealth(damage);
    }

    public virtual void TakeHealth(float value)
    {
        health -= value;
        HpManager.instance.ShowDamage(value, transform.position);
    }

    private void OnMouseEnter()
    {
        HpManager.instance.ShowHp(this);
    }

    private void OnMouseExit()
    {
        HpManager.instance.HideHp();
    }
}
