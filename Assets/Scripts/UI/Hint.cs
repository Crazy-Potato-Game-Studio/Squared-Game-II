using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Hint : MonoBehaviour
{
    public Transform hintObject;
    [SerializeField] private GameObject PcHintGFX;
    [SerializeField] private GameObject playStationHintGFX;
    [SerializeField] private GameObject xboxHintGFX;

    void Update()
    {
        if(hintObject){
            transform.parent = null;
            transform.rotation = new Quaternion(0,0,0,0);
            transform.position = new Vector2(hintObject.position.x, hintObject.position.y);
        }else{
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.tag == "ResistanceCollider"){
            SetGFX(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.tag == "ResistanceCollider"){
            SetGFX(false);
            PcHintGFX.SetActive(false);
            playStationHintGFX.SetActive(false);
            xboxHintGFX.SetActive(false);
        }
    }

    void SetGFX(bool isOn){
        if(hintObject.GetComponent<Portal>()){
            if(hintObject.GetComponent<Portal>().isOn && hintObject.GetComponent<Portal>().destinationPortal.GetComponent<Portal>().isOn){
                if(UsedDevice.usingGamepad){
                    if(Gamepad.current is DualShockGamepad){
                        playStationHintGFX.SetActive(isOn);
                    }else{
                        xboxHintGFX.SetActive(isOn);
                    }
                }else{
                    PcHintGFX.SetActive(isOn);
                }
            }
        }else{
            if(UsedDevice.usingGamepad){
                if(Gamepad.current is DualShockGamepad){
                    playStationHintGFX.SetActive(isOn);
                }else{
                    xboxHintGFX.SetActive(isOn);
                }
            }else{
                PcHintGFX.SetActive(isOn);
            }
        }

    }

}
