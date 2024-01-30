using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{

    private GameObject cam;

    private void Awake() {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){

            other.GetComponent<BoxCollider2D>().isTrigger = true;
            Destroy(cam.GetComponent<CameraFollow>());
            GetComponent<AudioSource>().Play();
            StartCoroutine("WaitAndLoadNextScene");
        }
    }

    IEnumerator WaitAndLoadNextScene(){
        yield return new WaitForSeconds(0.5f);

        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>().levelToLoad = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex+1;
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>().LoadNextLevel();
    }
}
