using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelBuilder
{
    public class UiItemInfo : MonoBehaviour
    {
        public Image itemImage;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemDescription;

        public void ShowItemInfo(LevelEditorItem levelEditorItem)
        {
            if(levelEditorItem == null) { gameObject.SetActive(false) ;return; }
            gameObject.SetActive(true);
            itemImage.sprite = levelEditorItem.uiSprite;
            itemName.text = levelEditorItem.name;
            itemDescription.text = levelEditorItem.description;
        }
    }
}

