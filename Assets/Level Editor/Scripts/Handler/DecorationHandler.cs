using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace LevelBuilder
{
    public class DecorationHandler : MonoBehaviour,ISaveLoad
    {
        public Tilemap decorationTileMap;
        public DecorationSO decorationSO;
        public Dictionary<string,string> occupiedTiles = new();
        Dictionary<string,GameObject> decorationGameObjectDictionary = new();
        public Dictionary<string, DecorationProperty> decorationDictionary = new();
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        private string uniqueId = SaveLoadManager.SaveDecoration;
        GameObject cursorObject;
        private void OnEnable()
        {
            LevelCreateManager.OverUiEvent += CursorOverUI;
            LevelCreateManager.ItemSelectEvent += OnItemSelect;
        }
        void OnItemSelect(LevelEditorItem selectedItem)
        {
            if (cursorObject != null) { Destroy(cursorObject); }
            if (selectedItem == null) { return; }
            if (selectedItem.validationType != ValidationType.Decoration) { return; }
            if (selectedItem.editorPrefab == null) { return; }
            cursorObject = Instantiate(selectedItem.editorPrefab, 
                new Vector3(0, 0, -20), Quaternion.identity,
                LevelCreateManager.Singleton.cursorObjectParent);
            cursorObject.transform.name = "====="+cursorObject.transform.name;
        }
        private void CursorOverUI()
        {
            if (cursorObject != null)
            {
                cursorObject.transform.position = new(cursorObject.transform.position.x, cursorObject.transform.position.y, -20);
            }
        }
        public bool ValidationCheckForDecoration(LevelEditorItem selectedItem, Vector2Int gridPos)
        {
            ItemStateDetails defaultState = selectedItem.states[0];
            Vector2Int startPos = new(gridPos.x, gridPos.y - (Mathf.FloorToInt(defaultState.dimension.y / 2)));
            GridCursor.Singleton.SetCursor(gridPos, defaultState.dimension,defaultState.cursorType);
            string decoKey = LevelCreateManager.GetTileKey(gridPos.x, gridPos.y);
            if (cursorObject)
            {
                cursorObject.transform.position = new(gridPos.x + defaultState.position.x, gridPos.y + defaultState.position.y,0);
                cursorObject.transform.rotation = Quaternion.Euler(0f, 0f, defaultState.rotation);
            }
            if (decorationDictionary.TryGetValue(decoKey, out var decorationParentProperty))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DestroyDecoration(decorationParentProperty);
                }
                return false;
            }

            for (int y = startPos.y; y < startPos.y+ defaultState.dimension.y; y++)
            {
                string tileKey = LevelCreateManager.GetTileKey(gridPos.x,y);
                if (!IsTileEmpty(tileKey))
                {
                    return false;
                }
                if (defaultState.supportType == SupportType.Down && !LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x, gridPos.y - 2)))
                {
                    return false;
                }
                if (defaultState.supportType == SupportType.Up && !LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x, gridPos.y +1)))
                {
                    return false;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                CreateDecoration(selectedItem, gridPos);
            }
            return true;
        }
        private bool IsTileEmpty(string tileKey)
        {
            return !occupiedTiles.ContainsKey(tileKey) &&
                   !LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) &&
                   !LevelCreateManager.Singleton.PlatformHandler.tileDictionary.ContainsKey(tileKey) &&
                   !LevelCreateManager.Singleton.ClimbableHandler.climbableDictionary.ContainsKey(tileKey);
        }
        void CreateDecoration(LevelEditorItem selectedItem,Vector2Int gridPos)
        {
            DecorationProperty newParentDetails = new()
            {
                itemId = selectedItem.id,
                pos = new(gridPos.x, gridPos.y)
            };
            PlaceDecoration(newParentDetails);
        }
        void PlaceDecoration(DecorationProperty parentDetails)
        {
            LevelEditorItem item = ItemManager.Singleton.GetItemDetails(parentDetails.itemId, ItemCategory.Decoration);
            ItemStateDetails defaultState = item.states[0];

            string newParentID = LevelCreateManager.GetTileKey(parentDetails.pos.x, parentDetails.pos.y);
            decorationDictionary.Add(newParentID, parentDetails);


            Vector2Int startPos = new(parentDetails.pos.x, parentDetails.pos.y - (Mathf.FloorToInt(defaultState.dimension.y / 2)));
            for (int y = startPos.y, tileIndex = 0; y < startPos.y + defaultState.dimension.y; y++, tileIndex++)
            {
                string newChildID = LevelCreateManager.GetTileKey(startPos.x, y);
                occupiedTiles.Add(newChildID, newParentID);
            }


            Tile[] decoTiles = decorationSO.details[parentDetails.itemId].tiles;
            for (int y = startPos.y, tileIndex = 0; y < startPos.y + defaultState.dimension.y; y++, tileIndex++)
            {
                if(decoTiles == null || decoTiles.Length <=0) { continue; }
                decorationTileMap.SetTile(new(startPos.x, y), decoTiles[tileIndex]);
            }


            GameObject decoObjToSpawn = decorationSO.details[parentDetails.itemId].prefab;
            if (decoObjToSpawn)
            {
                GameObject decoObj = Instantiate(item.editorPrefab,
                    new(parentDetails.pos.x + defaultState.position.x,
                    parentDetails.pos.y + defaultState.position.y), 
                    Quaternion.Euler(0,0,defaultState.rotation), 
                    LevelCreateManager.Singleton.transform);
                decorationGameObjectDictionary.Add(newParentID, decoObj);
            }
        }
        void DestroyDecoration(DecorationProperty decoProperty)
        {
            LevelEditorItem item = ItemManager.Singleton.GetItemDetails(decoProperty.itemId, ItemCategory.Decoration);
            ItemStateDetails defaultState = item.states[0];
            string newParentID = LevelCreateManager.GetTileKey(decoProperty.pos.x, decoProperty.pos.y);
            decorationDictionary.Remove(newParentID);

            Vector2Int startPos = new(decoProperty.pos.x, decoProperty.pos.y - (Mathf.FloorToInt(defaultState.dimension.y / 2)));
            
            for (int y = startPos.y; y < startPos.y + defaultState.dimension.y; y++)
            {
                decorationTileMap.SetTile(new(startPos.x, y), null);
                string newChildID = LevelCreateManager.GetTileKey(startPos.x, y);
                occupiedTiles.Remove(newChildID);
            }

            if (decorationGameObjectDictionary.TryGetValue(newParentID, out var decoObj))
            {
                Destroy(decoObj);
                decorationGameObjectDictionary.Remove(newParentID);
            }
        }

        public LevelData Save()
        {
            return new() { DecorationDictionary = decorationDictionary };
        }

        public void Load(LevelData mapSave)
        {
            decorationDictionary = new();
            occupiedTiles = new();
            decorationTileMap.ClearAllTiles();
            foreach (var item in mapSave.DecorationDictionary.Values)
            {
                PlaceDecoration(item);
            }
        }
    }

    [System.Serializable]
    public class DecorationProperty
    {
        public int itemId;
        public int state;
        public Vector2IntSerializable pos;
    }
}

