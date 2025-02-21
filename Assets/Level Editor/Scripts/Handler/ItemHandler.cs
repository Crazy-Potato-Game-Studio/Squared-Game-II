using System.Collections.Generic;
using UnityEngine;
namespace LevelBuilder
{
    public class ItemHandler : MonoBehaviour,ISaveLoad
    {
        public Dictionary<string, TileProperty> itemDictionary = new();
        Dictionary<string, GameObject> itemObjectDictionary = new();
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        private string uniqueId = SaveLoadManager.SaveItem;
        GameObject cursorObject;
        private void OnEnable()
        {
            LevelCreateManager.ItemSelectEvent += OnItemSelect;
            LevelCreateManager.OverUiEvent += CursorOverUI;
        }
        private void CursorOverUI()
        {
            if (cursorObject != null)
            {
                cursorObject.transform.position = new(cursorObject.transform.position.x, cursorObject.transform.position.y, -20);
            }
        }
        void OnItemSelect(LevelEditorItem selectedItem)
        {
            if (cursorObject != null) { Destroy(cursorObject); }
            if (selectedItem == null) { return; }
            if (selectedItem.validationType != ValidationType.Items) { return; }

            if (selectedItem.editorPrefab == null) { return; }
            cursorObject = Instantiate(selectedItem.editorPrefab, new Vector3(0, 0, -20), Quaternion.identity);
            cursorObject.transform.name = "=====" + cursorObject.transform.name;
        }
        public bool ValidationCheckForIItem(LevelEditorItem selectedItem, Vector2Int gridPos)
        {
            ItemStateDetails stateDetails = selectedItem.states[0];
            GridCursor.Singleton.SetCursor(gridPos, stateDetails.dimension, stateDetails.cursorType);
            if (cursorObject)
            {
                cursorObject.transform.position = new(gridPos.x + stateDetails.position.x, gridPos.y + stateDetails.position.y, 0f);
            }

            string tileKey = TileHandler.GetTileKey(gridPos.x, gridPos.y);
            if (itemDictionary.ContainsKey(tileKey))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    itemObjectDictionary.TryGetValue(tileKey, out var item);
                    Destroy(item);
                    itemDictionary.Remove(tileKey);
                    itemObjectDictionary.Remove(tileKey);
                }
                return false;
            }

            bool hasFloor = LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(TileHandler.GetTileKey(gridPos.x, gridPos.y - 1));
            if (hasFloor && TileEmpty(tileKey))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    TileProperty newItemTileProperty = new(selectedItem.id, new(gridPos.x, gridPos.y));
                    PlaceItem(newItemTileProperty);
                }
                return true;
            }
            return false;
        }
        private bool TileEmpty(string tileKey)
        {
            return !itemDictionary.ContainsKey(tileKey)&&
                    !LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) &&
                    !LevelCreateManager.Singleton.PlatformHandler.tileDictionary.ContainsKey(tileKey) &&
                    !LevelCreateManager.Singleton.ClimbableHandler.climbableDictionary.ContainsKey(tileKey) &&
                    !LevelCreateManager.Singleton.InteractableHandler.occupiedTileDictionary.ContainsKey(tileKey);
        }
        private void PlaceItem(TileProperty itemProperty)
        {
            string tileKey = TileHandler.GetTileKey(itemProperty.position.x, itemProperty.position.y);
            itemDictionary.Add(tileKey, itemProperty);
            LevelEditorItem item = ItemManager.Singleton.GetItemDetails(itemProperty.id, ItemCategory.Items);
            ItemStateDetails itemState = item.states[0];
            GameObject itemObject = Instantiate(item.editorPrefab, new(itemProperty.position.x + itemState.position.x, itemProperty.position.y + itemState.position.y), Quaternion.identity);
            itemObjectDictionary.Add(tileKey, itemObject);
        }

        public LevelData Save()
        {
            LevelData levelSave = new()
            {
                ItemDictionary = itemDictionary
            };
            return levelSave;
        }

        public void Load(LevelData mapSave)
        {
            foreach (var item in mapSave.ItemDictionary.Values)
            {
                PlaceItem(item);
            }
        }
    }
}

