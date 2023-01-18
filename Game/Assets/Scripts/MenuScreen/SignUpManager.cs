using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SignUpManager : MonoBehaviour
{
    [SerializeField]
    Text firstNameText;
    [SerializeField]
    Text lastNameText;
    [SerializeField]
    Text emailText;
    [SerializeField]
    Text passwordText;
    [SerializeField]
    GameObject info;

    public void SignUp()
    {
        HideInfo();
        string firstName = firstNameText.text.Trim().ToLower();
        string lastName = lastNameText.text.Trim().ToLower();
        string email = emailText.text.Trim().ToLower();
        string password = passwordText.text.Trim().ToLower();
        bool isValid = true;

        if(firstName == "" || lastName == "" || email == "" || password == "")
        {
            isValid = false;
        }

        if (MySqlManager.ExistsLogin(email))
        {
            isValid = false;
        }

        if (isValid)
        {
            UserData userData = new UserData(firstName, lastName, email, password, DateTime.Now, DateTime.Now);
            MySqlManager.CreateUser(userData);
            MySqlManager.userId = MySqlManager.GetUserId(email);
            SettingsManager.instance.LoadDefaultSettings();
            SettingsManager.instance.SaveInDB();
            ShowInfo("Пользователь создан");
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
