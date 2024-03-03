using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject sceneManager;
    [SerializeField] private GameObject sceneManagerPrefab;
    [SerializeField] private GameObject mainMenu;
    private GameObject[] sceneManagersCount;

    private void Awake() {
        sceneManagersCount = GameObject.FindGameObjectsWithTag("SceneManager");
        if(sceneManagersCount.Length > 1){
            for(int i = 0; i < sceneManagersCount.Length; i++){
                Destroy(sceneManagersCount[i]);
            }
            sceneManager = Instantiate(sceneManagerPrefab);
            sceneManager.transform.parent = null;
        }
    }

    public void StartGame(){
        sceneManager.GetComponent<SceneManagement>().LoadFirstScene();
    }

    public void OpenMainMenu(GameObject currentMenu){
        mainMenu.SetActive(true);
        currentMenu.SetActive(false);
    }

    public void OpenMenu(GameObject menuToOpen){
        mainMenu.SetActive(false);
        menuToOpen.SetActive(true);
    }

    public void Exit(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
