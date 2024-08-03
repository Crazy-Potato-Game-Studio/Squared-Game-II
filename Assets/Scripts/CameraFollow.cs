using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float smoothSpeed;
    Vector3 offset = new Vector3(0,0,-10f);

    private void Awake() {
        Transform spawn = GameObject.FindGameObjectWithTag("Respawn").transform;
        transform.position = new Vector3(spawn.transform.position.x, spawn.transform.position.y + 1, spawn.transform.position.z) + offset;
    }

    private void FixedUpdate() {
        if(player){
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, player.position, smoothSpeed);
            transform.position = smoothedPosition + offset;
        }
    }
}
