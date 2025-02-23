using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder
{
    public class PlayableLevelBuilder : MonoBehaviour
    {
        public GameObject playerObject;
        public List<int> triggerIds;
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
        public CameraFollow cameraFollow;
        private void Start()
        {
            if (PlayableMaker.Singleton != null && PlayableMaker.Singleton.playableToBuild != null)
            {
                BuildLevel(PlayableMaker.Singleton.playableToBuild);
            }
        }
        void BuildLevel(PlayableLevelData playableLevelData)
        {
            if (playableLevelData != null)
            {
                Dictionary<string, TileProperty> floorDictionary =
                BuildFloor(playableLevelData);
                BuildPlatform(playableLevelData, floorDictionary);
                BuildClimbable(playableLevelData);
                BuildDecoration(playableLevelData);
                BuildOther(playableLevelData);
                BuildItem(playableLevelData);
                BuildEnemy(playableLevelData);
                BuildInteractable(playableLevelData);

                LevelEditorItem spawnItem = interactableItemSO.items[playableLevelData.Spawn.id];
                Vector2 spawnPosition = new(playableLevelData.Spawn.pos.x + spawnItem.states[0].position.x,
                    playableLevelData.Spawn.pos.y + spawnItem.states[0].position.y);
                GameObject Spawn = Instantiate(spawnItem.gamePrefab, spawnPosition, Quaternion.identity);

                LevelEditorItem exitItem = interactableItemSO.items[playableLevelData.Exit.id];
                Vector2 exitPosition = new(playableLevelData.Exit.pos.x + exitItem.states[0].position.x,
                    playableLevelData.Exit.pos.y + exitItem.states[0].position.y);
                GameObject Exit = Instantiate(exitItem.gamePrefab, exitPosition, Quaternion.identity);

                
                playerObject.transform.position = spawnPosition;
                playerObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }



        private Dictionary<string, TileProperty> BuildFloor(PlayableLevelData playableLevelData)
        {
            Dictionary<string, TileProperty> floorDictionary = new();
            foreach (var floor in playableLevelData.FloorData)
            {
                floorDictionary.Add(TileHandler.GetTileKey(floor.position.x, floor.position.y), floor);
                TileHandler.ConnectFloorTile(floorSO, floorTileMap, floorDictionary, floor.id, new(floor.position.x, floor.position.y));
            }
            return floorDictionary;
        }

        private void BuildPlatform(PlayableLevelData playableLevelData, Dictionary<string, TileProperty> floorDictionary)
        {
            foreach (var platform in playableLevelData.PlatformData)
            {
                TileHandler.ConnectPlatformTile(platform.id, new(platform.position.x, platform.position.y, 0), floorDictionary, platformTilemap, platformSO);
            }
        }

        private void BuildDecoration(PlayableLevelData playableLevelData)
        {
            foreach (var parentDetails in playableLevelData.DecorationData)
            {
                LevelEditorItem item = decorationItemSO.items[parentDetails.itemId];
                ItemStateDetails defaultState = item.states[0];

                Vector2Int startPos = new(parentDetails.pos.x, parentDetails.pos.y - (Mathf.FloorToInt(defaultState.dimension.y / 2)));
                Tile[] decoTiles = decorationSO.details[parentDetails.itemId].tiles;
                for (int y = startPos.y, tileIndex = 0; y < startPos.y + defaultState.dimension.y; y++, tileIndex++)
                {
                    if (decoTiles == null || decoTiles.Length <= 0) { continue; }
                    decorationTileMap.SetTile(new(startPos.x, y), decoTiles[tileIndex]);
                }


                if (item.gamePrefab)
                {
                    Instantiate(item.gamePrefab,
                        new(parentDetails.pos.x + defaultState.position.x,
                        parentDetails.pos.y + defaultState.position.y),
                        Quaternion.Euler(0, 0, defaultState.rotation));
                }
            }
        }

        private void BuildClimbable(PlayableLevelData playableLevelData)
        {
            foreach (var climbable in playableLevelData.ClimbableData)
            {
                climbableTileMap.SetTile(new Vector3Int(climbable.position.x, climbable.position.y), otherItemSO.items[climbable.id].states[0].tile);
            }
        }

        public void BuildOther(PlayableLevelData playableLevelData)
        {
            foreach (var other in playableLevelData.OtherData)
            {
                LevelEditorItem item = otherItemSO.items[other.id];
                ItemStateDetails stateDetails = item.states[other.state];
                if (item.id == 2)//spike id
                {
                    spikeTileMap.SetTile(new(other.pos.x, other.pos.y, 0), stateDetails.tile);
                    Quaternion rotation = Quaternion.Euler(0, 0, stateDetails.rotation);
                    Matrix4x4 matrix = Matrix4x4.Rotate(rotation);
                    spikeTileMap.SetTransformMatrix(new(other.pos.x, other.pos.y, 0), matrix);
                }

                if (item.id == 3)//trampoline
                {
                    Instantiate(item.editorPrefab,
                        new(other.pos.x + stateDetails.position.x, other.pos.y + stateDetails.position.y),
                        Quaternion.identity);
                }
            }
        }
        private void BuildItem(PlayableLevelData playableLevelData)
        {
            foreach (var item in playableLevelData.ItemData)
            {
                LevelEditorItem itemDetails = itemSO.items[item.id];
                Instantiate(itemDetails.gamePrefab, new(item.position.x + itemDetails.states[0].position.x, item.position.y + itemDetails.states[0].position.y), Quaternion.identity);
            }
        }
        public void BuildEnemy(PlayableLevelData playableLevelData)
        {
            foreach (var enemy in playableLevelData.EnemyData)
            {
                LevelEditorItem item = enemyItemSO.items[enemy.id];
                ItemStateDetails stateDetails = item.states[enemy.state];

                Instantiate(item.gamePrefab,
                        new(enemy.pos.x + stateDetails.position.x, 
                        enemy.pos.y + stateDetails.position.y),
                        Quaternion.Euler(0f,stateDetails.rotation, 0f));
            }
        }
        public void BuildInteractable(PlayableLevelData playableLevelData)
        {
            Dictionary<string, GameObject> interactableObjects = new();
            List<ObjectProperty> triggers = new();
            foreach (var interactable in playableLevelData.InteractableData)
            {
                LevelEditorItem item = interactableItemSO.items[interactable.id];
                Debug.Log(interactable.id + " " + interactable.state);
                ItemStateDetails stateDetails = item.states[interactable.state];
                
                if(interactable.id ==0 ||  interactable.id == 1) { continue; }//ignore spawn and exit since already been Instantiated
                GameObject interactableObject = Instantiate(item.gamePrefab,
                        new(interactable.pos.x + stateDetails.position.x,
                        interactable.pos.y + stateDetails.position.y),
                        Quaternion.Euler(0f, 0f, stateDetails.rotation));
                if (triggerIds.Contains(interactable.id))
                {
                    triggers.Add(interactable);
                }
                interactableObjects.Add(TileHandler.GetTileKey(interactable.pos.x, interactable.pos.y),interactableObject);
            }

            foreach (var trigger in triggers)
            {
                if (trigger.stringValues.TryGetValue(Settings.LinkedObjectKey, out var linkedInteractableKey))
                {
                    if (interactableObjects.TryGetValue(linkedInteractableKey, out var interactableObject))
                    {
                        if (interactableObjects.TryGetValue(TileHandler.GetTileKey(trigger.pos.x, trigger.pos.y), out var triggerObject))
                        {
                            GameObject LinkedObject = new();
                            LinkedObject.name = "===LinkedObject===";
                            triggerObject.transform.SetParent(LinkedObject.transform, true);
                            interactableObject.transform.SetParent(LinkedObject.transform, true);
                            
                            
                            if (triggerObject.TryGetComponent<PressurePlate>(out PressurePlate pressurePlate))
                            {
                                pressurePlate.obejctsToTurnOn.Add(interactableObject);
                            }
                            else if (triggerObject.TryGetComponent<LaserDetector>(out LaserDetector laserDetector))
                            {
                                laserDetector.obejctsToTurnOn.Add(interactableObject);
                            }
                        }
                    }
                }
            }
        }
    }

}