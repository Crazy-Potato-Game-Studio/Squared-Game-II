using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder
{
    public class ClimbableHandler : MonoBehaviour,ISaveLoad
    {
        public Tilemap climbableTilemap;
        public Dictionary<string, TileProperty> climbableDictionary = new();
        Vector2Int startPos;
        Vector2Int maxValidPos;
        Vector2Int minValidPos;
        Vector2Int currentValidPos;
        bool selectingTile;
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        private string uniqueId = SaveLoadManager.SaveClimable;
        public bool ValidationCheckForClimbable(LevelEditorItem selectedItem, Vector2Int gridPos)
        {
            if (selectingTile)
            {
                currentValidPos = GetValidPos(gridPos);
                GridCursor.Singleton.SetCursor(currentValidPos, Vector2Int.one, CursorType.Dynamic);
                if (Input.GetMouseButtonDown(0))
                {
                    GridCursor.Singleton.SetScaling(false);
                    selectingTile = false;
                    CreateClimbableProperty(selectedItem);
                    currentValidPos = gridPos;
                }
                return true;
            }
            else
            {
                GridCursor.Singleton.SetCursor(gridPos, Vector2Int.one, CursorType.Dynamic);
                string tileKey = LevelCreateManager.GetTileKey(gridPos);
                if (!LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) &&
                    !LevelCreateManager.Singleton.PlatformHandler.tileDictionary.ContainsKey(tileKey)&&
                    !LevelCreateManager.Singleton.DecorationHandler.occupiedTiles.ContainsKey(tileKey))
                {
                    if (climbableDictionary.ContainsKey(tileKey))
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            climbableTilemap.SetTile(new(gridPos.x,gridPos.y), null);
                            climbableDictionary.Remove(tileKey);
                        }
                        return false;
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        GridCursor.Singleton.SetScaling(true);
                        selectingTile = true;
                        SetValidPos(gridPos);
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
                TileProperty climbableProperty = new(selectedItem.id, new(startPos.x, i));
                PlaceChilbable(climbableProperty);
            }
        }
        public void PlaceChilbable(TileProperty climbableProperty)
        {
            string tileKey = LevelCreateManager.GetTileKey(climbableProperty.position.x, climbableProperty.position.y);
            climbableDictionary.Add(tileKey, climbableProperty);
            climbableTilemap.SetTile(new(climbableProperty.position.x, climbableProperty.position.y),
                ItemManager.Singleton.GetItemDetails(climbableProperty.id, ItemCategory.Other).states[0].tile);
        }

        private void SetValidPos(Vector2Int gridPos)
        {
            startPos = gridPos;
            minValidPos = new(gridPos.x, gridPos.y - 4);
            maxValidPos = new(gridPos.x, gridPos.y + 4);

            for (int i = 1; i < 4; i++)
            {
                Vector2Int checkPos = new(gridPos.x, gridPos.y - i);
                string tileKey = LevelCreateManager.GetTileKey(checkPos.x, checkPos.y);
                if (LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) ||
                    LevelCreateManager.Singleton.PlatformHandler.tileDictionary.ContainsKey(tileKey) ||
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
                    LevelCreateManager.Singleton.PlatformHandler.tileDictionary.ContainsKey(tileKey) ||
                    climbableDictionary.ContainsKey(tileKey))
                {
                    maxValidPos = new(checkPos.x, checkPos.y - 1);
                    break;
                }
            }
            currentValidPos = gridPos;
        }
        private Vector2Int GetValidPos(Vector2Int gridPos)
        {
            if (gridPos.y > maxValidPos.y) return maxValidPos;
            if (gridPos.y < minValidPos.y) return minValidPos;
            return new(maxValidPos.x, gridPos.y);
        }
        public LevelData Save()
        {
            return new() { ClimbableDictionary = climbableDictionary };
        }

        public void Load(LevelData mapSave)
        {
            climbableDictionary = new();
            climbableTilemap.ClearAllTiles();
            foreach (var item in mapSave.ClimbableDictionary.Values)
            {
                PlaceChilbable(item);
            }
        }
    }
}