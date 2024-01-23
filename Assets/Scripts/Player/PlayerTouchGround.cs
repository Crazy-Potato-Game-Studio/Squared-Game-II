using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchGround : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject trampolineParticles;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Ground" || other.gameObject.tag == "Cube"){
            GameObject currentParticles = Instantiate(particles, GetContactPosition(other), other.transform.rotation);
            currentParticles.transform.parent = null;
            Destroy(currentParticles, 1f);
        }
        if(other.gameObject.tag == "Trampoline"){
            GameObject currentParticles = Instantiate(trampolineParticles, GetContactPosition(other), other.transform.rotation);
            currentParticles.transform.parent = null;
            Destroy(currentParticles, 1f);
        }
    }

    private Vector2 GetContactPosition(Collision2D other){
        Vector2 sum = new Vector2(0,0);
        for(int i = 0; i < other.contacts.Length; i++){
            sum += other.contacts[i].point;
        }
        Vector2 contactPosition = sum / other.contacts.Length;
        return contactPosition;
    }
}
