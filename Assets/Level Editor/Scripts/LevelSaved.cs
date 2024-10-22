using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSaved : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject levelSaved;

    public void ShowSavedLevelPopUp(){
        Destroy(Instantiate(levelSaved, canvas.transform), 0.7f);
    }
}
