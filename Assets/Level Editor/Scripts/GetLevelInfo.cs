using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetLevelInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName;

    void Start()
    {
        levelName.text = PlayerPrefs.GetString("CURRENT_LEVEL");
    }

    void Update()
    {
        
    }
}
