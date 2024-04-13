using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject[] obejctsToTurnOn;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;
    [SerializeField] private Animator animator;
    private int numberOfWeights = 0;
    private bool isPressed;
    public bool hasElectricity;

    private void OnTriggerEnter2D(Collider2D other) {
        if((other.gameObject.tag == "Player" || other.gameObject.tag == "Cube" || other.gameObject.tag == "ResistanceCollider") && hasElectricity){
            if(numberOfWeights == 0){
                PlaySound();
            }
            numberOfWeights++;
            UpdateNumberOfWeights();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if((other.gameObject.tag == "Player" || other.gameObject.tag == "Cube" || other.gameObject.tag == "ResistanceCollider") && hasElectricity){
            if(numberOfWeights == 1){
               PlaySound(); 
            }
            numberOfWeights--;
            UpdateNumberOfWeights();
        }
    }

    private void UpdateNumberOfWeights() {
        if(numberOfWeights == 0){
            TurnObjectsOff();
            PlaySound();
            isPressed = false;
            UpdateAnimatorBool();
        }else if(numberOfWeights == 1){
            TurnObjectsOn();
            isPressed = true;
            UpdateAnimatorBool();
        }

        //Debug.Log("Number of weights:"+numberOfWeights);
    }

    private void PlaySound(){
        source.PlayOneShot(clip);
    }

    private void TurnObjectsOn(){
        for(int i = 0; i < obejctsToTurnOn.Length; i++){
            if(obejctsToTurnOn[i].gameObject.GetComponent<Portal>() != null){
                obejctsToTurnOn[i].gameObject.GetComponent<Portal>().TurnOn();
            }else if(obejctsToTurnOn[i].gameObject.GetComponent<Doors>() != null){
                obejctsToTurnOn[i].gameObject.GetComponent<Doors>().OpenDoors();
            }
        } 
    }

    private void TurnObjectsOff(){
        for (int i = 0; i < obejctsToTurnOn.Length; i++)
        {
            if(obejctsToTurnOn[i].GetComponent<Portal>() != null){
                obejctsToTurnOn[i].GetComponent<Portal>().TurnOff();
            }else if(obejctsToTurnOn[i].GetComponent<Doors>() != null){
                obejctsToTurnOn[i].GetComponent<Doors>().CloseDoors();
            }
        }
    }

    private void UpdateAnimatorBool(){
        animator.SetBool("isPressed", isPressed);
    }
}
