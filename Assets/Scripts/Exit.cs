using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Exit : MonoBehaviour
{
    private GameObject cam;
    private GameObject player;

    private void Awake() {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            player = other.gameObject;
            other.GetComponent<BoxCollider2D>().isTrigger = true;
            Destroy(cam.GetComponent<CameraFollow>());
            GetComponent<AudioSource>().Play();
            StartCoroutine("WaitAndLoadNextScene");
            StartCoroutine("HidePlayer");
        }
    }

    IEnumerator WaitAndLoadNextScene(){
        yield return new WaitForSeconds(0.5f);

        GameObject _sceneManagement = GameObject.FindGameObjectWithTag("SceneManager");
        _sceneManagement.GetComponent<SceneManagement>().currentLevelNumer = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        _sceneManagement.GetComponent<SceneManagement>().LoadNextLevel();
    }

    IEnumerator HidePlayer(){
        yield return new WaitForSeconds(0.2f);

        Destroy(player);
    }
}
