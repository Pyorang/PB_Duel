using System;
using UnityEngine;

public class UserSettingsData : IUserData
{
    public string UserName { get; set; }
    public bool IsSoundEnable { get; set; }
    
    public void SetDefaultData()
    {
        UserName = "abcd1234";
        IsSoundEnable = true;
    }

    public bool LoadData()
    {
        bool result = false;
        try
        {
            UserName = (PlayerPrefs.GetString(nameof(UserName)));
            IsSoundEnable = (PlayerPrefs.GetInt(nameof(IsSoundEnable)) == 1) ? true : false;

            result = true;
        }
        catch (Exception e)
        {
        }

        return result;
    }

    public bool SaveData()
    {
        bool result = false;
        try
        {
            PlayerPrefs.SetString(nameof(UserName), UserName);
            PlayerPrefs.SetInt(nameof(IsSoundEnable), IsSoundEnable ? 1 : 0);
            PlayerPrefs.Save();

            result = true;
        }
        catch (Exception e)
        {
        }

        return result;
    }
}