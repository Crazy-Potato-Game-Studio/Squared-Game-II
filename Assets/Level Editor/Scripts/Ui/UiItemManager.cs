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

        [SerializeField] private Transform[] categoryElements;

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
                GameObject categoryObject = Instantiate(itemCategoryUiPrefab, LookForParent(((ItemCategory)i).ToString()));
                categoryObject.transform.parent.GetComponent<DropdownElement>().subElementsList = categoryObject;
                categoryObject.SetActive(false);
                uiItemCategoryGameObjectList.Add(categoryObject);
                categoryObject.transform.name = ((ItemCategory)i).ToString();
            }
        }

        private Transform LookForParent(string category){
            Transform parent;
            switch (category)
            {
                case "Floor":
                    parent = categoryElements[0];
                    break;
                case "Interactable":
                    parent = categoryElements[1];
                    break;
                case "Items":
                    parent = categoryElements[2];
                    break;
                case "Enemy":
                    parent = categoryElements[3];
                    break;
                case "Foliage":
                    parent = categoryElements[4];
                    break;
                case "Decoration":
                    parent = categoryElements[5];
                    break;
                case "Other":
                    parent = categoryElements[6];
                    break;
                default:
                    parent = categoryElements[0];
                    break;
            }
            return parent;
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

        public void ShowAllItem()
        {
            //allCategoryButton.image.color = categorySelectedColor;
            for (int i = 0; i < uiItemCategoryGameObjectList.Count; i++)
            {
                //categpryButtons[i].image.color = categoryNormalColor;
                uiItemCategoryGameObjectList[i].SetActive(true);
            }
        }
        public void ShowSelectedItemInfo() => uiSelectedItemInfo.ShowItemInfo(ItemManager.Singleton.selectedItem);
        public void ShowHoveredItemDetails(ItemCategory category, int id, bool show)
        {
            
        }
    }
}