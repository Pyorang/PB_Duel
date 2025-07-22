using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserDataManager : SingletonBehaviour<UserDataManager>
{
    public bool ExistsSavedData { get; private set; }

    public List<IUserData> UserDataList { get; } = new();

    protected override void Init()
    {
        base.Init();

        ExistsSavedData = PlayerPrefs.GetInt(nameof(ExistsSavedData)) == 1;

        UserDataList.Add(new UserSettingsData());
    }

    public void SetDefaultData()
    {
        foreach (IUserData data in UserDataList)
        {
            data.SetDefaultData();
        }
    }

    public void LoadUserData()
    {
        if (!ExistsSavedData)
        {
            return;
        }

        foreach (IUserData data in UserDataList)
        {
            data.LoadData();
        }
    }

    public void SaveUserData()
    {
        foreach (IUserData data in UserDataList)
        {
            if (!data.SaveData())
            {
                return;
            }
        }

        PlayerPrefs.SetInt(nameof(ExistsSavedData), 1);
        PlayerPrefs.Save();

        ExistsSavedData = true;
    }

    public T GetUserData<T>() where T : class, IUserData
    {
        return UserDataList.OfType<T>().FirstOrDefault();
    }
}
