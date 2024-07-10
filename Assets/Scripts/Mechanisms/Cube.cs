using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private GameObject outline;
    private GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider"){
            outline.SetActive(true);
            player.GetComponent<Arm>().canLiftCube = true;
            player.GetComponent<Arm>().groundCube = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider"){
            outline.SetActive(false);
            player.GetComponent<Arm>().canLiftCube = false;
            player.GetComponent<Arm>().groundCube = null;
        }
    }
}
