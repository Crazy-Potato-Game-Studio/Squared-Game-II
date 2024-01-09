using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed;
    Vector3 offset = new Vector3(0,0,-10f);

    private void FixedUpdate() {
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, player.position, smoothSpeed);
        transform.position = smoothedPosition + offset;
    }
}
