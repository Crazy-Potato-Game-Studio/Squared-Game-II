using System.Collections;
using System.Collections.Generic;
using LevelBuilder;
using UnityEngine;

public class PopulateUI : MonoBehaviour
{
    [SerializeField] private GameObject LevelCreateUIManager;

    private void Awake() {
        LevelCreateUIManager.GetComponent<LevelEditorMenuManager>().LoadDataFromFile();
        LevelCreateUIManager.GetComponent<LevelEditorMenuManager>().PopulateLevelsUI();
    }
}
