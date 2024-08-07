using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessLoopLevel : MonoBehaviour
{
    [SerializeField] private GameObject grid;
    private GameObject player;
    [SerializeField] private GameObject loopTilemap;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            loopTilemap.SetActive(false);
            player = other.gameObject;
            grid.transform.position = new Vector3(player.transform.position.x, 0, 0);
        }
    }
}
