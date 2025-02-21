using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace LevelBuilder
{
    public class FloorHandler : MonoBehaviour,ISaveLoad
    {
        public Tilemap floorTileMap;
        public FloorSO floorTileDetailsSo;
        public Dictionary<string, TileProperty> floorDictionary = new();
        private string uniqueId = SaveLoadManager.SaveFloor;
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        [Header("Mode Ui")]
        public bool placeMode = true;

        Vector2Int changedPos = new(-99999999, - 99999999);
        public bool ValidationCheckForFloor(LevelEditorItem selectedItem,Vector2Int gridPos)
        {
            GridCursor.Singleton.SetCursor(gridPos,Vector2Int.one);
            string tileKey = LevelCreateManager.GetTileKey(gridPos);
            if (LevelCreateManager.Singleton.ClimbableHandler.climbableDictionary.ContainsKey(tileKey) ||
                LevelCreateManager.Singleton.PlatformHandler.tileDictionary.ContainsKey(tileKey) ||
                LevelCreateManager.Singleton.DecorationHandler.occupiedTiles.ContainsKey(tileKey))
            {
                return false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos)))
                {
                    placeMode = false;
                }
                else
                {
                    placeMode = true;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                changedPos = new(-99999999, - 99999999);
            }
            
            if (Input.GetMouseButton(0))
            {
                if (gridPos != changedPos)
                {
                    if (!placeMode)
                    {
                        PlaceFloorTile(selectedItem, gridPos);
                        changedPos = gridPos;
                        return false;
                    }
                    else
                    {
                        PlaceFloorTile(selectedItem, gridPos);
                        changedPos = gridPos;
                        return true;
                    }
                }
            }
            return true;
        }

        private void PlaceFloorTile(LevelEditorItem selectedItem,Vector2Int gridPos)
        {
            string tileKey = LevelCreateManager.GetTileKey(gridPos);
            if (floorDictionary.ContainsKey(tileKey) && !placeMode)
            {
                floorDictionary.TryGetValue(tileKey,out TileProperty floorProperty);
                floorDictionary.Remove(tileKey);
                if (floorProperty.id == selectedItem.id)
                {
                    EventHandler.CallFloorDestroy(gridPos.x, gridPos.y);
                }
                else
                {
                    TileProperty property = new (selectedItem.id, new(gridPos.x, gridPos.y));
                    floorDictionary.Add(tileKey, property);
                }
            }
            else if(!floorDictionary.ContainsKey(tileKey) && placeMode)
            {
                TileProperty property = new(selectedItem.id, new(gridPos.x, gridPos.y));
                floorDictionary.Add(tileKey, property);
            }
            TileHandler.ConnectFloorTile(floorTileDetailsSo, floorTileMap, floorDictionary, selectedItem.id, gridPos);
            EventHandler.CallFloorUpdate(gridPos.x, gridPos.y);
        }


        public LevelData Save()
        {
            return new LevelData() { FloorDictionary = floorDictionary };
        }

        public void Load(LevelData mapSave)
        {
            floorDictionary = new();
            floorTileMap.ClearAllTiles();
            foreach (var item in mapSave.FloorDictionary)
            {
                TileProperty floorProperty = item.Value;
                LevelEditorItem sandBoxItem = ItemManager.Singleton.GetItemDetails(floorProperty.id, ItemCategory.Floor);
                PlaceFloorTile(sandBoxItem,new(floorProperty.position.x,floorProperty.position.y));
            }
        }
    }
}