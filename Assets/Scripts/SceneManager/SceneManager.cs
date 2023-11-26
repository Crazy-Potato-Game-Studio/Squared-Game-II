using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;


public class SceneManager : MonoBehaviour
{
    public TextMeshProUGUI hint;
    void Start(){
        Destroy(hint, 5);
    }

    void Exit(){

        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
            UnityEngine.Debug.Log("dupa");
        }

    }

    void DeleteHint(){

    }

    void ReloadScene(){
        if(Input.GetKeyDown(KeyCode.R)){
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            UnityEngine.Debug.Log("placki");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Exit();
        ReloadScene();
    }
}
