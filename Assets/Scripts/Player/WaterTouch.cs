using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTouch : MonoBehaviour
{
    private ParticleSystem waterParticles;
    [SerializeField] GameObject particles;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioSource source;

    private void Start() {
        waterParticles = particles.GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Water"){
            source.PlayOneShot(clip);
            ParticleSystem instantiatedParticles;
            instantiatedParticles = Instantiate(waterParticles, this.transform);
            instantiatedParticles.transform.parent = null;
            instantiatedParticles.Play();
            Destroy(instantiatedParticles.gameObject, 1);
        }
    }
}
