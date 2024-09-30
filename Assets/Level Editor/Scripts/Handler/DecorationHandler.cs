using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace LevelBuilder
{
    public class DecorationHandler : MonoBehaviour,ISaveLoad
    {
        public Tilemap decorationTileMap;
        public DecorationSO decorationSO;
        public Dictionary<string,string> decorationChildDictionary = new();
        Dictionary<string,GameObject> decorationGameObjectDictionary = new();
        public Dictionary<string, DecorationProperty> decorationParentDictionary = new();
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        private string uniqueId = "Decoration";
        public void OnEnable()
        {
            SaveLoadManager.Singleton.saveLoadList.Add(this);
        }
        public bool ValidationCheckForDecoration(LevelEditorItem selectedItem, Vector3Int gridPos)
        {
            Vector3Int startPos = new(gridPos.x, gridPos.y - (Mathf.FloorToInt(selectedItem.gridDimension.y / 2)));
            GridCursor.Singleton.DisplayMultiGridCursor(startPos, selectedItem.gridDimension);
            for (int i = startPos.y; i < startPos.y+ selectedItem.gridDimension.y; i++)
            {
                string tileKey = LevelCreateManager.GetTileKey(gridPos.x,i);
                if(LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey)||
                   LevelCreateManager.Singleton.PlatformHandler.childDictionary.ContainsKey(tileKey)||
                   LevelCreateManager.Singleton.ClimbableHandler.climbableDictionary.ContainsKey(tileKey) || decorationChildDictionary.ContainsKey(tileKey))
                {
                    if (Input.GetMouseButtonDown(0) && decorationChildDictionary.TryGetValue(LevelCreateManager.GetTileKey(gridPos), out string parentKey))
                    {
                        decorationParentDictionary.TryGetValue(parentKey, out var decorationParentProperty);
                        DestroyDecoration(decorationParentProperty);
                    }
                    return false;
                }
                if (selectedItem.id == decorationSO.details[5].id && !LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x, gridPos.y - 2)))
                {
                    return false;
                }
                if (selectedItem.id == decorationSO.details[2].id && !LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x, gridPos.y +1)))
                {
                    return false;
                }
            }
            if(Input.GetMouseButtonDown(0))
            {
                CreateDecoration(selectedItem, startPos);
            }
            return true;
        }
        void CreateDecoration(LevelEditorItem selectedItem,Vector3Int startPos)
        {
            DecorationProperty newParentDetails = new()
            {
                itemId = selectedItem.id,
                startPos = new(startPos.x, startPos.y)
            };
            PlaceDecoration(newParentDetails);
        }
        void PlaceDecoration(DecorationProperty parentDetails)
        {
            LevelEditorItem item = ItemManager.Singleton.GetItemDetails(parentDetails.itemId, ItemCategory.Decoration);
            string newParentID = LevelCreateManager.GetTileKey(parentDetails.startPos.x, parentDetails.startPos.y);
            decorationParentDictionary.Add(newParentID, parentDetails);
            for (int i = parentDetails.startPos.y, tileIndex = 0; i < parentDetails.startPos.y + item.gridDimension.y; i++, tileIndex++)
            {
                string newChildID = LevelCreateManager.GetTileKey(parentDetails.startPos.x, i);
                decorationChildDictionary.Add(newChildID, newParentID);
            }
            Tile[] tiles = decorationSO.details[parentDetails.itemId].tiles;
            GameObject decoObjToSpawn = decorationSO.details[parentDetails.itemId].prefab;

            for (int i = parentDetails.startPos.y, tileIndex = 0; i < parentDetails.startPos.y + item.gridDimension.y; i++, tileIndex++)
            {
                if(tiles == null || tiles.Length <=0) { continue; }
                decorationTileMap.SetTile(new(parentDetails.startPos.x, i), tiles[tileIndex]);
            }
            if (decoObjToSpawn)
            {
                GameObject decoObj = Instantiate(item.editorPrefab, new(parentDetails.startPos.x + item.objectOffset.x, parentDetails.startPos.y + item.objectOffset.y), Quaternion.identity);
                decorationGameObjectDictionary.Add(LevelCreateManager.GetTileKey(parentDetails.startPos.x, parentDetails.startPos.y), decoObj);
            }
        }
        void DestroyDecoration(DecorationProperty decoProperty)
        {
            LevelEditorItem item = ItemManager.Singleton.GetItemDetails(decoProperty.itemId, ItemCategory.Decoration);
            string newParentID = LevelCreateManager.GetTileKey(decoProperty.startPos.x, decoProperty.startPos.y);
            decorationParentDictionary.Remove(newParentID);

            for (int i = decoProperty.startPos.y, tileIndex = 0; i < decoProperty.startPos.y + item.gridDimension.y; i++, tileIndex++)
            {
                string newChildID = LevelCreateManager.GetTileKey(decoProperty.startPos.x, i);
                decorationChildDictionary.Remove(newChildID);
                decorationTileMap.SetTile(new(decoProperty.startPos.x, i), null);
            }
            if (decorationSO.details[decoProperty.itemId].prefab)
            {
                decorationGameObjectDictionary.TryGetValue(newParentID, out var decoObj);
                Destroy(decoObj);
                decorationGameObjectDictionary.Remove(newParentID);
            }
        }

        public LevelSave Save()
        {
            return new() { DecorationDictionary = decorationParentDictionary };
        }

        public void Load(LevelSave mapSave)
        {
            decorationParentDictionary = new();
            decorationChildDictionary = new();
            decorationTileMap.ClearAllTiles();
            foreach (var item in mapSave.DecorationDictionary.Values)
            {
                PlaceDecoration(item);
            }
        }
    }

    [System.Serializable]
    public struct DecorationProperty
    {
        public int itemId;
        public Vector2IntSerializable startPos;
    }
}

