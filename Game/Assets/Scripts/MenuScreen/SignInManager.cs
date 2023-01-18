using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInManager : MonoBehaviour
{
    [SerializeField]
    Text emailText;
    [SerializeField]
    Text passwordText;
    [SerializeField]
    GameObject info;

    public void SignIn()
    {
        HideInfo();
        string email = emailText.text.Trim().ToLower();
        string password = passwordText.text.Trim().ToLower();
        bool isValid = true;

        if (email == "" || password == "")
        {
            isValid = false;
        }

        if (isValid)
        {
            if (MySqlManager.ExistsUser(email, password))
            {
                MySqlManager.userId = MySqlManager.GetUserId(email);
                ShowInfo("Вход выполнен!");
                SettingsManager.instance.LoadFromDB();
            }
            else
            {
                ShowInfo("Данные ошибочны");
            }
        }
        else
        {
            ShowInfo("Данные ошибочны");
        }
    }

    public void ShowInfo(string text)
    {
        info.SetActive(true);
        info.GetComponent<Text>().text = text;
    }

    public void HideInfo()
    {
        info.SetActive(false);
    }
}
