using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelBuilder
{
    public class UiEnemyPropertyManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [Header("Drop 1")]
        [SerializeField] private Button[] drop1Button;
        [SerializeField] private Image[] drop1ButtonBorder;
        [Header("Drop 2")]
        [SerializeField] private Button[] drop2Button;
        [SerializeField] private Image[] drop2ButtonBorder;
        [Header("Min Item Drop Qantity")]
        [SerializeField] private TextMeshProUGUI minDropText;
        [SerializeField] private Slider minDropQuantitySlider;
        [Header("Max Item Drop Qantity")]
        [SerializeField] private TextMeshProUGUI maxDropText;
        [SerializeField] private Slider maxDropQuantitySlider;
        public ObjectProperty enemyProperty;
        List<LevelEditorItem> itemsList;
        public void SetDropableItemsDetailsList()
        {
            itemsList = new();
            for (int i = 0; i < ItemManager.Singleton.GetItemDetailsList(ItemCategory.Items).Count; i++)
            {
                itemsList.Add(ItemManager.Singleton.GetItemDetailsList(ItemCategory.Items)[i]);
            }
        }
        //public void SetEnemyPropertyUi(ObjectProperty enemyProperty_)
        //{
        //    if(itemsList == null) { SetDropableItemsDetailsList(); }
        //    gameObject.SetActive(true);
        //    minDropQuantitySlider.onValueChanged.AddListener(MinDropChange);
        //    maxDropQuantitySlider.onValueChanged.AddListener(MaxDropChange);

        //    enemyProperty = enemyProperty_;
        //    LevelEditorItem itemDetails = ItemManager.Singleton.GetItemDetails(enemyProperty.enemyProperty.id,ItemCategory.Enemy);

        //    nameText.text = itemDetails.name;
        //    Ui_SetDrop1(enemyProperty.enemyProperty.defaultDropItem1Id);
        //    Ui_SetDrop2(enemyProperty.enemyProperty.defaultDropItem2Id);
        //    minDropQuantitySlider.SetValueWithoutNotify(enemyProperty.enemyProperty.defaultDropItemMinQuantity);
        //    maxDropQuantitySlider.SetValueWithoutNotify(enemyProperty.enemyProperty.defaultDropItemMaxQuantity);
        //    minDropText.text = "Minimum Drop Amount[" + enemyProperty.enemyProperty.defaultDropItemMinQuantity.ToString("00") + "]";
        //    maxDropText.text = "Minimum Drop Amount[" + enemyProperty.enemyProperty.defaultDropItemMaxQuantity.ToString("00") + "]";
        //}
        //private void MinDropChange(float value)
        //{
        //    if (maxDropQuantitySlider.value < value) { maxDropQuantitySlider.value = value; }
        //    enemyProperty.enemyProperty.defaultDropItemMinQuantity = (int)value;
        //    minDropText.text = "Minimum Drop Amount["+ value.ToString("00")+"]";
        //}
        //private void MaxDropChange(float value)
        //{
        //    if(minDropQuantitySlider.value > value) {  minDropQuantitySlider.value = value; }
        //    enemyProperty.enemyProperty.defaultDropItemMaxQuantity = (int)value;
        //    maxDropText.text = "Minimum Drop Amount[" + value.ToString("00") + "]";
        //}

        //public void Ui_SetDrop1(int itemId)
        //{
        //    for (int i = 0; i < drop1ButtonBorder.Length; i++)
        //    {
        //        drop1ButtonBorder[i].gameObject.SetActive(itemsList[i].id == itemId);
        //    }
        //    enemyProperty.enemyProperty.defaultDropItem1Id = itemId;
        //}

        //public void Ui_SetDrop2(int itemId)
        //{
        //    for (int i = 0; i < drop2ButtonBorder.Length; i++)
        //    {
        //        drop2ButtonBorder[i].gameObject.SetActive(itemsList[i].id == itemId);
        //    }
        //    enemyProperty.enemyProperty.defaultDropItem2Id = itemId;
        //}

        //public void Ui_RemoveEnemy()
        //{
        //    Destroy(enemyProperty.gameObject);
        //    gameObject.SetActive(false);
        //}

        //public void Ui_FlipEnemy()
        //{
        //    enemyProperty.enemyProperty.fliped = !enemyProperty.enemyProperty.fliped;
        //}

        //public void Ui_Done()
        //{
        //    gameObject.SetActive(false);
        //}
    }
}