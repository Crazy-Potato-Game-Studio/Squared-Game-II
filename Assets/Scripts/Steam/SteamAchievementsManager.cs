using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.SceneManagement;

public class SteamAchievementsManager : MonoBehaviour
{
    public void UnlockAchievement(string id){
        if(SteamConnected()){
            if(!IsAchievementUnlocked(id)){
                var ach = new Steamworks.Data.Achievement(id);
                ach.Trigger();
                Debug.Log($"Achievement {id} unlocked");
            }else{
                Debug.Log("Achievement already unlocked");
            }
        }else{
            Debug.Log("Steam not connected");
        }
    }

    public void ClearAchievement(string id){
        if(SteamConnected()){
            var ach = new Steamworks.Data.Achievement(id);
            ach.Clear();

            Debug.Log($"Achievement {id} cleared");
        }else{
            Debug.Log("Steam not connected");
        }
    }

    public bool IsAchievementUnlocked(string id){
        if(SteamConnected()){
            var ach = new Steamworks.Data.Achievement(id);
            return ach.State;
        }else{
            Debug.Log("Steam not connected");
            return false;
        }
    }

    public bool SteamConnected(){
        return SteamInit.steamConnection;
    }
}
