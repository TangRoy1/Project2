using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : VisualUnit,IFlag
{
    //Tower values
    public int currentLevel;
    public int maxLevel;
    public bool isHealer = false;

    //IFlag values
    private TypeFlag typeFlag;
    private List<Vector2Int> territory = new List<Vector2Int>();

    public TypeFlag TypeFlag { get => typeFlag; }
    public List<Vector2Int> Territory { get => territory; set => territory = value; }

    GameObject activeFlag;

    public void SetFlag(TypeFlag typeFlag)
    {
        this.typeFlag = typeFlag;
        UpdateFlag();
    }

    void UpdateFlag()
    {
        if(activeFlag != null)
        {
            Destroy(activeFlag);
        }
        GameObject flag = GlobalMapManager.instance.flag;
        activeFlag = Instantiate(flag,Vector3.zero,Quaternion.identity,transform);
        activeFlag.transform.localPosition = new Vector3(0.58f, 1.29f,0);
        Sprite sprite = Resources.Load<Sprite>($"Flags/{typeFlag}");
        activeFlag.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    bool ExistsEnemy()
    {
        GameObject[,] map = GlobalMapManager.instance.map;
        for(int i=0;i < territory.Count; i++)
        {
            VisualUnit visualUnit = map[territory[i].x, territory[i].y].GetComponent<VisualUnit>();
            if(visualUnit.tag == GameManager.TAG_TILE)
            {
                CharacterData characterData = GlobalMapManager.instance.GetCharacterDataWithPos(visualUnit.x, visualUnit.y);
                if(characterData != null && characterData.typeCharacter == TypeCharacter.Enemy)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void CaptureTerritory()
    {
        GameObject[,] map = GlobalMapManager.instance.map;
        for (int i=0;i < territory.Count; i++)
        {
            VisualUnit visualUnit = map[territory[i].x, territory[i].y].GetComponent<VisualUnit>();
            IFlag flag;
            if(visualUnit.TryGetComponent<IFlag>(out flag))
            {
                flag.SetFlag(TypeFlag.Green);
            }
            visualUnit.ChangeVisualUnit(TypeLocation.Forest);
        }
    }

    private void OnMouseUpAsButton()
    {
        if (typeFlag == TypeFlag.Red)
        {
            if (currentLevel >= maxLevel)
            {
                
                CaptureTerritory();
                
            }
            else
            {
                GlobalMapManager.instance.OpenLevels(this);
            }
        }
        else
        {
            if (isHealer) return;
            isHealer = true;
            GlobalMapManager.instance.BuyHealer(new Vector2Int(x, y));
        }
    }
}
