using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelBuilder
{
    public class UiItemManager : MonoBehaviour
    {
        public UiItemInfo uiSelectedItemInfo;
        public UiItem itemUiPrefab;
        public GameObject itemCategoryUiPrefab;
        public Transform itemsUiParent;
        public Button[] categpryButtons;
        public Button allCategoryButton;
        public Color categorySelectedColor;
        public Color categoryNormalColor;
        private List<GameObject> uiItemCategoryGameObjectList = new();
        public static UiItemManager Singleton;
        private void Awake() => Singleton = this;

        void Start()
        {
            if (ItemManager.Singleton.itemDetailsDictionaryList == null) { return; }
            CreateCategoryGameObject();
            CreateUiItems();
        }

        void CreateCategoryGameObject()
        {
            int itemCategoryCount = Enum.GetNames(typeof(ItemCategory)).Length;
            for (int i = 0; i < itemCategoryCount; i++)
            {
                GameObject categoryObject = Instantiate(itemCategoryUiPrefab, itemsUiParent);
                uiItemCategoryGameObjectList.Add(categoryObject);
                categoryObject.transform.name = ((ItemCategory)i).ToString();
            }
        }

        void CreateUiItems()
        {
            for (int i = 0; i < ItemManager.Singleton.itemDetailsDictionaryList.Count; i++)
            {
                foreach (var item in ItemManager.Singleton.itemDetailsDictionaryList[i].Values)
                {
                    UiItem spawnedItem = Instantiate(itemUiPrefab.gameObject, uiItemCategoryGameObjectList[i].transform).GetComponent<UiItem>();
                    spawnedItem.SetUiItem(item, (ItemCategory)i);
                }
            }
        }



        public void ShowCategory(int CategoryEnumIndex)
        {
            allCategoryButton.image.color = categoryNormalColor;
            for (int i = 0; i < uiItemCategoryGameObjectList.Count; i++)
            {
                categpryButtons[i].image.color = (i == CategoryEnumIndex) ? categorySelectedColor : categoryNormalColor;
                uiItemCategoryGameObjectList[i].SetActive(i == CategoryEnumIndex);
            }
        }
        public void ShowAllItem()
        {
            allCategoryButton.image.color = categorySelectedColor;
            for (int i = 0; i < uiItemCategoryGameObjectList.Count; i++)
            {
                categpryButtons[i].image.color = categoryNormalColor;
                uiItemCategoryGameObjectList[i].SetActive(true);
            }
        }
        public void ShowSelectedItemInfo() => uiSelectedItemInfo.ShowItemInfo(ItemManager.Singleton.selectedItem);
        public void ShowHoveredItemDetails(ItemCategory category, int id, bool show)
        {

        }
    }
}