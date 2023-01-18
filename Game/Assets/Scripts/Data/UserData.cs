using System;

public class UserData
{
    public string firstName;
    public string lastName;
    public string login;
    public string password;
    public DateTime registrationDate;
    public DateTime lastLoginDate;

    public UserData(string firstName, string lastName, string login, string password, DateTime registrationDate, DateTime lastLoginDate)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.login = login;
        this.password = password;
        this.registrationDate = registrationDate;
        this.lastLoginDate = lastLoginDate;
    }
}