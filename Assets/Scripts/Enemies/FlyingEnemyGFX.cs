using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemyGFX : MonoBehaviour
{
    [SerializeField] AIPath aiPath;
    [SerializeField] Transform returnPoint;
    [SerializeField] GameObject player;
    private float distance;

    private void Start() {
        GetComponent<AIDestinationSetter>().target = returnPoint;
    }

    void Update(){

        distance = Vector2.Distance(transform.position, player.transform.position);

        if(distance < 13f ){
            GetComponent<AIDestinationSetter>().target = player.transform;
        }else if(distance > 13f ){
            GetComponent<AIDestinationSetter>().target = returnPoint;
        }

        if(aiPath.desiredVelocity.x >= 0.01f){
            transform.localScale = new Vector2(-1f, 1f);
        }else if(aiPath.desiredVelocity.x <= 0.01f){
            transform.localScale = new Vector2(1f, 1f);
        }
    }
}
