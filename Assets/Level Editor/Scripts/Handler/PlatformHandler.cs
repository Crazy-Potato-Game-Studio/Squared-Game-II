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
        Vector3Int startPos;
        Vector3Int validPos;
        Vector3Int endPos;
        Vector3Int supportPos;
        [field:SerializeField]
        public PlatformSO platformDetails { get; private set; }
        private string uniqueId = "Platform";
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        public void OnEnable()
        {
            SaveLoadManager.Singleton.saveLoadList.Add(this);
            EventHandler.OnFloorUpdate += OnFloorUpdate;
            EventHandler.OnFloorDestroy += OnFloorDestroy;
        }
        public Dictionary<string, PlatformChildProperty> childDictionary = new();
        public Dictionary<string, PlatformParentProperty> parentDictionary = new();

        

        public void OnFloorDestroy(int x,int y)
        {
            if (parentDictionary.TryGetValue(LevelCreateManager.GetTileKey(x + 1, y), out PlatformParentProperty rightPlatformParent))
            {
                DestroyPlatform(rightPlatformParent);
            }
            if (parentDictionary.TryGetValue(LevelCreateManager.GetTileKey(x - 1, y), out PlatformParentProperty leftPlatformParent))
            {
                DestroyPlatform(leftPlatformParent);
            }
        }
        public bool ValidationCheckForPlatform(LevelEditorItem selectedItem, Vector3Int gridPos)
        {
            if (!GridCursor.Singleton.locked)
            {
                if (childDictionary.TryGetValue(LevelCreateManager.GetTileKey(gridPos), out PlatformChildProperty platformChildDetails))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (parentDictionary.TryGetValue(platformChildDetails.parentId, out PlatformParentProperty parentDetails))
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
                    !LevelCreateManager.Singleton.DecorationHandler.decorationChildDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x, gridPos.y)))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        SetMultiTileSelection(selectedItem, gridPos);
                    }
                    return true;
                }
                return false;
            }
            else
            {
                validPos = GetCurrentValidEndPos(gridPos);
                GridCursor.Singleton.gridPos = validPos;
                if (Input.GetMouseButtonDown(0))
                {
                    PlatformParentProperty parent = CreatePlatform(selectedItem);
                    PlacePlatform(parent);
                    GridCursor.Singleton.LockCursor(false);
                }
                return true;
            }
        }
        public void SetMultiTileSelection(LevelEditorItem selectedItem,Vector3Int gridPos)
        {
            startPos = gridPos;
            endPos = GetValidMaxCursorScalePosition(selectedItem, gridPos);
            GridCursor.Singleton.gridPos = gridPos;
            GridCursor.Singleton.LockCursor(true);
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
                childDictionary.Remove(childKey);
                platformTileMap.SetTile(new(child.gridPos.x, child.gridPos.y), null);
            }
            parentDictionary.Remove(parentDetails.parentKey);
        }
        public void PlacePlatform(PlatformParentProperty platformParentDetails)
        {
            foreach (var child in platformParentDetails.childs)
            {
                TileHandler.ConnectPlatformTile(platformParentDetails.itemId, new(child.gridPos.x, child.gridPos.y), LevelCreateManager.Singleton.FloorHandler.floorDictionary,platformTileMap,platformDetails);
                string childKey = LevelCreateManager.GetTileKey(child.gridPos.x, child.gridPos.y);
                childDictionary.Add(childKey, child);
            }
            parentDictionary.Add(platformParentDetails.parentKey, platformParentDetails);
        }
        
        public void OnFloorUpdate(int x, int y)
        {
            if (childDictionary.TryGetValue(LevelCreateManager.GetTileKey(x + 1, y), out PlatformChildProperty rightChildDetails))
            {
                parentDictionary.TryGetValue(rightChildDetails.parentId, out PlatformParentProperty rightPlatformChildParent);
                TileHandler.ConnectPlatformTile(rightPlatformChildParent.itemId, new(x + 1, y), LevelCreateManager.Singleton.FloorHandler.floorDictionary, platformTileMap,platformDetails);
            }
            if (childDictionary.TryGetValue(LevelCreateManager.GetTileKey(x - 1, y), out PlatformChildProperty leftChildDetails))
            {
                parentDictionary.TryGetValue(leftChildDetails.parentId, out PlatformParentProperty leftPlatformChildParent);
                TileHandler.ConnectPlatformTile(leftPlatformChildParent.itemId, new(x - 1, y), LevelCreateManager.Singleton.FloorHandler.floorDictionary, platformTileMap, platformDetails);
            }
        }
        private Vector3Int GetValidMaxCursorScalePosition(LevelEditorItem selectedItem, Vector3Int gridPos)
        {
            supportPos = LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x - 1, gridPos.y)) ? new(1, 0) : new(-1, 0);

            for (int i = 1; i < platformDetails.properties[selectedItem.id - 11].maxPlacementDimention.x; i++)
            {
                Vector2Int checkPos = new(gridPos.x + (i * supportPos.x), gridPos.y);
                if (LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(checkPos.x, checkPos.y)) ||
                    childDictionary.ContainsKey(LevelCreateManager.GetTileKey(checkPos.x, checkPos.y)))
                {
                    return new(checkPos.x - supportPos.x, checkPos.y);
                }
            }
            return new(gridPos.x + supportPos.x * (platformDetails.properties[selectedItem.id - 11].maxPlacementDimention.x - 1), gridPos.y);
        }
        private Vector3Int GetCurrentValidEndPos(Vector3Int gridPos)
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
            return new Vector3Int(gridPos.x, startPos.y);
        }
        public LevelSave Save()
        {
            return new() { PlatformDictionary = parentDictionary };
        }
        public void Load(LevelSave mapSave)
        {
            childDictionary = new();
            parentDictionary = new();
            platformTileMap.ClearAllTiles();
            foreach (var iteparent in mapSave.PlatformDictionary.Values)
            {
                PlacePlatform(iteparent);
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

