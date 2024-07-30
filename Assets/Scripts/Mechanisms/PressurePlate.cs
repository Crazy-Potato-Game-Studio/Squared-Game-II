using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [HideInInspector] public GameObject[] obejctsToTurnOn;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;
    [SerializeField] private Animator animator;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material lightMaterial;

    private int numberOfWeights = 0;
    private bool isPressed;
    public bool hasElectricity = true;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cube" || other.gameObject.tag == "ResistanceCollider" || other.gameObject.tag == "Enemy"){
            numberOfWeights++; 
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Cube" || other.gameObject.tag == "ResistanceCollider" || other.gameObject.tag == "Enemy"){
            numberOfWeights--;   
        }
    }

    private void Update() {
        if(hasElectricity){
            if(numberOfWeights == 1 && !isPressed){
                Pressure();
            }

            if(numberOfWeights == 0 && isPressed){
                NoPressure();
            }

            transform.GetChild(0).GetComponent<SpriteRenderer>().material = lightMaterial;
        }else{
            transform.GetChild(0).GetComponent<SpriteRenderer>().material = defaultMaterial;
        }
    }

    private void Pressure(){
        PlaySound();
        isPressed = true;
        UpdateAnimatorBool();
        TurnObjects();
    }

    private void NoPressure(){
        PlaySound();
        isPressed = false;
        UpdateAnimatorBool();
        TurnObjects();
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

}
