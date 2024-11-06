using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedrunTimer : MonoBehaviour
{

    public static int timer;

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 5){
            ResetTimer();
        }
        InvokeRepeating("UpdateTimer", 0, 1);
    }

    public void UpdateTimer(){
        timer++;
    } 

    public void ResetTimer(){
        timer = 0;
    }
}
