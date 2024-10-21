using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LevelBuilder
{
    public class UiItem : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField] public Image uiImage;
        [SerializeField] public Image borderImage;
        public int itemId { get; private set; }
        public ItemCategory itemCategory { get; private set; }
        public void OnPointerClick(PointerEventData eventData)
        {
            ItemManager.Singleton.SelectItem(itemCategory, itemId);
            UiItemManager.Singleton.ShowSelectedItemInfo();
        }
        public void SetUiItem(LevelEditorItem item,ItemCategory category)
        {
            itemId = item.id;
            uiImage.sprite = item.uiSprite;
            itemCategory = category;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            UiItemManager.Singleton.ShowHoveredItemDetails(itemCategory, itemId,true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UiItemManager.Singleton.ShowHoveredItemDetails(itemCategory, itemId, false);
        }
    }
}

