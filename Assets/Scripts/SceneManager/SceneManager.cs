using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;


public class SceneManager : MonoBehaviour
{
    public TextMeshProUGUI hint;

    void Start(){
        Destroy(hint, 5);
    }

    public void LoadFirstScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OpenSettings(){
        
    }

    public void ExitToMenu(){

    }

    public void Exit(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void ReloadScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        UnityEngine.Debug.Log("placki");
    }
}
