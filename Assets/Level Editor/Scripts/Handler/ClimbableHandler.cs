using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder
{
    public class ClimbableHandler : MonoBehaviour,ISaveLoad
    {
        public Tilemap climbableTilemap;
        public Dictionary<string, ClimbableProperty> climbableDictionary = new();
        Vector3Int startPos;
        Vector3Int maxValidPos;
        Vector3Int minValidPos;
        Vector3Int currentValidPos;

        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        private string uniqueId = "Climbable";
        public void OnEnable()
        {
            SaveLoadManager.Singleton.saveLoadList.Add(this);
        }
        public bool ValidationCheckForClimbable(LevelEditorItem selectedItem, Vector3Int gridPos)
        {
            if (GridCursor.Singleton.locked)
            {
                currentValidPos = GetValidPos(gridPos);
                GridCursor.Singleton.gridPos = currentValidPos;
                if (Input.GetMouseButtonDown(0))
                {
                    CreateClimbableProperty(selectedItem);
                    currentValidPos = gridPos;
                    GridCursor.Singleton.gridPos = gridPos;
                    GridCursor.Singleton.LockCursor(false);
                }
                return true;
            }
            else
            {
                string tileKey = LevelCreateManager.GetTileKey(gridPos);
                if (!LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) &&
                    !LevelCreateManager.Singleton.PlatformHandler.childDictionary.ContainsKey(tileKey)&&
                    !LevelCreateManager.Singleton.DecorationHandler.decorationChildDictionary.ContainsKey(tileKey))
                {
                    if (climbableDictionary.ContainsKey(tileKey))
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            climbableTilemap.SetTile(gridPos, null);
                            climbableDictionary.Remove(tileKey);
                        }
                        return false;
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!GridCursor.Singleton.locked)
                        {
                            SetValidPos(gridPos);
                            GridCursor.Singleton.LockCursor(true);
                        }
                    }
                    return true;
                }
                return false;
            }
            
        }

        private void CreateClimbableProperty(LevelEditorItem selectedItem)
        {
            int startMinPosY = (int)MathF.Min(startPos.y, currentValidPos.y);
            int length = (int)MathF.Abs(startPos.y - currentValidPos.y);
            for (int i = startMinPosY; i < startMinPosY + length + 1; i++)
            {
                ClimbableProperty climbableProperty = new()
                {
                    itemId = selectedItem.id,
                    gridPos = new(startPos.x, i)
                };
                PlaceChilbable(climbableProperty);
            }
        }
        public void PlaceChilbable(ClimbableProperty climbableProperty)
        {
            string tileKey = LevelCreateManager.GetTileKey(climbableProperty.gridPos.x, climbableProperty.gridPos.y);
            climbableDictionary.Add(tileKey, climbableProperty);
            climbableTilemap.SetTile(new(climbableProperty.gridPos.x, climbableProperty.gridPos.y), ItemManager.Singleton.GetItemDetails(climbableProperty.itemId,ItemCategory.Other).tile);
        }

        private void SetValidPos(Vector3Int gridPos)
        {
            startPos = gridPos;
            minValidPos = new(gridPos.x, gridPos.y - 4);
            maxValidPos = new(gridPos.x, gridPos.y + 4);

            for (int i = 1; i < 4; i++)
            {
                Vector2Int checkPos = new(gridPos.x, gridPos.y - i);
                string tileKey = LevelCreateManager.GetTileKey(checkPos.x, checkPos.y);
                if (LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) ||
                    LevelCreateManager.Singleton.PlatformHandler.childDictionary.ContainsKey(tileKey) ||
                    climbableDictionary.ContainsKey(tileKey))
                {
                    minValidPos = new(checkPos.x, checkPos.y + 1);
                    break;
                }
            }

            for (int i = 1; i < 4; i++)
            {
                Vector2Int checkPos = new(gridPos.x, gridPos.y + i);
                string tileKey = LevelCreateManager.GetTileKey(checkPos.x, checkPos.y);
                if (LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) ||
                    LevelCreateManager.Singleton.PlatformHandler.childDictionary.ContainsKey(tileKey) ||
                    climbableDictionary.ContainsKey(tileKey))
                {
                    maxValidPos = new(checkPos.x, checkPos.y - 1);
                    break;
                }
            }
            currentValidPos = gridPos;
        }
        private Vector3Int GetValidPos(Vector3Int gridPos)
        {
            if (gridPos.y > maxValidPos.y) return maxValidPos;
            if (gridPos.y < minValidPos.y) return minValidPos;
            return new(maxValidPos.x, gridPos.y);
        }
        public LevelSave Save()
        {
            return new() { ClimbableDictionary = climbableDictionary };
        }

        public void Load(LevelSave mapSave)
        {
            climbableDictionary = new();
            climbableTilemap.ClearAllTiles();
            foreach (var item in mapSave.ClimbableDictionary.Values)
            {
                PlaceChilbable(item);
            }
        }
    }
    [System.Serializable]
    public struct ClimbableProperty
    {
        public int itemId;
        public Vector2IntSerializable gridPos;
    }
}