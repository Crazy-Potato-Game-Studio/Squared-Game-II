using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] string levelAndLocation;
    [SerializeField] GameObject textLvl;
    private Transform textLvlViewer;

    private void Awake() {
        textLvlViewer = GameObject.FindGameObjectWithTag("TextLvlViewer").transform;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            other.GetComponent<BoxCollider2D>().isTrigger = false;

            BoxCollider2D col = GetComponent<BoxCollider2D>();
            Destroy(col);

            GetComponent<AudioSource>().Play();

            GameObject text = Instantiate(textLvl, textLvlViewer);
            text.GetComponent<TextMeshProUGUI>().text = levelAndLocation;
            Destroy(text, 3f);
        }
    }
}
