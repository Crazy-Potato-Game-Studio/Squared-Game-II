using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public int levelToLoad;
    private GameObject sceneManeger;
    [SerializeField] private AudioClip plains;
    [SerializeField] private AudioClip electricity;
    [SerializeField] private AudioClip portals;
    [SerializeField] private AudioClip temple;

    private void Awake() {
        sceneManeger = GameObject.FindGameObjectWithTag("SceneManager");
    }

    public void LoadSelectedLevel(){
        sceneManeger.GetComponent<SceneManagement>().LoadLevel(levelToLoad);
        switch (levelToLoad)
        {
            case 8:
                sceneManeger.GetComponent<MusicManager>().PlayNewSong(electricity);
                break;
            case 10:
                sceneManeger.GetComponent<MusicManager>().PlayNewSong(portals);
                break;
            case 11:
                sceneManeger.GetComponent<MusicManager>().PlayNewSong(temple);
                break;
            default:
                break;
        }
    }
}
