using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject sceneManager;
    [SerializeField] private GameObject mainMenu;

    private void Awake() {
        StartCoroutine(Wait());
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

    private IEnumerator Wait(){
        yield return new WaitForSeconds(0.3f);
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
    }

}
