using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;

namespace LevelBuilder
{
    public class InteractableHandler : MonoBehaviour,ISaveLoad
    {
        public InteractableLink interactableLink;
        public Dictionary<string, ObjectProperty> interactableDictionary = new();
        public Dictionary<string, string> occupiedTileDictionary = new();
        public Dictionary<string, GameObject> interactableObjects= new();
        public Dictionary<int, int> interactableItemCountDictionary = new();
        GameObject cursorObject;
        public int state;
        private string uniqueId = SaveLoadManager.SaveInteractable;
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
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
            if (selectedItem.validationType != ValidationType.Interactable) { return; }
            state = 0;

            if (selectedItem.editorPrefab == null) { return; }
            cursorObject = Instantiate(selectedItem.editorPrefab, 
                new Vector3(0, 0, -20), 
                Quaternion.Euler(0f, 0f, selectedItem.states[state].rotation),
                LevelCreateManager.Singleton.cursorObjectParent);
            cursorObject.transform.name = "====="+cursorObject.transform.name;
        }

        public bool ValidationCheckForInteractableObject(LevelEditorItem selectedItem, Vector2Int gridPos)
        {
            ShowPopup(gridPos);
            ItemStateDetails stateDetails = selectedItem.states[state];
            GridCursor.Singleton.SetCursor(gridPos, stateDetails.dimension, stateDetails.cursorType);

            if (Input.GetKeyDown(KeyCode.R) && selectedItem.states.Length > 1)
            {
                state++;
                state = state == selectedItem.states.Length ? 0 : state;
            }

            if (cursorObject)
            {
                cursorObject.transform.position = new(gridPos.x + stateDetails.position.x, gridPos.y+ stateDetails.position.y,0f);
                cursorObject.transform.rotation = Quaternion.Euler(0f, 0f, stateDetails.rotation);
            }

            if (occupiedTileDictionary.TryGetValue(TileHandler.GetTileKey(gridPos.x, gridPos.y), out string parentKey))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    interactableDictionary.TryGetValue(parentKey, out var objectProperty);

                    if (interactableLink.triggerId.Contains(objectProperty.id))
                    {
                        ShowInteractableLinkProperty(objectProperty);
                    }
                    else
                    {
                        DestroyInteractable(objectProperty);
                    }
                }
                return false;
            }

            Vector2Int bottomLeftTilePos = GetBottomLeftPos(gridPos, stateDetails);
            if (stateDetails.supportType != SupportType.None && 
                !HasSupportTile(bottomLeftTilePos, stateDetails)) { return false; }

            for (int x = bottomLeftTilePos.x; x < bottomLeftTilePos.x + stateDetails.dimension.x; x++)
            {
                for (int y = bottomLeftTilePos.y; y < bottomLeftTilePos.y + stateDetails.dimension.y; y++)
                {
                    string tileKey = LevelCreateManager.GetTileKey(x, y);
                    if (!RequireTileEmpty(tileKey)) return false;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                ObjectProperty newInteractableProperty = CreateObjectProperty(selectedItem, gridPos, state, ItemCategory.Interactable);
                PlaceInteractable(newInteractableProperty);
            }
            return true;
        }
        void PlaceInteractable(ObjectProperty objectProperty)
        {
            LevelEditorItem selectedItem = ItemManager.Singleton.GetItemDetails(objectProperty.id, ItemCategory.Interactable);
            if (selectedItem.maxAllowedItem > 0)
            {
                if (!interactableItemCountDictionary.ContainsKey(objectProperty.id))
                {
                    interactableItemCountDictionary.Add(objectProperty.id, 0);
                }

                if (interactableItemCountDictionary[objectProperty.id] >= selectedItem.maxAllowedItem)
                {
                    return;
                }
                interactableItemCountDictionary[objectProperty.id]++;

            }

            Vector2Int gridPos = new(objectProperty.pos.x, objectProperty.pos.y);
            string parentKey = LevelCreateManager.GetTileKey(gridPos);
            ItemStateDetails stateDetails = selectedItem.states[objectProperty.state];


            GameObject newObject = Instantiate(selectedItem.editorPrefab, LevelCreateManager.Singleton.transform);
            newObject.transform.position = new(gridPos.x + stateDetails.position.x, gridPos.y + stateDetails.position.y);
            newObject.transform.rotation = Quaternion.Euler(new(0, 0, stateDetails.rotation));
            interactableObjects.Add(parentKey, newObject);


            Vector2Int bottomLeftTilePos = GetBottomLeftPos(gridPos, stateDetails);
            for (int x = 0; x < stateDetails.dimension.x; x++)
            {
                for (int y = 0; y < stateDetails.dimension.y; y++)
                {
                    string tileKey = LevelCreateManager.GetTileKey(bottomLeftTilePos.x + x, bottomLeftTilePos.y + y);
                    occupiedTileDictionary.Add(tileKey, parentKey);
                }
            }
            interactableDictionary.Add(parentKey, objectProperty);
        }

        public void DestroyInteractable(ObjectProperty property)
        {
            if(interactableItemCountDictionary.TryGetValue(property.id, out int count))
            {
                interactableItemCountDictionary[property.id]--;
                if (interactableItemCountDictionary[property.id] < 0)
                {
                    interactableItemCountDictionary[property.id] = 0;
                }
            }
            
            HandleObjectLink(property);

            string parentKey = TileHandler.GetTileKey(property.pos.x, property.pos.y);
            if (interactableObjects.TryGetValue(parentKey, out var obj))
            {
                Destroy(obj);
                interactableObjects.Remove(parentKey);
            }
            LevelEditorItem selectedItem = ItemManager.Singleton.GetItemDetails(property.id, ItemCategory.Interactable);
            ItemStateDetails stateDetails = selectedItem.states[property.state];
            Vector2Int bottomLeftTilePos = GetBottomLeftPos(new(property.pos.x, property.pos.y), stateDetails);

            for (int x = 0; x < stateDetails.dimension.x; x++)
            {
                for (int y = 0; y < stateDetails.dimension.y; y++)
                {
                    string tileKey = LevelCreateManager.GetTileKey(bottomLeftTilePos.x + x, bottomLeftTilePos.y + y);
                    occupiedTileDictionary.Remove(tileKey);
                }
            }
            interactableDictionary.Remove(parentKey);

        }
        private void ShowPopup(Vector2Int gridPos)
        {
            string key = TileHandler.GetTileKey(gridPos.x, gridPos.y);
            if (occupiedTileDictionary.TryGetValue(key, out var parentKey))
            {
                interactableDictionary.TryGetValue(parentKey, out var property);
                if (interactableLink.doorId.Contains(property.id))
                {
                    string doorKey = "Door [" + TileHandler.GetTileKey(property.pos.x, property.pos.y) + "]";
                    
                    interactableLink.popUp.ShowDoorName(doorKey);
                    return;
                }
            }
            interactableLink.popUp.gameObject.SetActive(false);
        }
        private bool RequireTileEmpty(string tileKey)
        {
            return !occupiedTileDictionary.ContainsKey(tileKey) &&
                   !LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) &&
                   !LevelCreateManager.Singleton.PlatformHandler.tileDictionary.ContainsKey(tileKey) &&
                   !LevelCreateManager.Singleton.ClimbableHandler.climbableDictionary.ContainsKey(tileKey);
        }
        Vector2Int GetBottomLeftPos(Vector2Int gridPos, ItemStateDetails stateDetails)
        {
            Vector2Int tileDimensionBottomLeftPos = new();
            if (stateDetails.alignment == ObjectAlignment.Bottom)
            {
                tileDimensionBottomLeftPos.x = gridPos.x - (int)stateDetails.dimension.x / 2;
                tileDimensionBottomLeftPos.y = gridPos.y;
            }
            else if (stateDetails.alignment == ObjectAlignment.Top)
            {
                tileDimensionBottomLeftPos.x = gridPos.x - (int)stateDetails.dimension.x / 2;
                tileDimensionBottomLeftPos.y = gridPos.y - stateDetails.dimension.y+1;
            }
            else if (stateDetails.alignment == ObjectAlignment.Left)
            {
                tileDimensionBottomLeftPos.x = gridPos.x;
                tileDimensionBottomLeftPos.y = gridPos.y - (int)stateDetails.dimension.y / 2;
            }
            else if (stateDetails.alignment == ObjectAlignment.Right)
            {
                tileDimensionBottomLeftPos.x = gridPos.x - stateDetails.dimension.x+1;
                tileDimensionBottomLeftPos.y = gridPos.y - (int)stateDetails.dimension.y / 2;
            }
            else
            {
                tileDimensionBottomLeftPos.x = gridPos.x - (int)stateDetails.dimension.x / 2;
                tileDimensionBottomLeftPos.y = gridPos.y - (int)stateDetails.dimension.y / 2;
            }
            return tileDimensionBottomLeftPos;
        }
        private bool HasSupportTile(Vector2Int bottomLeftTilePos, ItemStateDetails stateDetails)
        {
            List<Vector2Int> supportTiles = new();
            if (stateDetails.supportType.HasFlag(SupportType.Down))
            {
                for (int x = 0; x < stateDetails.dimension.x; x++)
                {
                    supportTiles.Add(new(bottomLeftTilePos.x + x, bottomLeftTilePos.y-1));
                }
            }
            if (stateDetails.supportType.HasFlag(SupportType.Up))
            {
                int yPos = bottomLeftTilePos.y + stateDetails.dimension.y;
                for (int x = 0; x < stateDetails.dimension.x; x++)
                {
                    supportTiles.Add(new(bottomLeftTilePos.x + x, yPos));
                }
            }
            if (stateDetails.supportType.HasFlag(SupportType.Left))
            {
                for (int y = 0; y < stateDetails.dimension.y; y++)
                {
                    supportTiles.Add(new(bottomLeftTilePos.x-1, bottomLeftTilePos.y + y));
                }
            }
            if (stateDetails.supportType.HasFlag(SupportType.Right))
            {
                int xPos = bottomLeftTilePos.x + stateDetails.dimension.x;
                for (int y = 0; y < stateDetails.dimension.y; y++)
                {
                    supportTiles.Add(new(xPos, bottomLeftTilePos.y+y));
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




        private void HandleObjectLink(ObjectProperty objectProperty)
        {
            if (!objectProperty.stringValues.TryGetValue(Settings.LinkedObjectKey, out var linkedObjectKey) ||
                !interactableDictionary.TryGetValue(linkedObjectKey, out var linkedObject) ||
                !linkedObject.stringValues.TryGetValue(Settings.LinkedObjectKey, out var linkedKeyOfLinkedObject))
                return;
            string thisInteratableKey = TileHandler.GetTileKey(objectProperty.pos.x, objectProperty.pos.y);
            if (linkedKeyOfLinkedObject == thisInteratableKey)
            {
                linkedObject.stringValues.Remove(Settings.LinkedObjectKey);
            }
        }


        public ObjectProperty CreateObjectProperty(LevelEditorItem selectedItem, Vector2Int gridPos, int state, ItemCategory category)
        {
            ObjectProperty objectProperty = new()
            {
                id = selectedItem.id,
                pos = new(gridPos.x, gridPos.y),
                state = state,
                stringValues = new(),
            };
            return objectProperty;
        }

        public void ShowInteractableLinkProperty(ObjectProperty triggerProperty)
        {
            LevelEditorItem item = ItemManager.Singleton.GetItemDetails(triggerProperty.id, ItemCategory.Interactable);
            interactableLink.interactableTitle.text = item.name;
            
            List<OptionData> optionsData;
            optionsData = new();
            optionsData.Add(new() { text = "Select" });
            foreach (var interactableObject in interactableDictionary.Values)
            {
                if (interactableLink.doorId.Contains(interactableObject.id))
                {
                    string doorKey = TileHandler.GetTileKey(new(interactableObject.pos.x, interactableObject.pos.y));
                    optionsData.Add(new() { text = doorKey });
                }
            }
            interactableLink.dropDown.options = optionsData;
            interactableLink.dropDown.value = 0;

            //Load trigger already linked door/object
            if (triggerProperty.stringValues.TryGetValue(Settings.LinkedObjectKey, out var linkedObject))
            {
                int optionIndexOfLinkedObject = optionsData.FindIndex(item => item.text == linkedObject);
                if(optionIndexOfLinkedObject == -1)
                { 
                    triggerProperty.stringValues.Remove(Settings.LinkedObjectKey);
                    return;
                }
                interactableLink.dropDown.value = optionIndexOfLinkedObject;
            }

            //set new linked object
            interactableLink.dropDown.onValueChanged.AddListener((index) => {

                triggerProperty.stringValues.Remove(Settings.LinkedObjectKey);
                if (index == 0) { return; }//0=drop down select/first option

                string newLinkedObjectKey = interactableLink.dropDown.options[index].text;
                triggerProperty.stringValues.Add(Settings.LinkedObjectKey, newLinkedObjectKey);

                string linkedTriggerKey = TileHandler.GetTileKey(triggerProperty.pos.x, triggerProperty.pos.y);
                interactableDictionary.TryGetValue(newLinkedObjectKey, out var newLinkedObject);
                HandleObjectLink(newLinkedObject);
                newLinkedObject.stringValues.Remove(Settings.LinkedObjectKey);
                newLinkedObject.stringValues.Add(Settings.LinkedObjectKey, linkedTriggerKey);

                Debug.Log("Linked!");
            });

            interactableLink.destroyTrigger.onClick.AddListener(() =>
            {
                interactableLink.dropDown.onValueChanged.RemoveAllListeners();
                interactableLink.destroyTrigger.onClick.RemoveAllListeners();
                interactableLink.Done.onClick.RemoveAllListeners();
                interactableLink.uiPanel.SetActive(false);

                DestroyInteractable(triggerProperty);
            });
            interactableLink.Done.onClick.AddListener(() =>
            {
                interactableLink.dropDown.onValueChanged.RemoveAllListeners();
                interactableLink.destroyTrigger.onClick.RemoveAllListeners();
                interactableLink.Done.onClick.RemoveAllListeners();
                interactableLink.uiPanel.SetActive(false);
            });
            interactableLink.uiPanel.SetActive(true);
        }
        public LevelData Save()
        {
            LevelData levelSave = new()
            {
                InteractableDictionary = this.interactableDictionary
            };
            return levelSave;
        }

        public void Load(LevelData mapSave)
        {
            if(mapSave == null || mapSave.InteractableDictionary == null) { return; }
            foreach (var interactable in mapSave.InteractableDictionary.Values)
            {
                PlaceInteractable(interactable);
            }
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

public static class Settings
{
    public const string LinkedObjectKey = "LK";
}

public enum ObjectAlignment
{
    Center,
    Right, 
    Left,
    Top,
    Bottom
}
[System.Serializable]
public class InteractableLink
{
    public List<int> doorId;
    public List<int> triggerId;
    public UiDoorPopUp popUp;
    public GameObject uiPanel;
    public TextMeshProUGUI interactableTitle;
    public TMP_Dropdown dropDown;
    public Button destroyTrigger;
    public Button Done;
}