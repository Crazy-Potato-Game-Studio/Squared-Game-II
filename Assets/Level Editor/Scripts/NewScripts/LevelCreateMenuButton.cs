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
        Debug.Log(PlayerPrefs.GetString("CURRENT_LEVEL"));
        LevelCreateManager.GetComponent<LevelEditorMenuManager>().Play();
    }

    public void Edit()
    {
        Debug.Log(PlayerPrefs.GetString("CURRENT_LEVEL"));
        LevelCreateManager.GetComponent<LevelEditorMenuManager>().Edit();
    }

    public void Upload()
    {
        LevelCreateManager.GetComponent<LevelEditorMenuManager>().Upload();
    }

}
