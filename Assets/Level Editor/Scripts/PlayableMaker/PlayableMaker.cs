using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
namespace LevelBuilder
{
    public class PlayableMaker : MonoBehaviour
    {
        public const string PlayableKey = "Playable";
        public int spawnId = 0;
        public int exitId = 1;
        public string playableSceneName = "Scene1_Playable";
        public PlayableLevelData playableToBuild;
        public static PlayableMaker Singleton;
        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void Play()
        {
            StartCoroutine(PlayRoutine());
        }
        private IEnumerator PlayRoutine()
        {
            if (SaveLoadManager.Singleton.saveables == null) { yield return null; }
            SaveData dataToPlay = new();
            dataToPlay.saveInfo = SaveLoadManager.Singleton.currentlyLoadedSaveData.saveInfo;
            dataToPlay.saveInfo.dateAccessed = System.DateTime.Today.ToShortDateString();
            dataToPlay.levelData = new();
            foreach (var levelData in SaveLoadManager.Singleton.saveables)
            {
                dataToPlay.levelData.Add(levelData.UniqueSaveID, levelData.Save());
            }
            
            if (SaveLoadManager.Singleton.screenshotOnSave)
            {
                foreach (var objectToHide in SaveLoadManager.Singleton.objectToHideOnScreenshot)
                {
                    objectToHide.SetActive(false);
                }
                yield return new WaitForEndOfFrame();
                Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();
                dataToPlay.saveInfo.thumbnail = new(screenshot);
                Destroy(screenshot);
                foreach (var objectToHide in SaveLoadManager.Singleton.objectToHideOnScreenshot)
                {
                    objectToHide.SetActive(true);
                }
            }
            SaveLoadManager.Singleton.SaveDataToFile(dataToPlay);

            playableToBuild = MakePlayable(dataToPlay);
            if (playableToBuild != null)
            {
                SceneManager.LoadScene(playableSceneName);
                SavePlayableToFile(playableToBuild);
            }
        }
        public PlayableLevelData MakePlayable(SaveData dataToMakePlayable)
        {
            PlayableLevelData playableLevelData = GetValidPlayableLevelData(dataToMakePlayable);
            if (playableLevelData == null)
            {
                Debug.Log("Level Is Not Playable!");
                return null;
            }
            
            BakeFloor(dataToMakePlayable, playableLevelData);
            BakePlatform(dataToMakePlayable, playableLevelData);
            BakeClimbable(dataToMakePlayable, playableLevelData);
            BakeDecoration(dataToMakePlayable, playableLevelData);
            BakeOther(dataToMakePlayable, playableLevelData);
            BakeEnemy(dataToMakePlayable, playableLevelData);
            BakeInteractable(dataToMakePlayable, playableLevelData);
            BakeItem(dataToMakePlayable, playableLevelData);
            return playableLevelData;
        }

        private void SavePlayableToFile(PlayableLevelData playableLevelData)
        {
            if(playableLevelData == null) { return; }
            string playableFilePath = Application.persistentDataPath + "/Playables/";
            if (!Directory.Exists(playableFilePath)) { Directory.CreateDirectory(playableFilePath); }
            string filePath = playableFilePath + playableLevelData.playableInfo.levelName + ".playable";
            FileStream file = File.Open(filePath, FileMode.Create);
            BinaryFormatter bf = new();
            bf.Serialize(file, playableLevelData);
            file.Close();
            Debug.Log("Playable Saved");
        }

        private PlayableLevelData GetValidPlayableLevelData(SaveData currentLevelData)
        {
            if (currentLevelData.levelData.TryGetValue(SaveLoadManager.SaveInteractable, out var interactableSave))
            {
                if (interactableSave.InteractableDictionary == null)
                {
                    return null;
                }

                ObjectProperty spawn = interactableSave.InteractableDictionary.Values
                    .FirstOrDefault(interactable => interactable.id == spawnId);
                
                ObjectProperty exit = interactableSave.InteractableDictionary.Values
                    .FirstOrDefault(interactable => interactable.id == exitId);


                if (spawn == null || exit == null)
                {
                    return null;
                }

                PlayableInfo playableInfo = new()
                {
                    levelName = currentLevelData.saveInfo.saveDataFileName,
                    thumnail = currentLevelData.saveInfo.thumbnail
                };
                PlayableLevelData playableData = new();
                playableData.playableInfo = playableInfo;
                playableData.Spawn = spawn;
                playableData.Exit = exit;
                return playableData;
            }

            return null;
        }

