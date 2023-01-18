using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LangDropDown : Dropdown
{
    protected override void OnEnable()
    {
        base.OnEnable();
        if(SettingsManager.instance.Lang == Lang.English)
        {
            value = 0;
        }
        else
        {
            value = 1;
        }
    }
}
