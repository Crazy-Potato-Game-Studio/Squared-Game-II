using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private AudioClip earrape;

    void Start()
    {
        StartCoroutine(StartEarrape());
    }

    private IEnumerator StartEarrape(){
        yield return new WaitForSeconds(500);
        GameObject sceneManager = GameObject.FindGameObjectWithTag("SceneManager");
        sceneManager.GetComponent<SceneManagement>().PlayMusic(earrape);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<IsOnion>().beOnion();
        IsOnion.isTheOnion = true;
    }

}
