using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    private GameObject inGameMenu;
    [SerializeField] private AudioSource audioSource;
    public string musicName;
    public int currentLevelNumer;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)){
            if(SceneManager.GetActiveScene().buildIndex != 0){
                inGameMenu = GameObject.FindGameObjectWithTag("InGameMenu");
                if(Time.timeScale == 1){
                    PouseGame();
                }else{         
                    ResumeGame();
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.R)){
            ReloadScene();
        }
    }

    public void LoadLevel(int sceneNumber){
        SceneManager.LoadScene(sceneNumber);
    }

    public void PouseGame(){
        Time.timeScale = 0;
        inGameMenu.GetComponent<InGameMenu>().ShowInGameMenu();
    }

    public void ResumeGame(){
        Time.timeScale = 1;
        inGameMenu = GameObject.FindGameObjectWithTag("InGameMenu");
        inGameMenu.GetComponent<InGameMenu>().HideInGameMenu();
    }

    public void LoadNextLevel(){
        switch (NameOfSceneByBuildIndex(currentLevelNumer + 1))
        {
            case '1':
                //Plains
                LoadLevel(1);
            break;
            case '2':
                //Tunnels
                LoadLevel(2);
            break;
            case '3':
                //Winter   
            break;
            case '4':
                //Temple
                LoadLevel(3);
            break;
            case '5':
                //Hell
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
        SceneManager.LoadScene(4);
    }

    public void LoadMainMenu(){
        SetTimeScaleToOne();
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
        SetTimeScaleToOne();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
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
}
