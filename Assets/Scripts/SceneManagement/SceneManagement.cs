using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private GameObject inGameMenu;
    [SerializeField] private AudioSource audioSource;
    public string musicName;
    public int currentLevelNumer;
    PlayerInputActions playerInputActions;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.ReloadScene.performed += ReloadScene;
        playerInputActions.Player.InGameMenu.performed += PauseGame;
    }

    public void LoadLevel(int sceneNumber){
        SceneManager.LoadScene(sceneNumber);
    }

    public void PauseGame(InputAction.CallbackContext context){
        
        if(context.performed){
            if(SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1){
                inGameMenu = GameObject.FindGameObjectWithTag("InGameMenu");
                if(Time.timeScale == 0){
                    inGameMenu.GetComponent<InGameMenu>().HideInGameMenu();
                    Time.timeScale = 1;
                    Debug.Log("Hide UI");
                }else{         
                    Time.timeScale = 0;
                    inGameMenu.GetComponent<InGameMenu>().ShowInGameMenu();
                    Debug.Log("Show UI");
                }
            }
        }
    }

    public void ReloadScene(InputAction.CallbackContext context){
       if(context.performed){
            if(SceneManager.GetActiveScene().buildIndex > 5){
                SetTimeScaleToOne();
                int sceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(sceneIndex);
            }
       }    
    }

    public void ReloadSceneButton(){
        if(SceneManager.GetActiveScene().buildIndex > 5){
            SetTimeScaleToOne();
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneIndex);
        }
    }

    public void LoadNextLevel(){
        switch (NameOfSceneByBuildIndex(currentLevelNumer + 1))
        {
            case '1':
                //Plains
                LoadLevel(2);
            break;
            case '2':
                //Tunnels
                LoadLevel(3);
            break;
            case '3':
                //Winter
                LoadLevel(4);
            break;
            case '4':
                //Temple
                LoadLevel(5);
            break;
            case '5':
                //???
                LoadLevel(3);
            break;
            default:
                // bruh
            break;
        }
    }

    private char NameOfSceneByBuildIndex(int buildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot)[0];
    }

    public void LoadFirstScene(){
        SceneManager.LoadScene(6);
    }

    public void LoadMainMenu(){
        SetTimeScaleToOne();
        if(SceneManager.GetActiveScene().buildIndex != 1){
            SceneManager.LoadScene(1);
        }
    }

    public void Exit(){
        SteamClient.Shutdown();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            SteamClient.Shutdown();
            Application.Quit();
        #endif
    }

    private void SetTimeScaleToOne(){
        if(Time.timeScale == 0){
            Time.timeScale = 1;
        }
    }

    public void PlayMusic(AudioClip audioClip){
        audioSource.clip = audioClip;
        audioSource.Play();
        musicName = audioClip.name;
    }

    private void OnApplicationQuit() {
        SteamClient.Shutdown();
    }
}
