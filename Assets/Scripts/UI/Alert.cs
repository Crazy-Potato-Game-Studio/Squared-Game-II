using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour
{
    [SerializeField] GameObject spriteRenderer; 
    private GameObject player;

    float alertDistance;
    [SerializeField] float agroDistance = 5f;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        HideAlert();
    }

    
    void Update()
    {
        DistanceCheck();
        spriteRenderer.transform.localScale = new Vector2(1,1);
        spriteRenderer.transform.position = new Vector2(transform.position.x,transform.position.y + 1f);

        spriteRenderer.transform.eulerAngles = new Vector3(0,0,0);


        if(agroDistance > alertDistance){
            ShowAlert();
        }else{
            HideAlert();
        }
    }

    void DistanceCheck(){
        alertDistance = Vector2.Distance(player.transform.position, transform.position);
    }

    void ShowAlert(){
        spriteRenderer.GetComponent<SpriteRenderer>().enabled = true;
    }

    void HideAlert(){
        spriteRenderer.GetComponent<SpriteRenderer>().enabled = false;
    }
}
