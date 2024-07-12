using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorRotate : MonoBehaviour
{
    private bool canRotate;
    [SerializeField] private GameObject mirror;

    void Start()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" || other.tag == "ResistanceCollider"){
            canRotate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player" || other.tag == "ResistanceCollider"){
            canRotate = false;
        }
    }

    void Update()
    {
        if(canRotate){
            if(Input.GetKey(KeyCode.Q)){
                mirror.transform.Rotate(0, 0, 0.2f * Time.fixedDeltaTime * 20);
            }
            if(Input.GetKey(KeyCode.E)){
                mirror.transform.Rotate(0, 0, -0.2f * Time.fixedDeltaTime * 20);
            }
        }

    }
}
