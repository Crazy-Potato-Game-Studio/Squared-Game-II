using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder
{
    public class PlayableLevelBuilder : MonoBehaviour
    {
        [Header("Tilemaps")]
        public Tilemap floorTileMap;
        public Tilemap platformTilemap;
        public Tilemap climbableTileMap;
        public Tilemap decorationTileMap;
        public Tilemap spikeTileMap;
        [Header("Item Scriptable Objects")]
        public FloorSO floorSO;
        public PlatformSO platformSO;
        public DecorationSO decorationSO;
        public ItemSO decorationItemSO;
        public ItemSO otherItemSO;
        public ItemSO enemyItemSO;
        public ItemSO interactableItemSO;
        public ItemSO itemSO;
        public SpawnRe spawner;
        public Exit exit;
        Dictionary<string, GameObject> doorDictionary = new();
        Dictionary<ObjectProperty, GameObject> triggerDictionary = new();

        private void Start()
        {
            BuildLevel();
            StartGame();
        }
        void BuildLevel()
        {
            PlayableLevelData playableLevelData = LoadPlayableDataFromFile();
            if (playableLevelData != null)
            {
                Dictionary<string, FloorProperty> floorDictionary =
                BuildFloor(playableLevelData);
                BuildSpawn(playableLevelData);
                BuildExit(playableLevelData);
                BuildPlatform(playableLevelData, floorDictionary);
                BuildDecoration(playableLevelData);
                BuildClimbable(playableLevelData);
                BuildOther(playableLevelData);
                BuildItem(playableLevelData);
                BuildObjects(playableLevelData);
                LinkTriggers();
            }
        }
        private void StartGame()
        {
            spawner.StartGame();
        }

        private void BuildExit(PlayableLevelData playableLevelData)
        {
            LevelEditorItem exitItem = interactableItemSO.items[playableLevelData.Exit.id];
            ObjectTransformValue adjustValue = GetObjectTransformAdjustValue(exitItem, playableLevelData.Exit.state);
            exit = Instantiate(exitItem.gamePrefab, new Vector3(playableLevelData.Exit.position.x + adjustValue.positionX, playableLevelData.Exit.position.y + adjustValue.positionY, 0), Quaternion.Euler(new(0, 0, adjustValue.rotationZ))).GetComponent<Exit>();
        }

        private void BuildSpawn(PlayableLevelData playableLevelData)
        {
            Debug.Log(playableLevelData.Spawn);
            LevelEditorItem spawnItem = interactableItemSO.items[playableLevelData.Spawn.id];
            ObjectTransformValue adjustValue = GetObjectTransformAdjustValue(spawnItem, playableLevelData.Spawn.state);
            spawner = Instantiate(spawnItem.gamePrefab, new Vector3(playableLevelData.Spawn.position.x + adjustValue.positionX, playableLevelData.Spawn.position.y + adjustValue.positionY, 0), Quaternion.Euler(new(0, 0, adjustValue.rotationZ))).GetComponent<SpawnRe>();
        }

        public PlayableLevelData LoadPlayableDataFromFile()
        {
            string loadFilePath = Application.persistentDataPath + "/Playable.dat";
            if (File.Exists(loadFilePath))
            {
                FileStream file = File.Open(loadFilePath, FileMode.Open);
                BinaryFormatter bf = new();
                PlayableLevelData playableLevelData = (PlayableLevelData)bf.Deserialize(file);
                file.Close();
                return playableLevelData;

            }
            return null;

        }

        private Dictionary<string, FloorProperty> BuildFloor(PlayableLevelData playableLevelData)
        {
            Dictionary<string, FloorProperty> floorDictionary = new();
            foreach (var floor in playableLevelData.FloorData)
            {
                floorDictionary.Add(TileHandler.GetTileKey(floor.gridPos.x, floor.gridPos.y), floor);
                TileHandler.ConnectFloorTile(floorSO, floorTileMap, floorDictionary, floor.id, new(floor.gridPos.x, floor.gridPos.y, 0));
            }
            return floorDictionary;
        }

        private void BuildPlatform(PlayableLevelData playableLevelData,Dictionary<string, FloorProperty> floorDictionary)
        {
            foreach (var platform in playableLevelData.PlatformData)
            {
                TileHandler.ConnectPlatformTile(platform.id,new(platform.gridPos.x, platform.gridPos.y,0),floorDictionary,platformTilemap, platformSO);
            }
        }
        
        private void BuildDecoration(PlayableLevelData playableLevelData)
        {
            foreach (var decoration in playableLevelData.DecorationData)
            {
                LevelEditorItem item = decorationItemSO.items[decoration.itemId];
                Tile[] tiles = decorationSO.details[decoration.itemId].tiles;
                GameObject decoObjToSpawn = decorationSO.details[decoration.itemId].prefab;

                for (int i = decoration.startPos.y, tileIndex = 0; i < decoration.startPos.y + 3; i++, tileIndex++)
                {
                    if (tiles == null || tiles.Length <= 0) { continue; }
                    decorationTileMap.SetTile(new(decoration.startPos.x, i), tiles[tileIndex]);
                }
                if (decoObjToSpawn)
                {
                    Instantiate(item.gamePrefab, new(decoration.startPos.x + item.objectOffset.x, decoration.startPos.y + item.objectOffset.y), Quaternion.identity);
                }
            }
        }

        private void BuildClimbable(PlayableLevelData playableLevelData)
        {
            foreach (var climbable in playableLevelData.ClimbableData)
            {
                climbableTileMap.SetTile(new(climbable.gridPos.x,climbable.gridPos.y), otherItemSO.items[climbable.itemId].tile);
            }
        }

        public void BuildObjects(PlayableLevelData playableLevelData)
        {
            foreach (var propertyObject in playableLevelData.PropertyData)
            {
                if (propertyObject.category == ItemCategory.Enemy)
                {
                    BuildEnemy(propertyObject);
                }
                else if (propertyObject.category == ItemCategory.Interactable)
                {
                    BuildInteractable(propertyObject);
                }
            }
        }

        private void BuildEnemy(ObjectProperty propertyObject)
        {
            LevelEditorItem enemyItem = enemyItemSO.items[propertyObject.id];
            Vector2 offsetPos = new(enemyItem.objectOffset.x, enemyItem.objectOffset.y);
            if (propertyObject.state == -1)
            {
                offsetPos = new(enemyItem.extra.leftPos.x, enemyItem.extra.leftPos.y);
            }
            GameObject enemyObject = Instantiate(enemyItem.gamePrefab, new(propertyObject.position.x + offsetPos.x, propertyObject.position.y + offsetPos.y), Quaternion.identity);
            enemyObject.transform.localScale = new(propertyObject.state, enemyObject.transform.localScale.y, enemyObject.transform.localScale.z);
        }
        private void BuildInteractable(ObjectProperty propertyObject)
        {
            LevelEditorItem interactableItem = interactableItemSO.items[propertyObject.id];
            ObjectTransformValue adjustValue = GetObjectTransformAdjustValue(interactableItem, propertyObject.state);
            GameObject interactableObject = Instantiate(interactableItem.gamePrefab, new Vector3(propertyObject.position.x + adjustValue.positionX, propertyObject.position.y + adjustValue.positionY, 0), Quaternion.Euler(new(0, 0, adjustValue.rotationZ)));
            if (interactableItem.extra.propertyType == PropertyType.Trigger)
            {
                triggerDictionary.Add(propertyObject,interactableObject);
            }
            if (interactableItem.extra.propertyType == PropertyType.Door)
            {
                doorDictionary.Add(propertyObject.key, interactableObject);
            }
        }
        private void BuildItem(PlayableLevelData playableLevelData)
        {
            foreach (var item in playableLevelData.ItemData)
            {
                LevelEditorItem itemDetails = itemSO.items[item.id];
                Instantiate(itemDetails.gamePrefab, new(item.position.x + itemDetails.objectOffset.x, item.position.y + itemDetails.objectOffset.y), Quaternion.identity);
            }
        }

        public void BuildOther(PlayableLevelData playableLevelData)
        {
            foreach (var spike in playableLevelData.OtherData.spikeTileProperty)
            {
                LevelEditorItem item = otherItemSO.items[spike.Id];
                spikeTileMap.SetTile(new(spike.position.x, spike.position.y, 0), item.tile);
                float rotationZ = 0;
                if (spike.state == 0)
                {
                    rotationZ = 0;
                }
                else if (spike.state == 1)
                {
                    rotationZ = -90;
                }
                else if (spike.state == 2)
                {
                    rotationZ = -180;
                }
                else if (spike.state == 3)
                {
                    rotationZ = 90;
                }
                Quaternion rotation = Quaternion.Euler(0, 0, rotationZ);
                Matrix4x4 matrix = Matrix4x4.Rotate(rotation);
                spikeTileMap.SetTransformMatrix(new(spike.position.x, spike.position.y, 0), matrix);
            }
            foreach (var trampoline in playableLevelData.OtherData.tramploingProperty)
            {
                LevelEditorItem item = otherItemSO.items[trampoline.id];
                Instantiate(item.editorPrefab, new(trampoline.position.x + item.objectOffset.x, trampoline.position.y + item.objectOffset.y), Quaternion.identity);
            }
        }

        public ObjectTransformValue GetObjectTransformAdjustValue(LevelEditorItem selectedItem, int state_)
        {
            int State = state_;
            if (!selectedItem.extra.changeable) { State = 0; }


            ObjectTransformValue value = new();
            if (State == 0)
            {
                value.positionX = selectedItem.objectOffset.x;
                value.positionY = selectedItem.objectOffset.y;
                value.rotationZ = 0;
            }
            else if (State == 1)
            {
                value.positionX = selectedItem.extra.leftPos.x;
                value.positionY = selectedItem.extra.leftPos.y;
                value.rotationZ = -90;
            }
            else if (State == 2)
            {
                value.positionX = selectedItem.extra.upPos.x;
                value.positionY = selectedItem.extra.upPos.y;
                value.rotationZ = -180;
            }
            else if (State == 3)
            {
                value.positionX = selectedItem.extra.RightPos.x;
                value.positionY = selectedItem.extra.RightPos.y;
                value.rotationZ = 90;
            }

            if (selectedItem.validationType == ValidationType.Interactable && selectedItem.id == 17)
            {
                if (State == 0)
                {
                    value.rotationZ = -90;
                }
                else if (State == 1)
                {
                    value.rotationZ = 180;

                }
                else if (State == 2)
                {
                    value.rotationZ = 90;

                }
                else if (State == 3)
                {
                    value.rotationZ = 0;
                }
            }
            return value;
        }

        public void LinkTriggers()
        {
            foreach (var trigger in triggerDictionary)
            {
                ObjectProperty triggerProperty = trigger.Key;
                triggerProperty.valueStrings.TryGetValue("ConnectedDoor", out var connectedDoorKey);
                if (connectedDoorKey != "NONE")
                {
                    if (doorDictionary.TryGetValue(connectedDoorKey,out var doorObject))
                    {
                        if (trigger.Value.TryGetComponent(out PressurePlate pressurePlate))
                        {
                            pressurePlate.obejctsToTurnOn.Add(doorObject);
                        }
                        if (trigger.Value.TryGetComponent(out LaserDetector paserDetector))
                        {
                            paserDetector.obejctsToTurnOn.Add(doorObject);
                        }
                        if (trigger.Value.TryGetComponent(out Lever lever))
                        {
                            //lever.whatToTurnOn.Add(doorObject);
                        }
                    }
                }
            }
        }
    }
}