using NativeSerializableDictionary;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace LevelBuilder
{
    public class InteractableHandler : MonoBehaviour,ISaveLoad
    {
        public UiPowerTriggerManager powerTriggerManager;
        public Dictionary<string, ObjectProperty> interactableDictionary = new();
        public Dictionary<string, string> childDictionary = new();
        //Gameobject
        public Dictionary<string, GameObject> interactableObjects= new();
        public Dictionary<string, GameObject> doors = new();
        public Dictionary<string, GameObject> triggers = new();
        public Dictionary<string, GameObject> portals = new();
        //Current
        ObjectTransformValue adjustValue = new();
        GameObject cursorGameObject;
        public int state;
        //Save
        public Dictionary<int,int> interactableItemCountDictionary = new();
        private string uniqueId = "Interactable";
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        private void OnEnable()
        {
            SaveLoadManager.Singleton.saveLoadList.Add(this);
            LevelCreateManager.ItemSelectEvent += OnSelectItem;
            LevelCreateManager.OverUiEvent += OverUi;
        }


        public void OverUi()
        {
            if (cursorGameObject != null)
            {
                cursorGameObject.transform.position = new(cursorGameObject.transform.position.x, cursorGameObject.transform.position.y, -20);
            }
        }
        private void OnSelectItem(LevelEditorItem selectedItem)
        {
            if(cursorGameObject!=null) { Destroy(cursorGameObject); }
            state = 0;
            if (selectedItem != null && selectedItem.validationType == ValidationType.Interactable)
            {
                state = (int)selectedItem.extra.standingBlock;
                Vector3Int gridPos = LevelCreateManager.Singleton.gridPos;
                adjustValue = GetObjectTransformAdjustValue(selectedItem, state);
                cursorGameObject = Instantiate(selectedItem.editorPrefab);
                cursorGameObject.transform.position = new(gridPos.x + adjustValue.positionX, gridPos.y + adjustValue.positionY, 0);
                cursorGameObject.transform.rotation = Quaternion.Euler(0, 0, adjustValue.rotationZ);
            }
        }
        public bool ValidationCheckForInteractableObject(LevelEditorItem selectedItem, Vector3Int gridPos)
        {
            if(cursorGameObject!= null) 
            {
                cursorGameObject.transform.position =  new(LevelCreateManager.Singleton.gridPos.x + selectedItem.objectOffset.x, LevelCreateManager.Singleton.gridPos.y + selectedItem.objectOffset.y, 0);
            }
            if (selectedItem.extra.changeable)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    state++;
                    state = state != 4 ? state : 0;
                    adjustValue = GetObjectTransformAdjustValue(selectedItem, state);
                    cursorGameObject.transform.rotation = Quaternion.Euler(0, 0, adjustValue.rotationZ);
                }
            }

            Vector2Int dimension = new();
            Vector3Int startPos = new();
            List<Vector2Int> groundTiles = new();
            List<Vector2Int> ceilingTiles = new();
            int middleX = Mathf.FloorToInt(selectedItem.gridDimension.x / 2);
            cursorGameObject.transform.position = new(gridPos.x+ adjustValue.positionX , gridPos.y+ adjustValue.positionY, 0);

            if (state == 0)
            {
                dimension = selectedItem.gridDimension;
                startPos = new(gridPos.x - middleX, gridPos.y);
                groundTiles.Clear();
                ceilingTiles.Clear();
                for (int i = startPos.x; i < startPos.x + selectedItem.gridDimension.x; i++)
                {
                    groundTiles.Add(new(i, gridPos.y - 1));
                    ceilingTiles.Add(new(i, gridPos.y + selectedItem.gridDimension.y));
                }
            }
            else if (state == 1)
            {
                dimension = new(selectedItem.gridDimension.y, selectedItem.gridDimension.x);
                startPos = new(gridPos.x, gridPos.y - middleX);
                groundTiles.Clear();
                ceilingTiles.Clear();
                for (int i = startPos.y; i < startPos.y + dimension.y; i++)
                {
                    groundTiles.Add(new(gridPos.x-1, i));
                    ceilingTiles.Add(new(gridPos.x + dimension.x, i));
                }
            }
            else if (state == 2)
            {
                dimension = selectedItem.gridDimension;
                startPos = new(gridPos.x - middleX, gridPos.y - dimension.y + 1);
                groundTiles.Clear();
                ceilingTiles.Clear();
                for (int i = startPos.x; i < startPos.x + selectedItem.gridDimension.x; i++)
                {
                    groundTiles.Add(new(i, gridPos.y + 1));
                    ceilingTiles.Add(new(i, gridPos.y - selectedItem.gridDimension.y));
                }
            }
            else if (state == 3)
            {
                dimension = new(selectedItem.gridDimension.y, selectedItem.gridDimension.x);
                startPos = new(gridPos.x - dimension.x + 1, gridPos.y - middleX);
                groundTiles.Clear();
                ceilingTiles.Clear();
                for (int i = startPos.y; i < startPos.y + dimension.y; i++)
                {
                    groundTiles.Add(new(gridPos.x + 1, i));
                    ceilingTiles.Add(new(gridPos.x - dimension.x, i));
                }
            }

            if (selectedItem.id == 18 || selectedItem.id == 19 || selectedItem.id == 20)
            {
                dimension = new(selectedItem.gridDimension.x, selectedItem.gridDimension.y);
                startPos = new(gridPos.x - 1, gridPos.y - 1);
            }
            GridCursor.Singleton.DisplayMultiGridCursor(startPos, dimension);

            if (selectedItem.extra.checkType == GroundValidationType.GroundCheck || selectedItem.extra.checkType == GroundValidationType.GroundAndCeilingCheck)
            {
                for (int i = 0; i < groundTiles.Count; i++)
                {
                    if (!LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(groundTiles[i].x, groundTiles[i].y)))
                    {
                        return false;
                    }
                }
            }
            if (selectedItem.extra.checkType == GroundValidationType.GroundAndCeilingCheck)
            {
                for (int i = 0; i < ceilingTiles.Count; i++)
                {
                    if (!LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(ceilingTiles[i].x, ceilingTiles[i].y)))
                    {
                        return false;
                    }
                }
            }

            for (int x = startPos.x; x < startPos.x + dimension.x; x++)
            {
                for (int y = startPos.y; y < startPos.y + dimension.y; y++)
                {
                    string tileKey = LevelCreateManager.GetTileKey(x, y);
                    if (LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) ||
                       LevelCreateManager.Singleton.PlatformHandler.childDictionary.ContainsKey(tileKey) ||
                       LevelCreateManager.Singleton.ClimbableHandler.climbableDictionary.ContainsKey(tileKey) ||
                       childDictionary.ContainsKey(tileKey))
                    {
                        if (Input.GetMouseButtonDown(0) && childDictionary.TryGetValue((tileKey),out string parentKey))
                        {
                            childDictionary.TryGetValue(LevelCreateManager.GetTileKey(gridPos), out var currentChildParentKey);
                            if (parentKey == currentChildParentKey)
                            {
                                interactableDictionary.TryGetValue(parentKey, out var property);
                                LevelEditorItem item = ItemManager.Singleton.GetItemDetails(property.id, ItemCategory.Interactable);
                                if (item.extra.propertyType == PropertyType.Trigger)
                                {
                                    powerTriggerManager.ShowTrigger(interactableDictionary, property);
                                    return false;
                                } 
                                else if(item.extra.propertyType == PropertyType.Door)
                                {
                                    RemoveConnectedTrigger(property);
                                }
                                DestroyInteractable(parentKey);
                            }
                        }
                        return false;
                    }
                }
            }
            if(Input.GetMouseButtonDown(0))
            {
                ObjectProperty objectProperty = CreateObjectProperty(selectedItem, gridPos,state,ItemCategory.Interactable);
                PlaceInteractable(objectProperty);
            }
            return true;
        }

        private void RemoveConnectedTrigger(ObjectProperty doorProperty)
        {
            if (doorProperty.valueStrings.TryGetValue("ConnectedTrigger", out var triggerKey))
            {
                if(triggerKey == "NONE") { return; }
                if (interactableDictionary.TryGetValue(triggerKey, out var trigger))
                {
                    trigger.valueStrings.Remove("ConnectedDoor");
                    trigger.valueStrings.Add("ConnectedDoor","NONE");
                }
            }
        }

        void PlaceInteractable(ObjectProperty objectProperty)
        {
            LevelEditorItem selectedItem = ItemManager.Singleton.GetItemDetails(objectProperty.id,objectProperty.category);
            if (!interactableItemCountDictionary.ContainsKey(objectProperty.id))
            {
                interactableItemCountDictionary.Add(objectProperty.id, 0);
            }
            if (interactableItemCountDictionary[objectProperty.id] >= selectedItem.extra.maxAllowedItem)
            {
                return;
            }

            interactableItemCountDictionary[objectProperty.id]++;
            adjustValue = GetObjectTransformAdjustValue(selectedItem, objectProperty.state);
            Vector3Int gridPos = new(objectProperty.position.x,objectProperty.position.y);
            string parentKey = LevelCreateManager.GetTileKey(gridPos);


            GameObject newObject = Instantiate(selectedItem.editorPrefab, new Vector3(gridPos.x + adjustValue.positionX, gridPos.y + adjustValue.positionY,0), Quaternion.Euler(new(0,0, adjustValue.rotationZ)));
            interactableObjects.Add(parentKey, newObject);


            Vector2Int dimension = new();
            Vector3Int startPos = new();
            int middleX = Mathf.FloorToInt(selectedItem.gridDimension.x / 2);
            if (objectProperty.state == 0)
            {
                dimension = selectedItem.gridDimension;
                startPos = new(gridPos.x - middleX, gridPos.y);
            }
            else if (objectProperty.state == 1)
            {
                dimension = new(selectedItem.gridDimension.y, selectedItem.gridDimension.x);
                startPos = new(gridPos.x, gridPos.y - middleX);
            }
            else if (objectProperty.state == 2)
            {
                dimension = selectedItem.gridDimension;
                startPos = new(gridPos.x - middleX, gridPos.y - dimension.y + 1);
            }
            else if (objectProperty.state == 3)
            {
                dimension = new(selectedItem.gridDimension.y, selectedItem.gridDimension.x);
                startPos = new(gridPos.x - dimension.x + 1, gridPos.y - middleX);
            }
            
            for (int x = startPos.x; x < startPos.x + dimension.x; x++)
            {
                for (int y = startPos.y; y < startPos.y + dimension.y; y++)
                {
                    string tileKey = LevelCreateManager.GetTileKey(x, y);
                    childDictionary.Add(tileKey, parentKey);
                }
            }
            interactableDictionary.Add(parentKey, objectProperty);
            ManagePowerObject(objectProperty);
        }


        public void DestroyInteractable(string parentKey)
        {
            interactableDictionary.TryGetValue(parentKey, out var property);
            LevelEditorItem selectedItem = ItemManager.Singleton.GetItemDetails(property.id, property.category);
            interactableItemCountDictionary[property.id]--;
            if (interactableItemCountDictionary[property.id] < 0)
            {
                interactableItemCountDictionary[property.id] = 0;
            }

            if (doors.ContainsKey(parentKey))
            {
                interactableDictionary.TryGetValue(parentKey, out var doorProperty);
                if (doorProperty.valueStrings.TryGetValue("ConnectedTrigger", out var connectedTriggerKey))
                {
                    if (interactableDictionary.TryGetValue(connectedTriggerKey, out var triggerProperty))
                    {
                        triggerProperty.valueStrings.Remove("ConnectedDoor");
                        triggerProperty.valueStrings.Add("ConnectedDoor", "NONE");
                    }
                }
                doors.Remove(parentKey); 
            }
            if(triggers.ContainsKey(parentKey)) 
            {
                interactableDictionary.TryGetValue(parentKey, out var trigerProperty);
                if (trigerProperty.valueStrings.TryGetValue("ConnectedDoor", out var connectedDoorkey))
                {
                    if (interactableDictionary.TryGetValue(connectedDoorkey, out var doorProperty))
                    {
                        doorProperty.valueStrings.Remove("ConnectedTrigger");
                        doorProperty.valueStrings.Add("ConnectedTrigger", "NONE");
                    }
                }
                triggers.Remove(parentKey); 
            }
            RemovePropertyTile(property);
            if (interactableObjects.TryGetValue(parentKey, out var obj))
            {
                Destroy(obj);
                interactableObjects.Remove(parentKey);
            }
        }
        void RemovePropertyTile(ObjectProperty property)
        {
            LevelEditorItem selectedItem = ItemManager.Singleton.GetItemDetails(property.id, ItemCategory.Interactable);
            int middleX = Mathf.FloorToInt(selectedItem.gridDimension.x / 2);
            Vector2Int gridPos = new(property.position.x, property.position.y);
            Vector2Int dimension = new();
            Vector3Int cursorStartPos = new();
            if (property.state == 0)
            {
                dimension = selectedItem.gridDimension;
                cursorStartPos = new(gridPos.x - middleX, gridPos.y);
            }
            else if (property.state == 1)
            {
                dimension = new(selectedItem.gridDimension.y, selectedItem.gridDimension.x);
                cursorStartPos = new(gridPos.x, gridPos.y - middleX);
            }
            else if (property.state == 2)
            {
                dimension = selectedItem.gridDimension;
                cursorStartPos = new(gridPos.x - middleX, gridPos.y - dimension.y + 1);
            }
            else if (property.state == 3)
            {
                dimension = new(selectedItem.gridDimension.y, selectedItem.gridDimension.x);
                cursorStartPos = new(gridPos.x - dimension.x + 1, gridPos.y - middleX);
            }

            for (int x = cursorStartPos.x; x < cursorStartPos.x + dimension.x; x++)
            {
                for (int y = cursorStartPos.y; y < cursorStartPos.y + dimension.y; y++)
                {
                    string tileKey = LevelCreateManager.GetTileKey(x, y);
                    childDictionary.Remove(tileKey);
                }
            }
            interactableDictionary.Remove(property.key);
        }
        public ObjectTransformValue GetObjectTransformAdjustValue(LevelEditorItem selectedItem, int state_)
        {
            int State = state_;
            if (!selectedItem.extra.changeable) { State = 0; }


            ObjectTransformValue value = new();
            if (State == 0)
            {
                value.positionX = selectedItem.objectOffset.x;
                value.positionY = selectedItem.objectOffset.y;
                value.rotationZ = 0;
            }
            else if (State == 1)
            {
                value.positionX = selectedItem.extra.leftPos.x;
                value.positionY = selectedItem.extra.leftPos.y;
                value.rotationZ = -90;
            }
            else if (State == 2)
            {
                value.positionX = selectedItem.extra.upPos.x;
                value.positionY = selectedItem.extra.upPos.y;
                value.rotationZ = -180;
            }
            else if (State == 3)
            {
                value.positionX = selectedItem.extra.RightPos.x;
                value.positionY = selectedItem.extra.RightPos.y;
                value.rotationZ = 90;
            }
            if (selectedItem.validationType == ValidationType.Interactable && selectedItem.id == 17)
            {
                if (State == 0)
                {
                    value.rotationZ = -90;
                }
                else if(State == 1)
                {
                    value.rotationZ = 180;
                    
                }
                else if (State == 2)
                {
                    value.rotationZ = 90;

                }
                else if (State == 3)
                {
                    value.rotationZ = 0;
                }
            }
            return value;
        }
        void ManagePowerObject(ObjectProperty objectProperty)
        {
            LevelEditorItem item = ItemManager.Singleton.GetItemDetails(objectProperty.id, ItemCategory.Interactable);
            if (item.extra.propertyType == PropertyType.Door)
            {
                doors.Add(objectProperty.key, interactableObjects[objectProperty.key]);
            }
            if (item.extra.propertyType == PropertyType.Trigger)
            {
                triggers.Add(objectProperty.key, interactableObjects[objectProperty.key]);
            }
        }
        public LevelSave Save()
        {
            LevelSave levelSave = new();
            levelSave.InteractableDictionary = new();
            foreach (var dictionaryItem in interactableDictionary)
            {
                LevelEditorItem item = ItemManager.Singleton.GetItemDetails(dictionaryItem.Value.id, ItemCategory.Interactable);
                if (item.extra.propertyType == PropertyType.Spawn)
                {
                    levelSave.Spawn = dictionaryItem.Value;
                }
                else if (item.extra.propertyType == PropertyType.Exit)
                {
                    levelSave.Exit = dictionaryItem.Value;
                }
                else
                {
                    levelSave.InteractableDictionary.Add(dictionaryItem.Key, dictionaryItem.Value);
                }
            }
            return levelSave;
        }

        public void Load(LevelSave mapSave)
        {
            if (mapSave.Spawn != null)
            {
                PlaceInteractable(mapSave.Spawn);
            }
            if (mapSave.Exit != null)
            {
                PlaceInteractable(mapSave.Exit);
            }
            foreach (var interactable in mapSave.InteractableDictionary.Values)
            {
                PlaceInteractable(interactable);
            }
        }
        public ObjectProperty CreateObjectProperty(LevelEditorItem selectedItem, Vector3Int gridPos, int state, ItemCategory category)
        {
            ObjectProperty objectProperty = new()
            {
                id = selectedItem.id,
                key = TileHandler.GetTileKey(gridPos.x, gridPos.y),
                category = category,
                position = new(gridPos.x, gridPos.y),
                state = state,
                valueStrings = new(),
                valueFloats = new()
            };
            if(selectedItem.extra.propertyType == PropertyType.Trigger)
            {
                objectProperty.valueStrings.Add("ConnectedDoor", "NONE");
            }
            if (selectedItem.extra.propertyType == PropertyType.Door)
            {
                objectProperty.valueStrings.Add("ConnectedTrigger", "NONE");
            }
            return objectProperty;
        }
    }
    
    [System.Serializable]
    public class ObjectTransformValue
    {
        public float positionX;
        public float positionY;
        public float rotationZ;
    }
}

