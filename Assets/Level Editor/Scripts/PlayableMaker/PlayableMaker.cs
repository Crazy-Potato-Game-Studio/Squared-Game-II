using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UIElements;
namespace LevelBuilder
{
    public static class PlayableMaker
    {
        public static void MakePlayable()
        {
            SaveData saveData = SaveLoadManager.GetSaveData();
            if(saveData != null)
            {
                PlayableLevelData playableLevelData = BakeData(saveData);
                SavePlayableToFile(playableLevelData);
            }
        }

        static PlayableLevelData BakeData(SaveData saveDataTobake)
        {
            PlayableLevelData playableLevelData = new();
            BakeFloor(saveDataTobake,playableLevelData);
            BakePlatform(saveDataTobake,playableLevelData);
            BakeClimbable(saveDataTobake,playableLevelData);
            BakeDecoration(saveDataTobake,playableLevelData);
            BakeOther(saveDataTobake, playableLevelData);
            BakeEnemy(saveDataTobake,playableLevelData);
            BakeInteractable(saveDataTobake,playableLevelData);
            BakeItem(saveDataTobake, playableLevelData);
            BakeSpawn(saveDataTobake, playableLevelData);
            BakeExit(saveDataTobake, playableLevelData);
            return playableLevelData;
        }

        private static void BakeExit(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.Data.TryGetValue("Interactable", out var interactableData))
            {
                playableLevelData.Exit = interactableData.Exit;
            }
        }

        private static void BakeSpawn(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.Data.TryGetValue("Interactable", out var interactableData))
            {
                playableLevelData.Spawn = interactableData.Spawn;
            }
        }

        static void BakeFloor(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.Data.TryGetValue("Floor", out var floorData))
            {
                playableLevelData.FloorData = new();
                foreach (var floor in floorData.FloorDictionary.Values)
                {
                    playableLevelData.FloorData.Add(floor);
                }
            }
        }

        static void BakePlatform(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.Data.TryGetValue("Platform", out var platformData))
            {
                playableLevelData.PlatformData = new();
                foreach (var platform in platformData.PlatformDictionary.Values)
                {
                    int id = platform.itemId; ;
                    foreach (var item in platform.childs)
                    {
                        FloorProperty floorProperty = new()
                        {
                            id = id,
                            gridPos = item.gridPos
                        };
                        playableLevelData.PlatformData.Add(floorProperty);
                    }
                }
            }
        }

        static void BakeClimbable(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.Data.TryGetValue("Climbable", out var climbableData))
            {
                playableLevelData.ClimbableData = new();
                foreach (var climbable in climbableData.ClimbableDictionary.Values)
                {
                    playableLevelData.ClimbableData.Add(climbable);
                }
            }
        }

        static void BakeDecoration(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.Data.TryGetValue("Decoration", out var decorationData))
            {
                playableLevelData.DecorationData = new();
                foreach (var decoration in decorationData.DecorationDictionary.Values)
                {
                    playableLevelData.DecorationData.Add(decoration);
                }
            }
        }
        static void BakeEnemy(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.Data.TryGetValue("Enemy", out var enemyData))
            {
                playableLevelData.PropertyData ??= new();
                foreach (var enemy in enemyData.EnemyDictionary.Values)
                {
                    playableLevelData.PropertyData.Add(enemy);
                }
            }
        }
        static void BakeInteractable(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.Data.TryGetValue("Interactable", out var interacableData))
            {
                playableLevelData.PropertyData ??= new();
                foreach (var interactable in interacableData.InteractableDictionary.Values)
                {
                    playableLevelData.PropertyData.Add(interactable);
                }
            }
        }
        static void BakeItem(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.Data.TryGetValue("Item", out var itemData))
            {
                playableLevelData.ItemData = new();
                foreach (var item in itemData.ItemDictionary.Values)
                {
                    playableLevelData.ItemData.Add(item);
                }
            }
        }
        static void BakeOther(SaveData saveDataTobake, PlayableLevelData playableLevelData)
        {
            if (saveDataTobake.Data.TryGetValue("Other", out var otherData))
            {
                playableLevelData.OtherData = otherData.OtherProperty;
            }
        }
        static public void SavePlayableToFile(PlayableLevelData playableLevelData)
        {
            string saveFilePath = Application.persistentDataPath + "/Playable.dat";
            FileStream file = File.Open(saveFilePath, FileMode.Create);
            BinaryFormatter bf = new();
            bf.Serialize(file, playableLevelData);
            file.Close();
        }
    }
    [System.Serializable]
    public class PlayableLevelData
    {
        public ObjectProperty Spawn;
        public ObjectProperty Exit;
        public List<FloorProperty> FloorData;
        public List<FloorProperty> PlatformData;
        public List<ClimbableProperty> ClimbableData;
        public List<DecorationProperty> DecorationData;
        public List<ObjectProperty> PropertyData;
        public List<TileProperty> ItemData;
        public OtherProperty OtherData;
    }
}