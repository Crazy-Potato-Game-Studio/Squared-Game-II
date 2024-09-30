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
        public Dictionary<string, FloorProperty> floorDictionary = new();
        private string uniqueId = "Floor";
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        [Header("Mode Ui")]
        public bool placeMode = true;

        Vector3Int changedPos = new(-99999999, -9999 - 99999999);
        public void OnEnable()
        {
            SaveLoadManager.Singleton.saveLoadList.Add(this);
        }
        public bool ValidationCheckForFloor(LevelEditorItem selectedItem,Vector3Int gridPos)
        {
            string tileKey = LevelCreateManager.GetTileKey(gridPos);
            if (LevelCreateManager.Singleton.ClimbableHandler.climbableDictionary.ContainsKey(tileKey) ||
                LevelCreateManager.Singleton.PlatformHandler.childDictionary.ContainsKey(tileKey) ||
                LevelCreateManager.Singleton.DecorationHandler.decorationChildDictionary.ContainsKey(tileKey))
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
                changedPos = new(-99999999, -9999 - 99999999);
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
                    PlaceFloorTile(selectedItem, gridPos);
                    changedPos = gridPos;
                    return true;
                }
            }
            //if (floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos)))
            //{
            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        PlaceFloorTile(selectedItem,gridPos);
            //    }
            //    return false;
            //}
            //if (Input.GetMouseButtonDown(0))
            //{
            //    PlaceFloorTile(selectedItem, gridPos);
            //}
            return true;
        }

        private void PlaceFloorTile(LevelEditorItem selectedItem,Vector3Int gridPos)
        {
            string tileKey = LevelCreateManager.GetTileKey(gridPos);
            if (floorDictionary.ContainsKey(tileKey) && !placeMode)
            {
                floorDictionary.TryGetValue(tileKey,out FloorProperty floorProperty);
                floorDictionary.Remove(tileKey);
                if (floorProperty.id == selectedItem.id)
                {
                    EventHandler.CallFloorDestroy(gridPos.x, gridPos.y);
                }
                else
                {
                    FloorProperty property = new()
                    {
                        id = selectedItem.id,
                        gridPos = new(gridPos.x, gridPos.y)
                    };
                    floorDictionary.Add(tileKey, property);
                }
            }
            else if(!floorDictionary.ContainsKey(tileKey) && placeMode)
            {
                FloorProperty property = new()
                {
                    id = selectedItem.id,
                    gridPos = new(gridPos.x, gridPos.y)
                };
                floorDictionary.Add(tileKey, property);
            }
            TileHandler.ConnectFloorTile(floorTileDetailsSo, floorTileMap, floorDictionary, selectedItem.id, gridPos);
            EventHandler.CallFloorUpdate(gridPos.x, gridPos.y);
        }


        public LevelSave Save()
        {
            return new LevelSave() { FloorDictionary = floorDictionary };
        }

        public void Load(LevelSave mapSave)
        {
            floorDictionary = new();
            floorTileMap.ClearAllTiles();
            foreach (var item in mapSave.FloorDictionary)
            {
                FloorProperty floorProperty = item.Value;
                LevelEditorItem sandBoxItem = ItemManager.Singleton.GetItemDetails(floorProperty.id, ItemCategory.Floor);
                PlaceFloorTile(sandBoxItem,new(floorProperty.gridPos.x,floorProperty.gridPos.y));
            }
        }

        
        
    }
    [System.Serializable]
    public struct FloorProperty
    {
        public int id;
        public Vector2IntSerializable gridPos;
    }
}