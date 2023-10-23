using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTouch : MonoBehaviour
{
    private ParticleSystem lavaParticles;
    [SerializeField] GameObject particles;
    [SerializeField] AudioSource source;
    private bool isOutsideLava = true;

    private void Start() {
        lavaParticles = particles.GetComponent<ParticleSystem>();
        source.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Lava"){
            ParticleSystem instantiatedParticles;
            instantiatedParticles = Instantiate(lavaParticles, this.transform);
            instantiatedParticles.transform.parent = null;
            instantiatedParticles.Play();
            Destroy(instantiatedParticles.gameObject, 1);
            GetComponent<HealthManager>().StartLavaDamage();
            source.enabled = true;
            source.volume = 1;
            source.Play();
            isOutsideLava = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Lava"){
            isOutsideLava = true;
            GetComponent<HealthManager>().StopLavaDamage();
            if(source.volume <= 0){
                source.enabled = false;
            }
        }
    }

    /* private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Lava"){
            GetComponent<HealthManager>().LoseHealth(0.02f);
        }
    }*/

    private void Update() {
        if(isOutsideLava){
            source.volume -= Time.deltaTime;
        }
    }
}
