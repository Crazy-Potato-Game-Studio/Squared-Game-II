using System.Collections;
using System.Collections.Generic;
using LevelBuilder;
using UnityEngine;
using UnityEngine.UI;

public class LevelCreateMenuButton : MonoBehaviour
{
    private GameObject LevelCreateManager;

    private void Awake() {
        LevelCreateManager = GameObject.FindGameObjectWithTag("LevelCreateManager");
    }

    public void Play()
    {   
        LevelCreateManager.GetComponent<LevelEditorMenuManager>().Play();
    }

    public void Edit()
    {
        LevelCreateManager.GetComponent<LevelEditorMenuManager>().Edit();
        GameObject SteamAchievementsManager = GameObject.FindGameObjectWithTag("MainCamera");
        SteamAchievementsManager.GetComponent<SteamAchievementsManager>().UnlockAchievement("editor_ach");
    }

    public void Upload()
    {
        LevelCreateManager.GetComponent<LevelEditorMenuManager>().Upload();
    }

    public void Delete(){
        LevelCreateManager.GetComponent<LevelEditorMenuManager>().Delete();
    }

}
