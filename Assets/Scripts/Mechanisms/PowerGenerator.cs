using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGenerator : MonoBehaviour
{
    private bool playerPower;
    [SerializeField] private GameObject powerAnim;
    [SerializeField] private AudioClip turningOnSound;
    [SerializeField] private AudioSource source;
    [SerializeField] private GameObject hint;
    private bool generatorOn;

    private void Awake() {
        SetLamps(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            playerPower = other.GetComponent<PlayerHasPower>().playerHasPowerPickup;
            if(!generatorOn){
                hint.SetActive(true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && playerPower){
            GeneratorOn(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            hint.SetActive(false);
        }
    }

    private void GeneratorOn(Collider2D other){
        generatorOn = true;
        hint.SetActive(false);
        playerPower = false;
        other.GetComponent<PlayerHasPower>().PlayerLeftPower();
        Debug.Log("Generator on");
        powerAnim.SetActive(true);
        SetLamps(true);
    }

    private void SetLamps(bool lampOn){
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("Lamp");
        if(lampOn){
            source.PlayOneShot(turningOnSound);
            for(int i = 0; i < lamps.Length; i++){
                lamps[i].GetComponent<Lamp>().LampOn();
            }
            SetPressurePlates();
        }else{
            for(int i = 0; i < lamps.Length; i++){
                lamps[i].GetComponent<Lamp>().LampOff();
            }
        }
        
    }

    private void SetPressurePlates(){
        GameObject[] pressurePlates = GameObject.FindGameObjectsWithTag("PressurePlates");
        Debug.Log(pressurePlates.Length);
        for(int i = 0; i < pressurePlates.Length; i++){
            pressurePlates[i].GetComponent<PressurePlate>().hasElectricity = true;
        }
    }
}
