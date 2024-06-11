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
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            _sceneManagement = GameObject.FindGameObjectWithTag("SceneManager");
            _sceneManagement.GetComponent<SceneManagement>().LoadLevel(_sceneManagement.GetComponent<SceneManagement>().currentLevelNumer+1);
        }
    }
}
