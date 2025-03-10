using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

public class GetLevelInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI hoursText;
    private int hours;
    private int timeSpent;

    void Start()
    {
        levelName.text = PlayerPrefs.GetString("CURRENT_LEVEL");
        hoursText.text = "0 h";
        timeText.text = "0 min";
        LoadAdditionalDataFromFile();
        ChangeText();
        InvokeRepeating("ChangeTimeValue", 60, 60);
    }

    public void ChangeText(){
        hours = timeSpent/60;
        if(hours != 0){
            hoursText.text = hours.ToString() + " h";
        }else{
            hoursText.text = "0 h";
        }
        timeText.text = timeSpent - (hours * 60) + " min";
    }

    public void LoadAdditionalDataFromFile(){
        string destination = Application.persistentDataPath + "/Level Editor/" + levelName.text + ".potato";
        FileStream file;

        if(File.Exists(destination)){
            file = File.OpenRead(destination);
        }else{
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        AdditionalLevelInfo additionalLevelInfo = (AdditionalLevelInfo) bf.Deserialize(file);
        file.Close();
        timeSpent = additionalLevelInfo.timeSpentEditing;
    }
}

[System.Serializable]
public class AdditionalLevelInfo{
    public int timeSpentEditing;

    public AdditionalLevelInfo(int time){
        timeSpentEditing = time;
    }
}