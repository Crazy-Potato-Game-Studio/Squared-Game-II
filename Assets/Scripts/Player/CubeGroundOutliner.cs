using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGroundOutliner : MonoBehaviour
{
    [SerializeField] private GameObject outline;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Ground"){
            outline.SetActive(true);
            GetComponentInParent<Arm>().canThrowCube = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Ground"){
            outline.SetActive(true);
            GetComponentInParent<Arm>().canThrowCube = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Ground"){
            outline.SetActive(false);
            GetComponentInParent<Arm>().canThrowCube = true;
        }
    }
    
}
