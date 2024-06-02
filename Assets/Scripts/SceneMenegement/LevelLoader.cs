using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int nextLevelNumber;
    private GameObject player;
    private GameObject _sceneManagement;

    private void Awake() {
        _sceneManagement = GameObject.FindGameObjectWithTag("SceneManager");
        nextLevelNumber = _sceneManagement.GetComponent<SceneManagement>().levelToLoad;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            SceneManager.LoadScene(nextLevelNumber);
        }
    }
}
