using System.Collections.Generic;
using UnityEngine;
namespace LevelBuilder
{
    [DisallowMultipleComponent]
    public class EnemyHandler : MonoBehaviour, ISaveLoad
    {
        public Dictionary<string, ObjectProperty> enemyDictionary = new();
        Dictionary<string, GameObject> enemyObjects = new();
        public List<string> enemyTiles = new();
        GameObject cursorObject;
        private string uniqueId = SaveLoadManager.SaveEnemy;
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        int state;
        public void OnEnable()
        {
            LevelCreateManager.ItemSelectEvent += OnItemSelect;
            LevelCreateManager.OverUiEvent += CursorOverUI;
        }
        void OnItemSelect(LevelEditorItem selectedItem)
        {
            if (cursorObject != null) { Destroy(cursorObject); }
            if (selectedItem == null) { return; }
            if (selectedItem.validationType != ValidationType.Enemy) { return; }
            state = 0;

            if (selectedItem.editorPrefab == null) { return; }
            cursorObject = Instantiate(selectedItem.editorPrefab,
                new Vector3(0,0,-20), 
                Quaternion.Euler(0f, selectedItem.states[state].rotation, 0f),
                LevelCreateManager.Singleton.cursorObjectParent);
            cursorObject.transform.name = "====="+cursorObject.transform.name;
        }
        private void CursorOverUI()
        {
            if (cursorObject != null)
            {
                cursorObject.transform.position = new(cursorObject.transform.position.x, cursorObject.transform.position.y, -20);
            }
        }
        public bool ValidationCheckForEnemy(LevelEditorItem selectedItem, Vector2Int gridPos)
        {
            ItemStateDetails stateDetails = selectedItem.states[state];
            GridCursor.Singleton.SetCursor(gridPos, stateDetails.dimension, stateDetails.cursorType);

            if (Input.GetKeyDown(KeyCode.R) && selectedItem.states.Length > 1)
            {
                state++;
                state = state == selectedItem.states.Length ? 0 : state;
            }
            if (cursorObject)
            {
                cursorObject.transform.position = new(gridPos.x + stateDetails.position.x, gridPos.y + stateDetails.position.y,0);
                cursorObject.transform.rotation = Quaternion.Euler(0f, stateDetails.rotation, 0f);
            }

            if (enemyDictionary.TryGetValue(TileHandler.GetTileKey(gridPos),out var enemyProperty))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DestroyEnemy(enemyProperty);
                }
                return false;
            }
            Vector2Int startPos = GetBottomLeftPos(new(gridPos.x, gridPos.y), stateDetails);
            if (TileValidation(startPos, stateDetails))
            {
                bool hasGround = selectedItem.states[state].supportType == SupportType.None 
                    || GroundValidation(startPos, stateDetails);
                if (Input.GetMouseButtonDown(0) && hasGround)
                {
                    ObjectProperty objectProperty = CreateObjectProperty(selectedItem, gridPos, ItemCategory.Enemy);
                    PlaceEnemy(objectProperty);
                }
                return hasGround;
            }
            return false;
        }
        
        bool TileValidation(Vector2Int startPos, ItemStateDetails stateDetails)
        {
            for (int x = startPos.x; x < startPos.x + stateDetails.dimension.x; x++)
            {
                for (int y = startPos.y; y < startPos.y + stateDetails.dimension.y; y++)
                {
                    string tileKey = LevelCreateManager.GetTileKey(x, y);
                    if (LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) ||
                       LevelCreateManager.Singleton.PlatformHandler.tileDictionary.ContainsKey(tileKey) ||
                       LevelCreateManager.Singleton.ClimbableHandler.climbableDictionary.ContainsKey(tileKey) ||
                       LevelCreateManager.Singleton.InteractableHandler.interactableDictionary.ContainsKey(tileKey) ||
                       enemyTiles.Contains(tileKey))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        bool GroundValidation(Vector2Int startPos, ItemStateDetails stateDetails)
        {
            for (int x = startPos.x; x < startPos.x + stateDetails.dimension.x; x++)
            {
                if (!LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(x, startPos.y - 1)))
                {
                    return false;
                }
            }
            return true;
        }

        public ObjectProperty CreateObjectProperty(LevelEditorItem selectedItem, Vector2Int gridPos,ItemCategory category)
        {
            ObjectProperty objectProperty = new()
            {
                id = selectedItem.id,
                pos = new(gridPos.x, gridPos.y),
                state = state,
            };
            return objectProperty;
        }

        public void PlaceEnemy(ObjectProperty enemyProperty)
        {
            LevelEditorItem selectedItem = ItemManager.Singleton.GetItemDetails(enemyProperty.id, ItemCategory.Enemy);
            ItemStateDetails stateDetails = selectedItem.states[enemyProperty.state];

            Vector2 enemyPosition = new Vector2(enemyProperty.pos.x, enemyProperty.pos.y) + stateDetails.position;
            GameObject enemyObject = Instantiate(selectedItem.editorPrefab, enemyPosition, Quaternion.Euler(0f, stateDetails.rotation, 0f), LevelCreateManager.Singleton.transform);

            string enemyKey = TileHandler.GetTileKey(enemyProperty.pos.x, enemyProperty.pos.y);
            enemyObjects.Add(enemyKey, enemyObject);
            enemyDictionary.Add(enemyKey, enemyProperty);

            Vector2Int startPos = GetBottomLeftPos(new(enemyProperty.pos.x, enemyProperty.pos.y),stateDetails);
            for (int x = startPos.x; x < startPos.x + stateDetails.dimension.x; x++)
            {
                for (int y = startPos.y; y < startPos.y + stateDetails.dimension.y; y++)
                {
                    string tileKey = LevelCreateManager.GetTileKey(x, y);
                    enemyTiles.Add(tileKey);
                }
            }
        }


        public void DestroyEnemy(ObjectProperty enemyProperty)
        {
            string enemyKey = TileHandler.GetTileKey(enemyProperty.pos.x, enemyProperty.pos.y);
            LevelEditorItem selectedItem = ItemManager.Singleton.GetItemDetails(enemyProperty.id, ItemCategory.Enemy);
            ItemStateDetails stateDetails = selectedItem.states[state];

            enemyDictionary.Remove(enemyKey);
            if (enemyObjects.TryGetValue(enemyKey, out var Obj))
            {
                Destroy(Obj);
                enemyObjects.Remove(enemyKey);
            }


            Vector2Int startPos = GetBottomLeftPos(new(enemyProperty.pos.x, enemyProperty.pos.y), stateDetails);
            for (int x = startPos.x; x < startPos.x + stateDetails.dimension.x; x++)
            {
                for (int y = startPos.y; y < startPos.y + stateDetails.dimension.y; y++)
                {
                    string tileKey = LevelCreateManager.GetTileKey(x, y);
                    enemyTiles.Remove(tileKey);
                }
            }
        }
        Vector2Int GetBottomLeftPos(Vector2Int gridPos, ItemStateDetails stateDetails)
        {
            Vector2Int tileDimensionBottomLeftPos = new();
            if (stateDetails.alignment == ObjectAlignment.Bottom)
            {
                tileDimensionBottomLeftPos.x = gridPos.x - (int)stateDetails.dimension.x / 2;
                tileDimensionBottomLeftPos.y = gridPos.y;
            }
            else if (stateDetails.alignment == ObjectAlignment.Top)
            {
                tileDimensionBottomLeftPos.x = gridPos.x - (int)stateDetails.dimension.x / 2;
                tileDimensionBottomLeftPos.y = gridPos.y - stateDetails.dimension.y + 1;
            }
            else if (stateDetails.alignment == ObjectAlignment.Left)
            {
                tileDimensionBottomLeftPos.x = gridPos.x;
                tileDimensionBottomLeftPos.y = gridPos.y - (int)stateDetails.dimension.y / 2;
            }
            else if (stateDetails.alignment == ObjectAlignment.Right)
            {
                tileDimensionBottomLeftPos.x = gridPos.x - stateDetails.dimension.x + 1;
                tileDimensionBottomLeftPos.y = gridPos.y - (int)stateDetails.dimension.y / 2;
            }
            else
            {
                tileDimensionBottomLeftPos.x = gridPos.x - (int)stateDetails.dimension.x / 2;
                tileDimensionBottomLeftPos.y = gridPos.y - (int)stateDetails.dimension.y / 2;
            }
            return tileDimensionBottomLeftPos;
        }
        public LevelData Save()
        {
            LevelData levelSave = new()
            {
                EnemyDictionary = new()
            };
            levelSave.EnemyDictionary = enemyDictionary;
            return levelSave;
        }

        public void Load(LevelData levelSave)
        {
            if (levelSave == null || levelSave.EnemyDictionary == null) { return; }
            foreach (var item in levelSave.EnemyDictionary.Values)
            {
                PlaceEnemy(item);
            }
        }
    }
    public enum EnemyType
    {
        Ground,
        Air,
        Boss
    }
    [System.Serializable]
    public struct EnemyProperty
    {
        [HideInInspector] public int id;
        [HideInInspector] public Vector2IntSerializable position;
        [ItemCodeDescription]
        public int defaultDropItem1Id;
        [ItemCodeDescription]
        public int defaultDropItem2Id;
        public int defaultDropItemMinQuantity;
        public int defaultDropItemMaxQuantity;
        public bool fliped;
    }
}

