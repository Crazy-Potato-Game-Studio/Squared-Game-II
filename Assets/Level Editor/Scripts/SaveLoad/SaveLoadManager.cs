using LevelBuilder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class SaveLoadManager :MonoBehaviour
{
    [SerializeField]private string saveFileToLoad = "No Name";
    [field: SerializeField] public bool screenshotOnSave { get; private set; }
    public GameObject[] objectToHideOnScreenshot;
    public List<ISaveLoad> saveables { get; private set; } = new();
    public static SaveLoadManager Singleton;
    [field:SerializeField]public SaveData currentlyLoadedSaveData { get; private set; }
    public Action<SaveData> OnLoad;
    public Action<SaveData> OnSaveDataUpdate;
    public const string SaveFloor = "SaveFloor";
    public const string SavePlatform = "SavePlatform";
    public const string SaveClimable = "SaveClimable";
    public const string SaveInteractable = "SaveInteractable";
    public const string SaveOther = "SaveOther";
    public const string SaveEnemy = "SaveEnemy";
    public const string SaveDecoration = "SaveDecoration";
    public const string SaveItem = "SaveItem";

    public const string DirectorySave = "/Level Editor/Saves/";

    public const string LevelOpenDateKey = "LevelOpenDate";
    public const string LevelDataFileKey = "LevelData";
    private void Awake()
    {
        if (Singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            Singleton = this;
        }
        else Destroy(this.gameObject);
    }
    public void SetSaveToLoad(string s) => this.saveFileToLoad = s;

    public void Load()
    {
        saveables = new List<ISaveLoad>(FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>());

        if (saveables == null) { return; }
        currentlyLoadedSaveData = GetSaveDataFromFile(saveFileToLoad);
        if (currentlyLoadedSaveData == null)
        {
            SaveData newSave = new SaveData();
            newSave.saveInfo = new(saveFileToLoad);
            newSave.saveInfo.dateAccessed = System.DateTime.Today.ToShortDateString();
            currentlyLoadedSaveData = newSave;
            SaveDataToFile(newSave);
            return;
        }

        foreach (var saveLoad in saveables)
        {
            if (currentlyLoadedSaveData.levelData.TryGetValue(saveLoad.UniqueSaveID, out LevelData mapSave))
            {
                saveLoad.Load(mapSave);
            }
        }
        OnLoad?.Invoke(currentlyLoadedSaveData);
        Debug.Log("Loaded");
    }
    public void Save() => StartCoroutine(SaveRoutine());
    private IEnumerator SaveRoutine()
    {
        if (screenshotOnSave)
        {
            foreach (var objectToHide in objectToHideOnScreenshot)
            {
                objectToHide.SetActive(false);
            }
            yield return new WaitForEndOfFrame();
            Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();
            currentlyLoadedSaveData.saveInfo.thumbnail = new(screenshot);
            Destroy(screenshot);
            foreach (var objectToHide in objectToHideOnScreenshot)
            {
                objectToHide.SetActive(true);
            }
        }
        currentlyLoadedSaveData.saveInfo.dateAccessed = System.DateTime.Today.ToShortDateString();
        currentlyLoadedSaveData.levelData = new();
        foreach (var saveLoad in saveables)
        {
            currentlyLoadedSaveData.levelData.Add(saveLoad.UniqueSaveID, saveLoad.Save());
        }
        SaveDataToFile(currentlyLoadedSaveData);
    }

    public void SaveDataToFile(SaveData saveData)
    {
        string saveDirectory = Application.persistentDataPath + DirectorySave;
        if (!Directory.Exists(saveDirectory)) { Directory.CreateDirectory(saveDirectory); }
        string saveFilePath = saveDirectory + saveData.saveInfo.saveDataFileName + ".dat";

        FileStream file = File.Open(saveFilePath, FileMode.Create);
        BinaryFormatter bf = new();
        bf.Serialize(file, saveData);
        file.Close();
        Debug.Log("Saved!");
    }
    public SaveData GetSaveDataFromFile(string saveFileName)
    {
        if(saveFileName == null) { return null; }
        string loadFilePath = Application.persistentDataPath + DirectorySave + saveFileName +".dat";
        if (!File.Exists(loadFilePath)) { return null; }

        FileStream file = File.Open(loadFilePath, FileMode.Open);
        BinaryFormatter bf = new();
        SaveData saveData = (SaveData)bf.Deserialize(file);
        file.Close();
        return saveData;
    }

    public Sprite GetThumbnailSprite(LevelThumnail thumbnailData)
    {
        if (thumbnailData == null || thumbnailData.data == null || thumbnailData.data.Length == 0)
            return null;
        Debug.Log("Sprite Created");
        Texture2D texture = new Texture2D(thumbnailData.width, thumbnailData.height, TextureFormat.RGB24, false);
        texture.LoadImage(thumbnailData.data);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    public List<SaveData> GetSavesInfo()
    {
        List<SaveData> saveDatas = new();
        string saveDirectory = Application.persistentDataPath + DirectorySave;

        if (!Directory.Exists(saveDirectory)) { Directory.CreateDirectory(saveDirectory); }

        string[] savesDirectory = Directory.GetFiles(saveDirectory, "*.dat");
        foreach (string fileDirectory in savesDirectory)
        {
            string fileName = Path.GetFileNameWithoutExtension(fileDirectory);
            SaveData saveData = GetSaveDataFromFile(fileName);
            saveDatas.Add(saveData);
        }
        return saveDatas;
    }

    public void DeleteSave(string saveDataFileName)
    {
        if(saveDataFileName == null) { return; }
        string loadFilePath = Application.persistentDataPath + DirectorySave + saveDataFileName + ".dat";
        if (!File.Exists(loadFilePath)) { return; }
        File.Delete(loadFilePath);
    }
}


public interface ISaveLoad
{
    string UniqueSaveID { get; }
    public LevelData Save();
    public void Load(LevelData mapSave);
}
[System.Serializable]

public class SaveData
{
    public SaveInfo saveInfo;
    public Dictionary<string, LevelData> levelData;
    public SaveData()
    {
        levelData = new();
    }
}
[System.Serializable]
public class LevelData
{
    public Dictionary<string, TileProperty> FloorDictionary;
    public Dictionary<string, PlatformParentProperty> PlatformDictionary;
    public Dictionary<string, TileProperty> ClimbableDictionary;
    public Dictionary<string, DecorationProperty> DecorationDictionary;
    public Dictionary<string, ObjectProperty> EnemyDictionary;
    public Dictionary<string, ObjectProperty> InteractableDictionary;
    public Dictionary<string, TileProperty> ItemDictionary;
    public Dictionary<string, ObjectProperty> OtherDictionary;
}

[System.Serializable]
public class SaveInfo
{
    public string saveDataFileName;
    public string dateAccessed;
    public LevelThumnail thumbnail;
    public Dictionary<string, float> floatValues;
    public Dictionary<string, string> stringValues;
    public SaveInfo(string levelDataFileName,Texture2D screenshot = null)
    {
        this.saveDataFileName = levelDataFileName;
        this.dateAccessed = DateTime.Today.ToString();
        thumbnail = new(screenshot);
    }
}

[System.Serializable]
public class LevelThumnail
{
    public int height;
    public int width;
    public byte[] data;
    public LevelThumnail(Texture2D screenshot)
    {
        if(screenshot == null) { return; }
        Texture2D screenshotTexture = new(screenshot.width, screenshot.height, TextureFormat.RGB24, false);
        screenshotTexture.SetPixels(screenshot.GetPixels());
        screenshotTexture.Apply();
        data = screenshotTexture.EncodeToPNG();
        height = screenshotTexture.height;
        width = screenshotTexture.width;
    }
}