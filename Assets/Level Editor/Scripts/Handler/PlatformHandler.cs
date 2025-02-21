using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace LevelBuilder
{
    public class PlatformHandler : MonoBehaviour,ISaveLoad
    {
        public Tilemap platformTileMap;
        [Header("Debug")]
        Vector2Int startPos;
        Vector2Int validPos;
        Vector2Int endPos;
        Vector2Int supportPos;
        bool selectingTile;
        [field:SerializeField]
        public PlatformSO platformDetails { get; private set; }
        private string uniqueId = SaveLoadManager.SavePlatform;
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        public void OnEnable()
        {
            EventHandler.OnFloorUpdate += OnFloorUpdate;
            EventHandler.OnFloorDestroy += OnFloorDestroy;
        }
        public Dictionary<string, PlatformChildProperty> tileDictionary = new();
        public Dictionary<string, PlatformParentProperty> platformDictionary = new();

        

        public void OnFloorDestroy(int x,int y)
        {
            if (platformDictionary.TryGetValue(LevelCreateManager.GetTileKey(x + 1, y), out PlatformParentProperty rightPlatformParent))
            {
                DestroyPlatform(rightPlatformParent);
            }
            if (platformDictionary.TryGetValue(LevelCreateManager.GetTileKey(x - 1, y), out PlatformParentProperty leftPlatformParent))
            {
                DestroyPlatform(leftPlatformParent);
            }
        }
        public bool ValidationCheckForPlatform(LevelEditorItem selectedItem, Vector2Int gridPos)
        {
            if (!selectingTile)
            {
                GridCursor.Singleton.SetCursor(gridPos, Vector2Int.one, CursorType.Dynamic);
                if (tileDictionary.TryGetValue(LevelCreateManager.GetTileKey(gridPos), out PlatformChildProperty platformChildDetails))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (platformDictionary.TryGetValue(platformChildDetails.parentId, out PlatformParentProperty parentDetails))
                        {
                            DestroyPlatform(parentDetails);
                        }
                    }
                    return false;
                }
                
                if ((LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x + 1, gridPos.y)) ||
                     LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x - 1, gridPos.y))) &&
                    !LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x, gridPos.y))&&
                    !LevelCreateManager.Singleton.ClimbableHandler.climbableDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x, gridPos.y))&&
                    !LevelCreateManager.Singleton.DecorationHandler.occupiedTiles.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x, gridPos.y)))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        startPos = gridPos;
                        endPos = GetValidMaxCursorScalePosition(selectedItem, gridPos);
                        selectingTile = true;
                        GridCursor.Singleton.SetScaling(selectingTile);
                    }
                    return true;
                }
                return false;
            }
            else
            {
                validPos = GetCurrentValidEndPos(gridPos);
                GridCursor.Singleton.SetCursor(validPos, Vector2Int.one, CursorType.Dynamic);
                if (Input.GetMouseButtonDown(0))
                {
                    PlatformParentProperty parent = CreatePlatform(selectedItem);
                    PlacePlatform(parent);
                    selectingTile = false;
                    GridCursor.Singleton.SetScaling(selectingTile);
                }
                return true;
            }
        }

        public PlatformParentProperty CreatePlatform(LevelEditorItem selectedItem)
        {
            int length = (int)MathF.Abs(startPos.x - validPos.x) + 1;
            int startSetPosX = (int)MathF.Min(startPos.x, validPos.x);
            string parentKey = LevelCreateManager.GetTileKey(startPos.x, startPos.y);
            
            PlatformParentProperty newParent = new()
            {
                itemId = selectedItem.id,
                parentKey = parentKey,
                childs = new()
            };

            for (int i = startSetPosX, loopCount = 0; i < startSetPosX + length; i++, loopCount++)
            {
                Vector3Int currentTilePos = new(i, startPos.y);
                PlatformChildProperty newChield = new()
                {
                    gridPos = new(currentTilePos.x,currentTilePos.y),
                    parentId = parentKey
                };
                newParent.childs.Add(newChield);
            }
            return newParent;
        }
        
        public void DestroyPlatform(PlatformParentProperty parentDetails)
        {
            foreach (var child in parentDetails.childs)
            {
                string childKey = LevelCreateManager.GetTileKey(child.gridPos.x, child.gridPos.y);
                tileDictionary.Remove(childKey);
                platformTileMap.SetTile(new(child.gridPos.x, child.gridPos.y), null);
            }
            platformDictionary.Remove(parentDetails.parentKey);
        }
        public void PlacePlatform(PlatformParentProperty platformParentDetails)
        {
            foreach (var child in platformParentDetails.childs)
            {
                string childKey = LevelCreateManager.GetTileKey(child.gridPos.x, child.gridPos.y);
                tileDictionary.Add(childKey, child);
                int platformId = platformParentDetails.itemId;
                Vector3Int childPos = new(child.gridPos.x, child.gridPos.y);
                var floorDictionary = LevelCreateManager.Singleton.FloorHandler.floorDictionary;
                TileHandler.ConnectPlatformTile(platformId, childPos, floorDictionary, platformTileMap, platformDetails);
            }
            platformDictionary.Add(platformParentDetails.parentKey, platformParentDetails);
        }
        
        public void OnFloorUpdate(int x, int y)
        {
            foreach (var platform in platformDictionary.Values)
            {
                foreach (var child in platform.childs)
                {
                    int platformId = platform.itemId;
                    Vector3Int childPos = new(child.gridPos.x, child.gridPos.y);
                    var floorDictionary = LevelCreateManager.Singleton.FloorHandler.floorDictionary;
                    TileHandler.ConnectPlatformTile(platformId, childPos, floorDictionary, platformTileMap, platformDetails);
                }
            }
            if (tileDictionary.TryGetValue(LevelCreateManager.GetTileKey(x + 1, y), out PlatformChildProperty rightChildDetails))
            {
                platformDictionary.TryGetValue(rightChildDetails.parentId, out PlatformParentProperty rightPlatformChildParent);
                TileHandler.ConnectPlatformTile(rightPlatformChildParent.itemId, new(x + 1, y), LevelCreateManager.Singleton.FloorHandler.floorDictionary, platformTileMap, platformDetails);
            }
            if (tileDictionary.TryGetValue(LevelCreateManager.GetTileKey(x - 1, y), out PlatformChildProperty leftChildDetails))
            {
                platformDictionary.TryGetValue(leftChildDetails.parentId, out PlatformParentProperty leftPlatformChildParent);
                TileHandler.ConnectPlatformTile(leftPlatformChildParent.itemId, new(x - 1, y), LevelCreateManager.Singleton.FloorHandler.floorDictionary, platformTileMap, platformDetails);
            }
        }
        private Vector2Int GetValidMaxCursorScalePosition(LevelEditorItem selectedItem, Vector2Int gridPos)
        {
            supportPos = LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x - 1, gridPos.y)) ? new(1, 0) : new(-1, 0);

            for (int i = 1; i < platformDetails.properties[selectedItem.id - 11].maxPlacementDimention.x; i++)
            {
                Vector2Int checkPos = new(gridPos.x + (i * supportPos.x), gridPos.y);
                if (LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(checkPos.x, checkPos.y)) ||
                    tileDictionary.ContainsKey(LevelCreateManager.GetTileKey(checkPos.x, checkPos.y)))
                {
                    return new(checkPos.x - supportPos.x, checkPos.y);
                }
            }
            return new(gridPos.x + supportPos.x * (platformDetails.properties[selectedItem.id - 11].maxPlacementDimention.x - 1), gridPos.y);
        }
        private Vector2Int GetCurrentValidEndPos(Vector2Int gridPos)
        {
            if (supportPos.x > 0)
            {
                if (gridPos.x > endPos.x) return endPos;
                if (gridPos.x < startPos.x) return startPos;
            }
            else
            {
                if (gridPos.x < endPos.x) return endPos;
                if (gridPos.x > startPos.x) return startPos;
            }
            return new Vector2Int(gridPos.x, startPos.y);
        }
        public LevelData Save()
        {
            return new() { PlatformDictionary = platformDictionary };
        }
        public void Load(LevelData mapSave)
        {
            tileDictionary = new();
            platformDictionary = new();
            foreach (var platform in mapSave.PlatformDictionary.Values)
            {
                PlacePlatform(platform);
            }
        }
    }


    [System.Serializable]
    public struct PlatformDetails
    {
        public int id;
        public Tile[] tiles;
        public Vector2Int maxPlacementDimention;
    }
    [System.Serializable]
    public struct PlatformChildProperty
    {
        public string parentId;
        public Vector2IntSerializable gridPos;
    }
}

