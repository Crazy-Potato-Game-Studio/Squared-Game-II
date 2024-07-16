using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] private GameObject player;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            other.GetComponent<BoxCollider2D>().isTrigger = false;

            BoxCollider2D col = GetComponent<BoxCollider2D>();
            Destroy(col);

            GetComponent<AudioSource>().Play();
        }
    }

    public void SpawnPlayer(){
        player.transform.parent = null;
        player.transform.localScale = new Vector3(1,1,1);

        if(SceneManager.GetActiveScene().buildIndex > 6){
            GivePlayerItems();
        }
    }

    private void Awake() {
        SpawnPlayer();
    }

    private void GivePlayerItems(){
        GameObject itemCounter = GameObject.FindGameObjectWithTag("ItemCounter");
        if(itemCounter && itemCounter.GetComponent<ItemsCounter>().levelArray[SceneManager.GetActiveScene().buildIndex-1] != null){
            player.GetComponent<ItemsManager>().arrowCount = itemCounter.GetComponent<ItemsCounter>().levelArray[SceneManager.GetActiveScene().buildIndex-1].arrowsNumber;
            player.GetComponent<ItemsManager>().UpdateArrowsCount();
        }
        if(itemCounter && itemCounter.GetComponent<ItemsCounter>().levelArray[SceneManager.GetActiveScene().buildIndex-1] != null){
            player.GetComponent<ItemsManager>().potionsCount = itemCounter.GetComponent<ItemsCounter>().levelArray[SceneManager.GetActiveScene().buildIndex-1].potionsNumber;
            player.GetComponent<ItemsManager>().UpdatePotionsCount();
        }
    }
}
