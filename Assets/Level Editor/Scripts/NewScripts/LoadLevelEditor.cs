using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelEditor : MonoBehaviour
{
    public void LoadEditor(){
        Time.timeScale = 1;
        SceneManager.LoadScene("Scene0_CreateScene");
    }
}
