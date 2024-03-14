using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public int levelToLoad;
    private GameObject sceneManeger;

    private void Awake() {
        sceneManeger = GameObject.FindGameObjectWithTag("SceneManager");
    }

    public void LoadSelectedLevel(){
        sceneManeger.GetComponent<SceneManagement>().LoadLevel(levelToLoad);
    }
}
