using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private GameObject outline;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider"){
            outline.SetActive(true);
            other.GetComponent<Arm>().canLiftCube = true;
            other.GetComponent<Arm>().groundCube = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider"){
            outline.SetActive(false);
            other.GetComponent<Arm>().canLiftCube = false;
            other.GetComponent<Arm>().groundCube = null;
        }
    }
}
