using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScene : MonoBehaviour
{
    private bool changeScale;
    private GameObject sprite;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            sprite = GameObject.FindGameObjectWithTag("Finish");
            changeScale = true;
        }
    }

    private void Update() {
        if(changeScale){
            sprite.transform.localScale = new Vector3(sprite.transform.localScale.x - Time.deltaTime * 25, sprite.transform.localScale.x - Time.deltaTime * 25, sprite.transform.localScale.x - Time.deltaTime * 25);

            if(sprite.transform.localScale.x <= 1){
                SceneManager.LoadScene(1);
            }

        }
    }
}
