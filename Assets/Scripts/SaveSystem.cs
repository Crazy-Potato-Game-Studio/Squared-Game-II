using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveProgress(ItemsCounter itemsCounter){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.potato";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(itemsCounter);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadData(){
        string path = Application.persistentDataPath + "/player.potato";
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            
            return data;
        }else{
            Debug.Log("Save file not found in "+path);  
            return null;
        }
    }
}
