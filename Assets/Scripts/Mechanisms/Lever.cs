using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lever : MonoBehaviour
{
    public bool isOn = false;

    [SerializeField] private Sprite leverOn;
    [SerializeField] private Sprite leverOff;
    private SpriteRenderer leverSprite;

    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;
    private GameObject player;

    private bool playerInRange;

    private void Start() {
        if(isOn){
            SetSprite(true);
        }

        leverSprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider"){
            playerInRange = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "ResistanceCollider"){
            playerInRange = false;
        }
    }

    private void Update() {
        if(playerInRange && Input.GetKeyDown(KeyCode.E)){
            if(!isOn){
                ChangeGravityDir();
                isOn = true;
                SetSprite(true);
            }else{
                ChangeGravityDir();
                isOn = false;
                SetSprite(false);
            }
            source.PlayOneShot(clip);
        }

        if(Physics2D.gravity.y < 0){
            isOn = true;
            SetSprite(true);
        }else{
            isOn = false;
            SetSprite(false);
        }
    }

    private void ChangeGravityDir(){
        Physics2D.gravity *= -1;
        MirrorObjects();
    }

    private void MirrorObjects(){
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().gravityUp = !player.GetComponent<PlayerMovement>().gravityUp;
        player.GetComponent<Transform>().localScale = new Vector3(1, player.GetComponent<Transform>().localScale.y * -1, 1);
    }

    private void SetSprite(bool spriteOn){
        if(spriteOn){
            leverSprite.sprite = leverOn;
        }else{
            leverSprite.sprite = leverOff;
        }
    }

}
