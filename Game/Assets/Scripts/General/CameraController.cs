using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera camera;
    public static CameraController instance;
    Coroutine coroutineMove;

    private void Awake()
    {
        instance = this;
        camera = GetComponent<Camera>();
    }

    public void Move(Vector3 pos)
    {
        if(coroutineMove != null)
        {
            StopCoroutine(coroutineMove);
            coroutineMove = null;
        }
        camera.transform.position = new Vector3(pos.x, pos.y, -10);
    }

    public void SetSize(float size)
    {
        camera.orthographicSize = size;
    }

    public void MoveLerp(Vector3 endPos)
    {
        if(coroutineMove != null)
        {
            StopCoroutine(coroutineMove);
        }
        coroutineMove = StartCoroutine(StartCoroutineMove(endPos));
    }

    IEnumerator StartCoroutineMove(Vector3 endPos)
    {
        while(Vector2.Distance(transform.position,endPos) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, 0.01f);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            yield return new WaitForSeconds(0.01f);
        }
        coroutineMove = null;
    }
}
