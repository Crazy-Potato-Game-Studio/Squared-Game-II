using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnionScript : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<AudioSource>().PlayOneShot(clip);
            IsOnion.isTheOnion = true;
            other.gameObject.GetComponent<IsOnion>().beOnion();
            GameObject SteamAchievementsManager = GameObject.FindGameObjectWithTag("MainCamera");
            SteamAchievementsManager.GetComponent<SteamAchievementsManager>().UnlockAchievement("onion_ach");
        }
    }
}
