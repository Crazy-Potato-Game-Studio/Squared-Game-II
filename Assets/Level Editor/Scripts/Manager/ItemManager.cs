using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilder
{
    public class ItemManager : MonoBehaviour
    {
        [Tooltip("According The The ItemCategory Enum")]
        [SerializeField] private ItemSO[] itemsSOArray;
        public LevelEditorItem selectedItem;
        public List<Dictionary<int, LevelEditorItem>> itemDetailsDictionaryList;
        public static ItemManager Singleton;

        private void Awake()
        {
            Singleton = this;
            InitializeItemSOs();
        }
        private void Start()
        {
            selectedItem = null;
        }
        private void InitializeItemSOs()
        {
            int itemCategoryCount = Enum.GetNames(typeof(ItemCategory)).Length;
            if (itemsSOArray.Length != itemCategoryCount) { Debug.LogError("Failed To Sync Item Category With Scriptable Item Array."); return; }
            itemDetailsDictionaryList = new();

            for (int i = 0; i < itemCategoryCount; i++)
            {
                Dictionary<int, LevelEditorItem> newCategoryItemsDictionary = new();
                itemDetailsDictionaryList.Add(newCategoryItemsDictionary);

                int itemId = 0;
                foreach (var item in itemsSOArray[i].items)
                {
                    if (!item.includeInGame) { continue; }
                    itemDetailsDictionaryList[i].Add(itemId, item);
                    item.id = itemId;
                    itemId++;
                }
            }
        }
        public LevelEditorItem GetItemDetails(int id,ItemCategory itemCategory)
        {
            int itemDetailsDictionaryListIndex = (int)itemCategory;
            if (itemDetailsDictionaryList[itemDetailsDictionaryListIndex].TryGetValue(id, out var item)) return item; return null;
        }
        
        public Dictionary<int,LevelEditorItem> GetItemDetailsList(ItemCategory itemCategory)
        {
            return itemDetailsDictionaryList[(int)itemCategory];
        }
        public void SelectItem(ItemCategory category,int id)
        {
            LevelEditorItem newSelectedItem = GetItemDetails(id, category);
            selectedItem = newSelectedItem == selectedItem ? null : newSelectedItem;
            LevelCreateManager.Singleton.SelectItem(selectedItem);
        }
    }


    public enum ValidationType
    {
        None,
        Floor,
        Platform,
        Decoration,
        Enemy,
        Interactable,
        Items,
        Climbable,
        Background,
        Other,
        Foliage
    }
    public enum ItemCategory
    {
        Floor,
        Decoration,
        Enemy,
        Interactable,
        Items,
        Foliage,
        Other,
    }
    public enum PlacementType
    {
        SingleGrid,
        VarticleGrid,
        HorizontalGrid,
        BoxGrid,
        None
    }

    public enum GroundValidationType
    {
        None,
        GroundCheck,
        GroundAndCeilingCheck
    }

    


}

