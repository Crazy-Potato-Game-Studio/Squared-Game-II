using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private GameObject cam;
    private GameObject player;
    private GameObject audioListener;

    private void Awake() {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            player = other.gameObject;
            other.GetComponent<BoxCollider2D>().isTrigger = true;
            Destroy(cam.GetComponent<CameraFollow>());
            GetComponent<AudioSource>().Play();
            
            int arrowsNum = other.gameObject.GetComponent<ItemsManager>().arrowCount;
            int potionsNum = other.gameObject.GetComponent<ItemsManager>().potionsCount;

            SavePlayerItems(arrowsNum, potionsNum);

            StartCoroutine("WaitAndLoadNextScene");
            StartCoroutine("HidePlayer");

            audioListener = other.gameObject.transform.Find("AudioListener").gameObject;
            audioListener.transform.parent = null;
        }
    }

    IEnumerator WaitAndLoadNextScene(){
        yield return new WaitForSeconds(0.5f);

        if(Physics2D.gravity.y > 0){
            Physics2D.gravity *= -1;
        }
        GameObject _sceneManagement = GameObject.FindGameObjectWithTag("SceneManager");
        _sceneManagement.GetComponent<SceneManagement>().currentLevelNumer = SceneManager.GetActiveScene().buildIndex;
        _sceneManagement.GetComponent<SceneManagement>().LoadNextLevel();
    }

    IEnumerator HidePlayer(){
        yield return new WaitForSeconds(0.2f);

        Destroy(player);
    }

    void SavePlayerItems(int arrowsNum, int potionsNum){
        if(GameObject.FindGameObjectWithTag("ItemCounter")){
            GameObject.FindGameObjectWithTag("ItemCounter").GetComponent<ItemsCounter>().SaveItems(arrowsNum, potionsNum);
        }
    }
}
