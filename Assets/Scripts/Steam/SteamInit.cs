using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamInit : MonoBehaviour
{
    public static bool steamConnection = false;

    void Start()
    {
        if(!steamConnection){
            try{
                SteamClient.Init(2960090);
                PrintSteamName();
                steamConnection = true;
            }catch(System.Exception error){
                Debug.Log(error);
                steamConnection = false;
            }
        }

        #if UNITY_EDITOR
            GetComponent<SteamAchievementsManager>().ClearAchievement("slime_ach");
            GetComponent<SteamAchievementsManager>().ClearAchievement("king_ach");
            GetComponent<SteamAchievementsManager>().ClearAchievement("queen_ach");
            GetComponent<SteamAchievementsManager>().ClearAchievement("eyes_ach");
            GetComponent<SteamAchievementsManager>().ClearAchievement("finish_ach");
            GetComponent<SteamAchievementsManager>().ClearAchievement("speedrun_ach");
            GetComponent<SteamAchievementsManager>().ClearAchievement("onion_ach");
            GetComponent<SteamAchievementsManager>().ClearAchievement("reverse_ach");
            GetComponent<SteamAchievementsManager>().ClearAchievement("laser_ach");
            GetComponent<SteamAchievementsManager>().ClearAchievement("speedrun_ach");
        #endif
    }

    private void PrintSteamName(){
        Debug.Log(SteamClient.Name);
    }
}
