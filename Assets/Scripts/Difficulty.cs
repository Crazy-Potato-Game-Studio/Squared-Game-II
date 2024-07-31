using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    [SerializeField] private Transform tick;

    [SerializeField] private GameObject easyButton;
    [SerializeField] private GameObject normalButton;
    [SerializeField] private GameObject hardButton;
    [SerializeField] private GameObject impButton;

    private void Awake() {
        ChangeDifficultLevel(Bow.difficultyLevel);
    }

    public void ChangeDifficultLevel(float difLevel){
        Bow.difficultyLevel = difLevel;

        switch (difLevel)
        {
            case 2:
            tick.position = new Vector3(easyButton.transform.position.x, easyButton.transform.position.y - 40, easyButton.transform.position.z);
            break;
            case 1:
            tick.position = new Vector3(normalButton.transform.position.x, normalButton.transform.position.y - 40, normalButton.transform.position.z);
            break;
            case 0.75f:
            tick.position = new Vector3(hardButton.transform.position.x, hardButton.transform.position.y - 40, hardButton.transform.position.z);
            break;
            case 0.5f:
            tick.position = new Vector3(impButton.transform.position.x, impButton.transform.position.y - 40, impButton.transform.position.z);
            break;
            default:
            break;
        }
    }
}
