using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackNameScript : MonoBehaviour
{
    public AudioClip soundtrack;
    private string soundtrackName;

    private void Awake() {

        if(soundtrack){
            soundtrackName = soundtrack.name;
        }else{
            Debug.Log("NO MUSIC IS PLAYING");
        }

        Search();
    }

    IEnumerator SearchForSceneManager(){
        yield return new WaitForSeconds(1f);

        Search();
    }

    void Search(){
        GameObject _sceneManagement = GameObject.FindGameObjectWithTag("SceneManager");

        if(_sceneManagement){
            if (_sceneManagement.GetComponent<SceneManagement>().musicName != soundtrackName)
            {
                _sceneManagement.GetComponent<SceneManagement>().PlayMusic(soundtrack);
            }
        }else{
            StartCoroutine(SearchForSceneManager());
        }
    }
}
