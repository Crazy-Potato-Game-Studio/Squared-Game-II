using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] private GameObject playerPrefab;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            other.GetComponent<BoxCollider2D>().isTrigger = false;

            BoxCollider2D col = GetComponent<BoxCollider2D>();
            Destroy(col);

            GetComponent<AudioSource>().Play();
        }
    }

    public void SpawnPlayer(){
        GameObject player = Instantiate(playerPrefab, spawnPoint);
        player.transform.parent = null;
    }
}
