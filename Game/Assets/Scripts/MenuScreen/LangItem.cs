using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LangItem : MonoBehaviour
{
    private Lang currentLang;
    [SerializeField]
    new string name;
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        SettingsManager.instance.OnChangedLang += ChangeLang;
    }

    public void ChangeLang(Lang newLang)
    {
        currentLang = newLang;
        UpdateText();
    }

    void UpdateText()
    {
        text.text = MySqlManager.GetTranslation(name, currentLang);
    }
}
