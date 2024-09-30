using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace LevelBuilder
{
    public class SaveLoadManager : MonoBehaviour
    {
        [SerializeField] public bool loadLevelOnStart;
        public static SaveLoadManager Singleton;
        public List<ISaveLoad> saveLoadList = new();
        private void Awake()
        {
            Singleton = this;
        }

        private void Start()
        {
            if(loadLevelOnStart) { LoadDataFromFile(); }
        }
        public void LoadDataFromFile()
        {
            SaveData saveData = GetSaveData();
            if(saveData == null) { return; }
            foreach (var saveLoad in saveLoadList)
            {
                if (saveData.Data.TryGetValue(saveLoad.UniqueSaveID, out LevelSave mapSave))
                {
                    saveLoad.Load(mapSave);
                }
            }
        }

        public static SaveData GetSaveData()
        {
            string loadFilePath = Application.persistentDataPath + "/Level Editor/Levels/" + PlayerPrefs.GetString("CURRENT_LEVEL") + ".dat";
            if (File.Exists(loadFilePath))
            {
                FileStream file = File.Open(loadFilePath, FileMode.Open);
                BinaryFormatter bf = new();
                SaveData saveData = (SaveData)bf.Deserialize(file);
                file.Close();
                return saveData;
            }
            return null;
        }

        public void SaveDataToFile()
        {
            SaveData saveData = new() { Data = new() };
            foreach (var saveLoad in saveLoadList)
            {
                saveData.Data.Add(saveLoad.UniqueSaveID, saveLoad.Save());
            }

            string saveFilePath = Application.persistentDataPath + "/Level Editor/Levels/" + PlayerPrefs.GetString("CURRENT_LEVEL") + ".dat";
            FileStream file = File.Open(saveFilePath, FileMode.Create);
            BinaryFormatter bf = new();
            bf.Serialize(file, saveData);
            file.Close();
        }
    }

    [System.Serializable]
    public class LevelSave
    {
        public ObjectProperty Spawn;
        public ObjectProperty Exit;
        public string backGroundMusic;
        public byte[] thumbnail;
        public Dictionary<string, FloorProperty> FloorDictionary;
        public Dictionary<string, PlatformParentProperty> PlatformDictionary;
        public Dictionary<string, ClimbableProperty> ClimbableDictionary;
        public Dictionary<string, DecorationProperty> DecorationDictionary;
        public Dictionary<string, ObjectProperty> EnemyDictionary;
        public Dictionary<string, ObjectProperty> InteractableDictionary;
        public Dictionary<string, TileProperty> ItemDictionary;
        public OtherProperty OtherProperty;
    }
    public interface ISaveLoad
    {
        string UniqueSaveID { get; set; }
        public LevelSave Save();
        public void Load(LevelSave mapSave);
    }
    [System.Serializable]

    public class SaveData
    {
        public Dictionary<string, LevelSave> Data;
    }
    
}

