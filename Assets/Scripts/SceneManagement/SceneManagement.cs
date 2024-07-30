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
            if(SceneManager.GetActiveScene().buildIndex != 0 || SceneManager.GetActiveScene().buildIndex != 1){
                inGameMenu = GameObject.FindGameObjectWithTag("InGameMenu");
                if(Time.timeScale == 1){
                    PouseGame();
                }else{         
                    ResumeGame();
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.R) && SceneManager.GetActiveScene().buildIndex > 5){
            ReloadScene();
        }

        if(Input.GetKeyDown(KeyCode.N)){
            currentLevelNumer = SceneManager.GetActiveScene().buildIndex;
            LoadNextLevel();
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
