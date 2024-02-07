using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagementSpawner : MonoBehaviour
{
    [SerializeField] private GameObject sceneManegement;

    private void Awake() {
        int sceneManegementCount;
        sceneManegementCount = GameObject.FindGameObjectsWithTag("SceneManager").Length;

        if(sceneManegementCount < 1){
            Instantiate(sceneManegement);
        }
    }
}
