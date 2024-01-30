using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class SceneManager : MonoBehaviour
{
    public int levelToLoad;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            ReloadScene();
        }

        if(Input.GetKeyDown(KeyCode.P)){
            if(Time.timeScale == 1){
                Time.timeScale = 0;
            }else{
                Time.timeScale = 1;
            }
            
        }
    }

    public void LoadNextLevel(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void LoadFirstScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void LoadMainMenu(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Exit(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void ReloadScene(){
        int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
}
