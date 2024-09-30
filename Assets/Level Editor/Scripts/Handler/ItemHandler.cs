using System.Collections.Generic;
using UnityEngine;
namespace LevelBuilder
{
    public class ItemHandler : MonoBehaviour,ISaveLoad
    {
        public Dictionary<string, TileProperty> itemDictionary = new();
        Dictionary<string, GameObject> itemObjectDictionary = new();
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        private string uniqueId = "Item";
        public void OnEnable()
        {
            SaveLoadManager.Singleton.saveLoadList.Add(this);
        }
        

        

        public bool ValidationCheckForIItem(LevelEditorItem selectedItem, Vector3Int gridPos)
        {
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
            else
            {
                if (!LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) &&
                    !LevelCreateManager.Singleton.PlatformHandler.childDictionary.ContainsKey(tileKey) &&
                    !LevelCreateManager.Singleton.InteractableHandler.childDictionary.ContainsKey(tileKey))
                {
                    if (LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(TileHandler.GetTileKey(gridPos.x, gridPos.y - 1)))
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            TileProperty newItemTileProperty = new(selectedItem.id, gridPos.x, gridPos.y);
                            PlaceItem(newItemTileProperty);
                        }
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        private void PlaceItem(TileProperty itemProperty)
        {
            string tileKey = TileHandler.GetTileKey(itemProperty.position.x, itemProperty.position.y);
            LevelEditorItem item = ItemManager.Singleton.GetItemDetails(itemProperty.id, ItemCategory.Items);
            GameObject itemObject = Instantiate(item.editorPrefab, new(itemProperty.position.x + item.objectOffset.x, itemProperty.position.y + item.objectOffset.y), Quaternion.identity);
            itemDictionary.Add(tileKey, itemProperty);
            itemObjectDictionary.Add(tileKey, itemObject);
        }

        public LevelSave Save()
        {
            LevelSave levelSave = new()
            {
                ItemDictionary = itemDictionary
            };
            return levelSave;
        }

        public void Load(LevelSave mapSave)
        {
            foreach (var item in mapSave.ItemDictionary.Values)
            {
                PlaceItem(item);
            }
        }
    }
}

