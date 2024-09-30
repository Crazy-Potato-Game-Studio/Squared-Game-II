using LevelBuilder;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.TMP_Dropdown;

public class UiPowerTriggerManager : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TMP_Dropdown dropDown;
    ObjectProperty triggerProperty;
    List<ObjectProperty> doorsProperty;
    List<OptionData> optionsData;
    private void OnDisable()
    {
        dropDown.onValueChanged.RemoveAllListeners();
        
    }
    public void ShowTrigger(Dictionary<string,ObjectProperty> interactableObjectDictionary, ObjectProperty objectProperty)
    {
        triggerProperty = objectProperty;
        SetUi(interactableObjectDictionary);
        LoadSavedData();
        dropDown.onValueChanged.AddListener(NewSelect);
        gameObject.SetActive(true);
    }
    void SetUi(Dictionary<string, ObjectProperty> interactableObjectDictionary)
    {
        LevelEditorItem item = ItemManager.Singleton.GetItemDetails(triggerProperty.id, ItemCategory.Interactable);
        title.text = item.name;
        optionsData = new();
        doorsProperty = new();
        optionsData.Add(new() { text = "Select" });


        foreach (var interactableObject in interactableObjectDictionary.Values)
        {
            LevelEditorItem interactableItem = ItemManager.Singleton.GetItemDetails(interactableObject.id, ItemCategory.Interactable);
            if (interactableItem.extra.propertyType == PropertyType.Door)
            {
                doorsProperty.Add(interactableObject);
                optionsData.Add(new() { text = interactableObject.key });
            }
        }
        dropDown.options = optionsData;
    }
    void LoadSavedData()
    {
        if (triggerProperty.valueStrings != null && triggerProperty.valueStrings.TryGetValue("ConnectedDoor", out var connectedDoorKey))
        {
            if(connectedDoorKey == "NONE")
            {
                dropDown.value = 0;
            }
            else
            {
                for (int i = 0; i < doorsProperty.Count; i++)
                {
                    if (doorsProperty[i].key != connectedDoorKey) { continue; }
                    dropDown.value = i + 1; return;
                }
                triggerProperty.valueStrings.Remove("ConnectedDoor");
                triggerProperty.valueStrings.Add("ConnectedDoor","NONE");
                dropDown.value = 0;
            }
        }
    }
    public void NewSelect(int value)
    {
        if (value == 0)
        {
            triggerProperty.valueStrings.Remove("ConnectedDoor");
            triggerProperty.valueStrings.Add("ConnectedDoor", "NONE");
        }
        else
        {
            triggerProperty.valueStrings.Remove("ConnectedDoor");
            triggerProperty.valueStrings.Add("ConnectedDoor", doorsProperty[value-1].key);

            doorsProperty[value - 1].valueStrings.Remove("ConnectedTrigger");
            doorsProperty[value - 1].valueStrings.Add("ConnectedTrigger", triggerProperty.key);
        }
    }
    public void RemoveInteractable()
    {
        LevelCreateManager.Singleton.InteractableHandler.DestroyInteractable(triggerProperty.key);
        triggerProperty = null;
        gameObject.SetActive(false);
    }

    public void Done()
    {
        gameObject.SetActive(false);
    }
}
