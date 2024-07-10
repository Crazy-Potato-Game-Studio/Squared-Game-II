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
    public bool hasElectricity = true;

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
            if(numberOfWeights > 0){
                numberOfWeights--;
            }
            UpdateNumberOfWeights();
        }
    }

    private void UpdateNumberOfWeights() {
        if(numberOfWeights == 0){
            TurnObjects();
            PlaySound();
            isPressed = false;
            UpdateAnimatorBool();
        }else if(numberOfWeights == 1){
            TurnObjects();
            isPressed = true;
            UpdateAnimatorBool();
        }

    }

    private void PlaySound(){
        source.PlayOneShot(clip);
    }

    private void TurnObjects(){
        for(int i = 0; i < obejctsToTurnOn.Length; i++){
            if(obejctsToTurnOn[i].gameObject.GetComponent<Portal>() != null){
                if(obejctsToTurnOn[i].gameObject.GetComponent<Portal>().isOn){
                    obejctsToTurnOn[i].gameObject.GetComponent<Portal>().TurnOff();
                }else{
                    obejctsToTurnOn[i].gameObject.GetComponent<Portal>().TurnOn();
                } 
            }else if(obejctsToTurnOn[i].gameObject.GetComponent<Doors>() != null){
                if(obejctsToTurnOn[i].gameObject.GetComponent<Doors>().doorsOpen){
                    obejctsToTurnOn[i].gameObject.GetComponent<Doors>().CloseDoors();
                }else{
                    obejctsToTurnOn[i].gameObject.GetComponent<Doors>().OpenDoors();
                } 
            }
        } 
    }

    private void UpdateAnimatorBool(){
        animator.SetBool("isPressed", isPressed);
    }

    public void ResetCollider(){
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = true;
    }
}
