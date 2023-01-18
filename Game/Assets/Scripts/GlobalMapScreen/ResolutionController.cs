using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour
{
    List<Vector2Int> resolutions;
    int numberResolution = 2;
    [SerializeField]
    Image line;

    private void Start()
    {
        resolutions = new List<Vector2Int>()
        {
            new Vector2Int(800,600),
            new Vector2Int(1024,768),
            new Vector2Int(1920,1080)
        };
        ChangeResolution(2);
    }

    public void Plus()
    {
        if (numberResolution +1 <=2)
        {
            line.fillAmount += 0.5f;
            numberResolution += 1;
            ChangeResolution(numberResolution);
        }
    }

    public void Minus()
    {
        if (numberResolution-1 >= 0)
        {
            line.fillAmount -= 0.5f;
            numberResolution -= 1;
            ChangeResolution(numberResolution);
        }
    }

    public void ChangeResolution(int number)
    {
        Screen.SetResolution(resolutions[number].x, resolutions[number].y, true);
    }
}
