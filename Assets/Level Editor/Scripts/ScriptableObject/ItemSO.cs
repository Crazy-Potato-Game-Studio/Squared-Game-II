using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace LevelBuilder
{
    [CreateAssetMenu(fileName = "Item SO", menuName = "Scriptable Object/Level Editor/Item SO")]
    public class ItemSO : ScriptableObject
    {
        public List<LevelEditorItem> items;

        private void OnValidate()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].id = i;
            }
        }
    }
    [System.Serializable]
    public class LevelEditorItem
    {
        public string name;
        [TextArea] public string description;
        public int id;
        public Sprite uiSprite;
        public GameObject editorPrefab;
        public GameObject gamePrefab;
        public ValidationType validationType;
        public ItemStateDetails[] states;
        public int maxAllowedItem;
        public bool includeInGame = true;
    }
}

[System.Serializable]
public class ItemStateDetails
{
    public Vector2 position;
    public float rotation;
    public Vector2Int dimension;
    public CursorType cursorType;
    public ObjectAlignment alignment;
    public SupportType supportType;
    public Tile tile;
}