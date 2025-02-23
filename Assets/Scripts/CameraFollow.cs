using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float smoothSpeed;
    GameObject spawn;
    Vector3 offset = new Vector3(0,0,-10f);

    private void Awake() {
        spawn = GameObject.FindGameObjectWithTag("Respawn");

        if(spawn){
            transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y + 1, spawn.transform.position.z) + offset;
        }

        if(!player){
            LookForPlayer();
        }
    }

    private void FixedUpdate() {
        if(player){
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, player.position, smoothSpeed);
            transform.position = smoothedPosition + offset;
        }else{
            LookForPlayer();
        }
    }

    private void LookForPlayer(){
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerObject){
            player = playerObject.transform;
        }
    }
}
