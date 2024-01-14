using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemy : MonoBehaviour
{
    private BoxCollider2D bc2d;
    private Animator anim;
    public Transform player;
    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player"){
            anim.SetBool("playerInDistance", true);
        }
    }
    void OnTriggerExit2D(Collider2D col){
        if(col.gameObject.tag == "Player"){
            anim.SetBool("playerInDistance", false);
        }
    }
    void Update(){
        if(player.position.x > transform.position.x){
            transform.localScale = new Vector2(-1, 1);
        }else{
            transform.localScale = new Vector2(1, 1);
        }
    }
}
