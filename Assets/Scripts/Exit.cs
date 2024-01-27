using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            other.GetComponent<BoxCollider2D>().isTrigger = true;

            BoxCollider2D col = GetComponent<BoxCollider2D>();
            Destroy(col);

            GetComponent<AudioSource>().Play();

            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>().levelToLoad = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex+1;
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>().LoadNextLevel();

        }
    }
}
