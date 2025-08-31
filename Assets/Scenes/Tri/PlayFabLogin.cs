using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


public class PlayFabLogin : MonoBehaviour
{
    void Start()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };


      
    }


    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Đăng nhập PlayFab thành công!" + result.PlayFabId);
        SavePlayerData();
        LoadPlayerData();
    }


   

    public void SavePlayerData()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Level", "5"},
                {"Gold", "1234"},
                {"LastCheckpoint", "ForestTemple"}
            }
        };


        PlayFabClientAPI.UpdateUserData(request, OnDataSaved, OnDataSaveFailed);
    }


    void OnDataSaved(UpdateUserDataResult result)
    {
        Debug.Log("Dữ liệu đã được lưu thành công!");
    }


    void OnDataSaveFailed(PlayFabError error)
    {
        Debug.LogError("Lưu dữ liệu thất bại: " + error.GenerateErrorReport());
    }

    public void LoadPlayerData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataLoaded, OnDataLoadFailed);
    }


    void OnDataLoaded(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("Level"))
        {
            string level = result.Data["Level"].Value;
            string gold = result.Data["Gold"].Value;
            Debug.Log("Level: " + level + " | Gold: " + gold);
        }
        else
        {
            Debug.Log("Không có dữ liệu người chơi.");
        }
    }


    void OnDataLoadFailed(PlayFabError error)
    {
        Debug.LogError("Tải dữ liệu thất bại: " + error.GenerateErrorReport());
    }

}
