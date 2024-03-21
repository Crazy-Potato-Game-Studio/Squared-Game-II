using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private ParticleSystem lavaParticles;
    [SerializeField] GameObject particles;
    [SerializeField] AudioSource source;


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
            GetComponent<HealthManager>().PlayerDeath();
        }
    }

}
