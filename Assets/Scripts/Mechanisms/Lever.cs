using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lever : MonoBehaviour
{
    public List<GameObject> obejctsToTurnOn;
    public bool isOn = false;

    [SerializeField] private Sprite leverOn;
    [SerializeField] private Sprite leverOff;
    private SpriteRenderer leverSprite;

    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;

    private GameObject player;

    private void Start() {
        if(isOn){
            SetSprite(true);
        }

        leverSprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider"){
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider"){
            player = null;
        }
    }

    private void Update() {
        if(player && Input.GetKeyDown(KeyCode.E)){
            if(!isOn){
                TurnObjects();
                isOn = true;
                SetSprite(true);
            }else{
                TurnObjects();
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

    private void TurnObjects(){
        for(int i = 0; i < obejctsToTurnOn.Count; i++){
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
}
