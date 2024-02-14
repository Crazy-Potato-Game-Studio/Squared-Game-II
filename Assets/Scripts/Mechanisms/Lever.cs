using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lever : MonoBehaviour
{
    private bool playerInRange = false;
    public GameObject whatToTurnOn;
    [SerializeField] private Sprite leverOn;
    [SerializeField] private Sprite leverOff;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source; 
    [SerializeField] private bool isOn = false;
    private SpriteRenderer leverSprite;
    private GameObject player;

    private void Start() {
        if(isOn){
            SetSprite(true);
        }

        leverSprite = GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private bool PlayerInRange(){
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if(distance <= 4f){
            playerInRange = true;
        }else{
            playerInRange = false;
        }
        return playerInRange;
    }

    private void Update() {
        if(PlayerInRange() && Input.GetKeyDown(KeyCode.E)){
            if(!isOn){
                TurnObjectOn();
                isOn = true;
                SetSprite(true);
            }else{
                TurnObjectOff();
                isOn = false;
                SetSprite(false);
            }
            source.PlayOneShot(clip);
        }
    }

    private void SetSprite(bool spriteOn){
        if(spriteOn){
            leverSprite.sprite = leverOn;
        }else{
            leverSprite.sprite = leverOff;
        }
    }

    private void TurnObjectOn(){
        if(whatToTurnOn.GetComponent<Portal>() != null){
            whatToTurnOn.GetComponent<Portal>().TurnOn();
        }else if(whatToTurnOn.GetComponent<Doors>() != null){
            whatToTurnOn.GetComponent<Doors>().OpenDoors();
        }
    }

    private void TurnObjectOff(){
        if(whatToTurnOn.GetComponent<Portal>() != null){
            whatToTurnOn.GetComponent<Portal>().TurnOff();
        }else if(whatToTurnOn.GetComponent<Doors>() != null){
            whatToTurnOn.GetComponent<Doors>().CloseDoors();
        }
    }
}
