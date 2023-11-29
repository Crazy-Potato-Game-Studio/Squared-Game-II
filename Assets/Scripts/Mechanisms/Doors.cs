using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float distanceToOpen;
    [SerializeField] private string keyColor;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;
    [SerializeField] private SpriteRenderer keyIcon;
    private bool doorsOpen = false;
    private float distance;
    private GameObject player;
    private bool playerHasAKey;
    
    private void Start() {
        SetDoorsBool();
        keyIcon.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OpenDoors(){
        doorsOpen = true;
        SetDoorsBool();
    }

    void CloseDoors(){
        doorsOpen = false;
        SetDoorsBool();
    }

    void SetDoorsBool(){
        animator.SetBool("DoorsOpen", doorsOpen);
    }

    float CalculateDistance(){
        distance = Vector2.Distance(transform.position, player.transform.position);

        return distance;
    }

    private void Update() {
        if(CalculateDistance() < distanceToOpen){
            if(!doorsOpen){
                ShowKeyIcon();
            }else{
                keyIcon.enabled = false;
            }
            if(Input.GetKeyDown(KeyCode.E) && PlayerHasAKey()){
                OpenDoors();
                source.PlayOneShot(clip);
                GameObject.Destroy(GameObject.Find(keyColor + "Item(Clone)"));
            }
        }else{
            keyIcon.enabled = false;
        }
    }

    bool PlayerHasAKey(){
        
        if(GameObject.Find(keyColor + "Item(Clone)") != null){
            playerHasAKey = true;
        }else{
            playerHasAKey = false;
        }

        return playerHasAKey;
    }

    void ShowKeyIcon(){
        keyIcon.enabled = true;
    }
}
