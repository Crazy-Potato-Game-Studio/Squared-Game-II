using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class PowerGenerator : MonoBehaviour
{
    [SerializeField] private GameObject powerAnim;
    [SerializeField] private AudioClip turningOnSound;
    [SerializeField] private AudioSource source;
    [SerializeField] private GameObject PcHint;
    [SerializeField] private GameObject playStationHint;
    [SerializeField] private GameObject XboxHint;
    public bool generatorOn;
    private GameObject player;

    private void Awake() {
        SetLamps(false);
        SetPressurePlates(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            player = other.gameObject;
            if(!generatorOn){
                if(UsedDevice.usingGamepad){
                    if(Gamepad.current is DualShockGamepad){
                        playStationHint.SetActive(true);
                    }else{
                        XboxHint.SetActive(true);
                    }
                }else{
                    PcHint.SetActive(true);
                }
            }
            player.GetComponent<TurnOnGenerator>().playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            PcHint.SetActive(false);
            playStationHint.SetActive(false);
            XboxHint.SetActive(false);
            player.GetComponent<TurnOnGenerator>().playerInRange = false;
        }
    }

    public void GeneratorOn(){
        generatorOn = true;
        PcHint.SetActive(false);
        playStationHint.SetActive(false);
        XboxHint.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHasPower>().PlayerLeftPower();
        powerAnim.SetActive(true);
        SetLamps(true);
        SetPressurePlates(true);
    }

    private void SetLamps(bool lampOn){
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("Lamp");
        if(lampOn){
            source.PlayOneShot(turningOnSound);
            for(int i = 0; i < lamps.Length; i++){
                lamps[i].GetComponent<Lamp>().LampOn();
            }
        }else{
            for(int i = 0; i < lamps.Length; i++){
                lamps[i].GetComponent<Lamp>().LampOff();
            }
        }
        
    }

    private void SetPressurePlates(bool platesOn){
        GameObject[] pressurePlates = GameObject.FindGameObjectsWithTag("PressurePlates");
        if(platesOn){
            for(int i = 0; i < pressurePlates.Length; i++){
                pressurePlates[i].GetComponent<PressurePlate>().hasElectricity = true;
            }
        }else{
            for(int i = 0; i < pressurePlates.Length; i++){
                pressurePlates[i].GetComponent<PressurePlate>().hasElectricity = false;
            }
        }
        
    }

}
