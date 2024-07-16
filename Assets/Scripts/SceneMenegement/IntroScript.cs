using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    private void Awake() {
        if(SceneManager.GetActiveScene().buildIndex == 0){
            StartCoroutine("LoadGame");
        }
    }

    private IEnumerator LoadGame(){
        yield return new WaitForSeconds(5.7f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
