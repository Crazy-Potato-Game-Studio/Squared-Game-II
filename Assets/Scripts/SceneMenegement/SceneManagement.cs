using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
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
        SceneManager.LoadScene(1);
    }

    public void LoadFirstScene(){
        SceneManager.LoadScene(2);
    }

    public void LoadMainMenu(){
        if(SceneManager.GetActiveScene().buildIndex != 0){
            SceneManager.LoadScene(0);
        }
    }

    public void Exit(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ReloadScene(){
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }
}
