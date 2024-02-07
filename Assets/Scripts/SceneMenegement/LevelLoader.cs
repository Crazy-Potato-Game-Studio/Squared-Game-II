using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public int nextLevelNumber;
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelNumber);
        }
    }

    private void Awake() {
        nextLevelNumber = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagement>().levelToLoad;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().enabled = false;
    }
}
