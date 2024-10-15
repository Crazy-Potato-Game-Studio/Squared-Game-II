using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayCurrentLevel : MonoBehaviour
{

    public void PlayLevel(){
        SceneManager.LoadScene("CURRENT_LEVEL");
    }

}
