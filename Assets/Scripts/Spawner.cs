using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    private GameObject player;
    [SerializeField] private GameObject playerPrefab;
    private AudioListener audioListener;

    private void Awake() {
        audioListener = GetComponent<AudioListener>();
        audioListener.enabled = true;
        StartCoroutine(SpawnPlayer(0.2f));
        ItemsCounter.lastPlayedLevel = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            other.GetComponent<BoxCollider2D>().isTrigger = false;

            BoxCollider2D col = GetComponent<BoxCollider2D>();
            Destroy(col);

            GetComponent<AudioSource>().Play();
        }
    }

    public IEnumerator SpawnPlayer(float timeToWait){
        yield return new WaitForSeconds(timeToWait);

        audioListener.enabled = false;

        player = Instantiate(playerPrefab, new Vector2(transform.position.x, transform.position.y + 1), quaternion.identity);
        player.transform.parent = null;

        CameraFollowPlayer(player.transform);

        if(SceneManager.GetActiveScene().buildIndex > 6){
            GivePlayerItems();
        }
        
    }

    private void CameraFollowPlayer(Transform player){
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().player = player;
    }

    private void GivePlayerItems(){
        player.GetComponent<ItemsManager>().arrowCount = SaveSystem.LoadData().lastLevelArrows;
        player.GetComponent<ItemsManager>().UpdateArrowsCount();

        player.GetComponent<ItemsManager>().potionsCount = SaveSystem.LoadData().lastLevelPotions;
        player.GetComponent<ItemsManager>().UpdatePotionsCount();
    }
}
