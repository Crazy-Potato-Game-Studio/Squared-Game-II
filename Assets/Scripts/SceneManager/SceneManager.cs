using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;


public class SceneManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject controlsPanel;

    void Start(){
        menu.SetActive(false);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            ReloadScene();
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!menu.activeSelf && !controlsPanel.activeSelf){
                ShowMenu();
            }else{
                HideMenu();
            }
        }
    }

    private void ShowMenu(){
        menu.SetActive(true);
    }

    private void HideMenu(){
        menu.SetActive(false);
    }

    public void LoadFirstScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
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
