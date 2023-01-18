using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using MySql.Data.MySqlClient;

public static class MySqlManager
{
    private static MySqlConnection connection;
    public static int userId = -1;

    public static void CreateConnection()
    {
        if(connection == null)
        {
            string connectionString = $"host={ConfigData.HOST};user={ConfigData.USER};password={ConfigData.PASSWORD};database={ConfigData.NAME_BASE}";
            connection = new MySqlConnection(connectionString);
        }
    }

    public static void OpenConnection()
    {
        if(connection.State == ConnectionState.Closed)
        {
            connection.Open();
        }
    }

    public static void CloseConnection()
    {
        if(connection.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }

    static DataTable Query(string sql)
    {
        OpenConnection();
        DataTable data = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter(sql, connection);
        adapter.Fill(data);
        CloseConnection();
        return data;
    }
     public static string GetTranslation(string name,Lang lang)
    {
        string sql = $"SELECT `value` FROM `dictionary` " +
            $"INNER JOIN `dictionary_keys` AS `dk` ON `dk`.`id`=`dictionary_keys_id` " +
            $"WHERE `dk`.`name`='{name}' AND `langs_id`={(int)lang}";
        DataTable data = Query(sql);
        if(data.Rows.Count > 0)
        {
            return data.Rows[0][0].ToString();
        }
        else
        {
            return "";
        }
    }

    public static bool IsLogged()
    {
        if (userId == -1) {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool ExistsLogin(string name)
    {
        string sql = $"SELECT `id` FROM `users` WHERE `email`='{name}'";
        DataTable data = Query(sql);
        if (data.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }

    public static void CreateUser(UserData userData)
    {
        string registrationDate = userData.registrationDate.ToString("yyyy-MM-dd HH:mm:ss");
        string lastLoginDate = userData.lastLoginDate.ToString("yyyy-MM-dd HH:mm:ss");

        string sql = $"INSERT INTO `users`(`first_name`,`last_name`,`email`,`password`,`registration_date`,`last_login_date`) " +
            $"VALUES('{userData.firstName}','{userData.lastName}','{userData.login}','{userData.password}','{registrationDate}','{lastLoginDate}')";
        Query(sql);
    }

    public static int GetUserId(string login)
    {
        string sql = $"SELECT `id` FROM `users` WHERE `email`='{login}'";
        DataTable data = Query(sql);
        if (data.Rows.Count > 0)
        {
            return (int)data.Rows[0][0];
        }
        return -1;
    }

    public static bool ExistsUser(string email,string password)
    {
        string sql = $"SELECT `id` FROM `users` WHERE `email`='{email}' AND `password`='{password}'";
        DataTable data = Query(sql);
        if (data.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }

    public static void InsertSettingData(string name,string value)
    {
        string sql = $"INSERT INTO `settings`(`setting_name`,`setting_value`,`users_id`) VALUES('{name}','{value}','{userId}')";
        Query(sql);
    }

    public static void ChangeSettingData(string name, string value)
    {
        string sql = $"UPDATE `settings` SET `setting_value`='{value}' WHERE `setting_name`='{name}' AND `users_id`='{(int)userId}'";
        Query(sql);
    }

    public static string GetSettingData(string name)
    {
        string sql = $"SELECT `setting_value` FROM `settings` WHERE `setting_name`='{name}' AND `users_id`={userId}";
        DataTable data = Query(sql);
        if(data.Rows.Count > 0)
        {
            return data.Rows[0][0].ToString();
        }
        return "";
    }

    public static bool ExistsSettingData(string name)
    {
        string sql = $"SELECT `id` FROM `settings` WHERE `setting_name`='{name}' AND `users_id`={userId}";
        DataTable data = Query(sql);
        if (data.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }
}
