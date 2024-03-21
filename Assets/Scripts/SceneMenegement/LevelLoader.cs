using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int nextLevelNumber;
    private GameObject player;
    private GameObject sceneManagement;

    [SerializeField] private AudioClip plains;
    [SerializeField] private AudioClip electricity;
    [SerializeField] private AudioClip portals;
    [SerializeField] private AudioClip temple;

    private void Awake() {
        sceneManagement = GameObject.FindGameObjectWithTag("SceneManager");
        nextLevelNumber = sceneManagement.GetComponent<SceneManagement>().levelToLoad;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){

            switch (nextLevelNumber)
            {
                case 8:
                    sceneManagement.GetComponent<MusicManager>().PlayNewSong(electricity);
                    break;
                case 10:
                    sceneManagement.GetComponent<MusicManager>().PlayNewSong(portals);
                    break;
                case 11:
                    sceneManagement.GetComponent<MusicManager>().PlayNewSong(temple);
                    break;
                default:
                    break;
            }
            SceneManager.LoadScene(nextLevelNumber);
        }
    }


}
