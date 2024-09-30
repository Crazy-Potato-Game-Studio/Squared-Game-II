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
        public int id;
        public Sprite uiSprite;
        public GameObject editorPrefab;
        public GameObject gamePrefab;
        public Tile tile;
        [Space]
        public ValidationType validationType;
        public Vector2 objectOffset;
        public Vector2Int gridDimension;
        [TextArea] public string description;
        public PlacementType cursorType;
        public Extra extra;
        public bool includeInGame = true;
    }

    [System.Serializable]
    public class Extra
    {
        public int maxAllowedItem;
        public GroundValidationType checkType;
        public DefaultStandingBlock standingBlock;
        public bool changeable;
        public PropertyType propertyType;
        public enum DefaultStandingBlock
        {
            Down,
            Right,
            Up,
            Left
        }
        public Vector2 leftPos;
        public Vector2 upPos;
        public Vector2 RightPos;
    }
    public enum PropertyType
    {
        None,
        Door,
        Trigger,
        Portal,
        Spawn,
        Exit,
        RotatingObject,
    }
}
