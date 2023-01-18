using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : VisualUnit,IMove
{
    //Characters values
    public TypeCharacter typeCharacter;
    public float health;
    public int speed;
    public int level;

    //IMove values
    protected List<Vector3> _arrayPos;
    public List<Vector3> ArrayPos { get => _arrayPos; set => _arrayPos=value; }
    protected Coroutine coroutineMove;
    protected int posId = 0;
    protected Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void StartMove()
    {
        if(coroutineMove != null)
        {
            StopCoroutine(coroutineMove);
        }
        posId = 0;
        coroutineMove = StartCoroutine(StartCoroutineMove());
    }

    protected virtual IEnumerator StartCoroutineMove()
    {
        while(Vector3.Distance(rb2D.position,_arrayPos[posId]+PositionCalculator.additionPos) > 0.00001f)
        {
            rb2D.MovePosition(Vector2.MoveTowards(rb2D.position,_arrayPos[posId]+PositionCalculator.additionPos,0.3f));
            yield return new WaitForSeconds(0.001f);
        }

        posId++;
        if (posId >= _arrayPos.Count)
        {
            coroutineMove = null;
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            coroutineMove = StartCoroutine(StartCoroutineMove());
        }
    }
}