        private void BakeFloor(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.levelData.TryGetValue(SaveLoadManager.SaveFloor, out var floorData))
            {
                playableLevelData.FloorData = floorData.FloorDictionary.Values.ToList();
            }
        }

        private void BakePlatform(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.levelData.TryGetValue(SaveLoadManager.SavePlatform, out var platformData))
            {
                playableLevelData.PlatformData = new();
                foreach (var platform in platformData.PlatformDictionary.Values)
                {
                    int id = platform.itemId;
                    foreach (var item in platform.childs)
                    {
                        TileProperty floorProperty = new(id, item.gridPos);
                        playableLevelData.PlatformData.Add(floorProperty);
                    }
                }
            }
        }

        private void BakeClimbable(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.levelData.TryGetValue(SaveLoadManager.SaveClimable, out var climbableData))
            {
                playableLevelData.ClimbableData = climbableData.ClimbableDictionary.Values.ToList();
            }
        }

        private void BakeDecoration(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.levelData.TryGetValue(SaveLoadManager.SaveDecoration, out var decorationData))
            {
                playableLevelData.DecorationData = decorationData.DecorationDictionary.Values.ToList();
            }
        }
        private void BakeEnemy(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.levelData.TryGetValue(SaveLoadManager.SaveEnemy, out var enemyData))
            {
                playableLevelData.EnemyData = enemyData.EnemyDictionary.Values.ToList();
            }
        }
        private static void BakeInteractable(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.levelData.TryGetValue(SaveLoadManager.SaveInteractable, out var interacableData))
            {
                playableLevelData.InteractableData = interacableData.InteractableDictionary.Values.ToList();
            }
        }
        private void BakeItem(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.levelData.TryGetValue(SaveLoadManager.SaveItem, out var itemData))
            {
                playableLevelData.ItemData = itemData.ItemDictionary.Values.ToList();
            }
        }
        private void BakeOther(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.levelData.TryGetValue(SaveLoadManager.SaveOther, out var otherData))
            {
                playableLevelData.OtherData = otherData.OtherDictionary.Values.ToList();
            }
        }
        public PlayableLevelData LoadPlayableDataFromFile(string fileName)
        {
            string playableDirectoryPath = Application.persistentDataPath + "/Playables/";
            if (!Directory.Exists(playableDirectoryPath)) { Directory.CreateDirectory(playableDirectoryPath); }
            string playbleFileName = playableDirectoryPath + fileName + ".playable";

            if (File.Exists(playbleFileName))
            {
                FileStream file = File.Open(playbleFileName, FileMode.Open);
                BinaryFormatter bf = new();
                PlayableLevelData playableLevelData = (PlayableLevelData)bf.Deserialize(file);
                file.Close();
                return playableLevelData;
            }
            return null;
        }
    }
    [System.Serializable]
    public class PlayableLevelData
    {
        public PlayableInfo playableInfo;
        public List<TileProperty> FloorData;
        public List<TileProperty> PlatformData;
        public List<TileProperty> ClimbableData;
        public List<TileProperty> ItemData;
        public List<DecorationProperty> DecorationData;
        public List<ObjectProperty> InteractableData;
        public List<ObjectProperty> EnemyData;
        public List<ObjectProperty> OtherData;
        public ObjectProperty Spawn;
        public ObjectProperty Exit;
    }
    [System.Serializable]
    public class PlayableInfo
    {
        public string creatorName;
        public string levelName;
        public string description;
        public LevelThumnail thumnail;
    }
}