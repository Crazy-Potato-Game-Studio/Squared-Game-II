using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace LevelBuilder
{
    [CustomPropertyDrawer(typeof(ItemCodeDescriptionAttribute))]
    public class ItemCodeDescriptionDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return EditorGUI.GetPropertyHeight(property) * 2;}

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                EditorGUI.BeginChangeCheck(); 
                var newValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height / 2), label, property.intValue);
                EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Name", GetItemDescription(property.intValue));
                if (EditorGUI.EndChangeCheck()) { property.intValue = newValue; }
            }

            EditorGUI.EndProperty();
        }

        private string GetItemDescription(int itemCode)
        {
            ItemSO so_itemList = AssetDatabase.LoadAssetAtPath("Assets/_Level Editor/Scriptable Objects/ItemDetails SO/Item_ItemSO.asset", typeof(ItemSO)) as ItemSO;
            List<LevelEditorItem> itemDetailsList = so_itemList.items;
            LevelEditorItem itemDetail = itemDetailsList.Find(x => x.id == itemCode);

            return itemDetail != null ? itemDetail.name : "";
        }
    }

    [CustomPropertyDrawer(typeof(EnemyItemCodeDescriptionAttribute))]
    public class EnemyItemCodeDescriptionDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) { return EditorGUI.GetPropertyHeight(property) * 2; }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                EditorGUI.BeginChangeCheck();
                var newValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height / 2), label, property.intValue);
                EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Name", GetItemDescription(property.intValue));
                if (EditorGUI.EndChangeCheck()) { property.intValue = newValue; }
            }

            EditorGUI.EndProperty();
        }

        private string GetItemDescription(int itemCode)
        {
            ItemSO so_itemList = AssetDatabase.LoadAssetAtPath("Assets/_Level Editor/Scriptable Objects/ItemDetails SO/Enemy_ItemSO.asset", typeof(ItemSO)) as ItemSO;

            List<LevelEditorItem> itemDetailsList = so_itemList.items;
            LevelEditorItem itemDetail = itemDetailsList.Find(x => x.id == itemCode);

            return itemDetail != null ? itemDetail.name : "";
        }
    }
}



