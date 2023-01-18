using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpManager : MonoBehaviour
{
    public static HpManager instance;
    [SerializeField]
    GameObject damagePrefab;
    [SerializeField]
    GameObject hpPrefab;
    [SerializeField]
    Transform parent;
    GameObject activeHp;

    private void Awake()
    {
        instance = this;
    }

    public void ShowDamage(float damage, Vector3 basePos)
    {
        Vector3 newPos = basePos - new Vector3(0, 0.5f, 0);
        GameObject clon = Instantiate(damagePrefab, newPos, Quaternion.identity, parent);
        clon.transform.GetChild(0).GetComponent<Text>().text = System.Math.Round(damage, 3).ToString();
        clon.name = "Damage";
        StartCoroutine(StartCoroutineMove(basePos + new Vector3(0, 0.5f, 0), clon));
    }

    IEnumerator StartCoroutineMove(Vector3 endPos,GameObject movingObject)
    {
        while(Vector3.Distance(movingObject.transform.position,endPos) > 0.00001f)
        {
            movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, endPos, 0.03f);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(movingObject);
    }

    public void ShowHp(Warrior warrior)
    {
        float percentHp = (warrior.health / warrior.maxHealth)*100;
        GameObject clon = Instantiate(hpPrefab, warrior.transform.position, Quaternion.identity, parent);
        clon.transform.GetChild(0).GetComponent<Text>().text = warrior.health.ToString();
        clon.name = "Hp";
        activeHp = clon;
        if(percentHp > 50)
        {
            clon.GetComponent<Image>().color = Color.green;
        }else if(percentHp <= 50 && percentHp > 30)
        {
            clon.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            clon.GetComponent<Image>().color = Color.red;
        }
    }

    public void HideHp()
    {
        Destroy(activeHp);
    }
}
