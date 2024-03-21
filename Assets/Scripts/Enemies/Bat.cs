using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Bat : MonoBehaviour
{
    [SerializeField] private AIPath aiPath;
    [SerializeField] private Transform returnPoint;
    [SerializeField] private GameObject enemyCanvas;
    private GameObject player;
    private float distance;

    private void Awake() {
        GetComponent<AIDestinationSetter>().target = returnPoint;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update(){
        if(player){

            distance = Vector2.Distance(transform.position, player.transform.position);

            if(distance < 13f){
                GetComponent<AIDestinationSetter>().target = player.transform;
            }else if(distance > 13f ){
                GetComponent<AIDestinationSetter>().target = returnPoint;
            }
        }
        

        if(aiPath.desiredVelocity.x >= 0.01f){
            transform.localScale = new Vector2(-1f, 1f);
            enemyCanvas.GetComponent<ScaleChanger>().ChangeUIScale(transform.localScale.x);
        }else if(aiPath.desiredVelocity.x <= 0.01f){
            transform.localScale = new Vector2(1f, 1f);
            enemyCanvas.GetComponent<ScaleChanger>().ChangeUIScale(transform.localScale.x);
        }
    }
}
