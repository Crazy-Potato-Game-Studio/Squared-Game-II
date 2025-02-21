using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder
{
    public class OtherHandler : MonoBehaviour,ISaveLoad
    {
        public Tilemap spikeTileMap;
        int state;
        GameObject cursorObject;
        public Dictionary<string, ObjectProperty> otherDictionary = new();
        public Dictionary<string, GameObject> otherObjects = new();
        public Dictionary<string, string> occupiedTiles = new();
        private string uniqueId = SaveLoadManager.SaveOther;
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }

        private void OnEnable()
        {
            LevelCreateManager.ItemSelectEvent += OnItemSelect;
            LevelCreateManager.OverUiEvent += OverUi;
        }
        void OverUi()
        {
            if (cursorObject != null)
            {
                cursorObject.transform.position = new(cursorObject.transform.position.x, cursorObject.transform.position.y, -20);
            }
        }
        private void OnItemSelect(LevelEditorItem item)
        {
            if(cursorObject != null) { Destroy(cursorObject); }
            if(item == null || item.validationType != ValidationType.Other) { return; }
            state = 0;
            if (item.editorPrefab != null)
            {
                cursorObject = Instantiate(item.editorPrefab, LevelCreateManager.Singleton.cursorObjectParent);
                cursorObject.transform.position = new(cursorObject.transform.position.x, cursorObject.transform.position.y, -20);
                cursorObject.transform.rotation = Quaternion.Euler(0, 0, item.states[state].rotation);
            }
        }

        public bool ValidationCheckForOthers(LevelEditorItem selectedItem, Vector2Int gridPos)
        {
            ItemStateDetails stateDetails = selectedItem.states[state];
            GridCursor.Singleton.SetCursor(gridPos, stateDetails.dimension,stateDetails.cursorType);
            if (Input.GetKeyDown(KeyCode.R) && selectedItem.states.Length >1)
            {
                state++;
                if (state == selectedItem.states.Length) state = 0;
            }
            if (cursorObject != null)
            {
                cursorObject.transform.position = new(gridPos.x + stateDetails.position.x, gridPos.y + stateDetails.position.y,0);
                cursorObject.transform.rotation = Quaternion.Euler(0, 0, stateDetails.rotation);
            }

            string tileKey = TileHandler.GetTileKey(gridPos.x, gridPos.y);
            if (otherDictionary.ContainsKey(tileKey))
            {
                if(Input.GetMouseButtonDown(0))
                {
                    DestroyOther(stateDetails, gridPos);
                }
                return false;
            }
            if (TileEmpty(stateDetails,gridPos) && HasSupport(stateDetails, gridPos))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ObjectProperty otherProperty = new(selectedItem.id, state, new(gridPos.x, gridPos.y));
                    PlaceOther(otherProperty);
                }
                return true;
            }
            return false;
        }
        private void PlaceOther(ObjectProperty otherProperty)
        {
            LevelEditorItem item = ItemManager.Singleton.GetItemDetails(otherProperty.id, ItemCategory.Other);
            ItemStateDetails stateDetails = item.states[otherProperty.state];
            string otherKey = TileHandler.GetTileKey(otherProperty.pos.x,otherProperty.pos.y);
            otherDictionary.Add(otherKey, otherProperty);
            for (int x = 0; x < stateDetails.dimension.x; x++)
            {
                string tilekey = TileHandler.GetTileKey(x + otherProperty.pos.x, otherProperty.pos.y);
                occupiedTiles.Add(tilekey,otherKey);
            }

            if (item.id == 2)//spike id
            {
                spikeTileMap.SetTile(new(otherProperty.pos.x, otherProperty.pos.y, 0), stateDetails.tile);
                Quaternion rotation = Quaternion.Euler(0, 0, stateDetails.rotation);
                Matrix4x4 matrix = Matrix4x4.Rotate(rotation);
                spikeTileMap.SetTransformMatrix(new(otherProperty.pos.x, otherProperty.pos.y, 0), matrix);
            }

            if(item.id == 3)//trampoline
            {
                GameObject trampoline = Instantiate(item.editorPrefab,
                    new(otherProperty.pos.x + stateDetails.position.x, otherProperty.pos.y + stateDetails.position.y),
                    Quaternion.identity);
                otherObjects.Add(otherKey, trampoline);
            }
            
        }
        private void DestroyOther(ItemStateDetails stateDetails,Vector2Int gridPos)
        {
            string otherKey = TileHandler.GetTileKey(gridPos);
            for (int x = 0; x < stateDetails.dimension.x; x++)
            {
                string tilekey = TileHandler.GetTileKey(x + gridPos.x, gridPos.y);
                occupiedTiles.Remove(tilekey);
            }
            if (otherObjects.TryGetValue(otherKey, out var otherObject))
            {
                Destroy(otherObject);
                otherObjects.Remove(otherKey);
            }
            otherDictionary.Remove(otherKey);
            spikeTileMap.SetTile(new(gridPos.x, gridPos.y), null);
        }
        private bool TileEmpty(ItemStateDetails stateDetails, Vector2Int gridPos)
        {
            for (int x = 0; x < stateDetails.dimension.x; x++)
            {
                string tileKey = TileHandler.GetTileKey(x + gridPos.x, gridPos.y);
                if (occupiedTiles.ContainsKey(tileKey) ||
                LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) ||
                LevelCreateManager.Singleton.PlatformHandler.tileDictionary.ContainsKey(tileKey) ||
                LevelCreateManager.Singleton.InteractableHandler.occupiedTileDictionary.ContainsKey(tileKey) ||
                LevelCreateManager.Singleton.EnemyHandler.enemyTiles.Contains(tileKey) ||
                LevelCreateManager.Singleton.ItemHandler.itemDictionary.ContainsKey(tileKey))
                {
                    return false;
                }
            }
            return true;
        }
        private bool HasSupport(ItemStateDetails stateDetails,Vector2Int gridPos)
        {
            List<Vector2Int> supportTiles = new();
            if (stateDetails.supportType.HasFlag(SupportType.Down))
            {
                for (int x = 0; x < stateDetails.dimension.x; x++)
                {
                    supportTiles.Add(new(gridPos.x + x, gridPos.y - 1));
                }
            }
            if (stateDetails.supportType.HasFlag(SupportType.Up))
            {
                int yPos = gridPos.y + stateDetails.dimension.y;
                for (int x = 0; x < stateDetails.dimension.x; x++)
                {
                    supportTiles.Add(new(gridPos.x + x, yPos));
                }
            }
            if (stateDetails.supportType.HasFlag(SupportType.Left))
            {
                for (int y = 0; y < stateDetails.dimension.y; y++)
                {
                    supportTiles.Add(new(gridPos.x - 1, gridPos.y + y));
                }
            }
            if (stateDetails.supportType.HasFlag(SupportType.Right))
            {
                int xPos = gridPos.x + stateDetails.dimension.x;
                for (int y = 0; y < stateDetails.dimension.y; y++)
                {
                    supportTiles.Add(new(xPos, gridPos.y + y));
                }
            }

            foreach (var supportTile in supportTiles)
            {
                string tileKey = TileHandler.GetTileKey(supportTile);
                if (!LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey))
                {
                    return false;
                }
            }
            return true;
        }
        
        public LevelData Save()
        {
            LevelData levelSave = new();
            levelSave.OtherDictionary = otherDictionary;
            return levelSave;
        }

        public void Load(LevelData mapSave)
        {
            if(mapSave ==null || mapSave.OtherDictionary == null) { return; }
            foreach (var other in mapSave.OtherDictionary.Values)
            {
                PlaceOther(other);
            }
        }
    }
}