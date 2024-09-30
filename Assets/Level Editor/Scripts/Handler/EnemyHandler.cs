using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelBuilder
{
    [DisallowMultipleComponent]
    public class EnemyHandler : MonoBehaviour, ISaveLoad
    {
        public Dictionary<string, ObjectProperty> enemyPropertyDictionary = new();
        Dictionary<string, GameObject> enemyObjectDictionary = new();
        List<string> enemySpawnTileKey = new();
        GameObject cursorObject;
        private string uniqueId = "Enemy";
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }
        int state;
        public void OnEnable()
        {
            LevelCreateManager.ItemSelectEvent += ChangeState;
            LevelCreateManager.OverUiEvent += OverUi;
            SaveLoadManager.Singleton.saveLoadList.Add(this);
        }
        void ChangeState(LevelEditorItem item)
        {
            if (cursorObject != null) { Destroy(cursorObject); }
            if(item == null || item.validationType!= ValidationType.Enemy) { return; }
            if (item.extra.standingBlock == Extra.DefaultStandingBlock.Right)
            {
                state = 1;
            }
            else
            {
                state = -1;
            }
            cursorObject = Instantiate(item.editorPrefab);
            cursorObject.transform.localScale = new(state, 1, 1);

        }
        void OverUi()
        {
            if (cursorObject != null)
            {
                cursorObject.transform.position = new(cursorObject.transform.position.x, cursorObject.transform.position.y, -20);
            }
        }
        public void OpenProperty(PropertyObject propertyObject)
        {

        }
        public bool ValidationCheckForEnemy(LevelEditorItem selectedItem, Vector3Int gridPos)
        {
            GridCursor.Singleton.DisplayMultiGridCursor(gridPos, selectedItem.gridDimension);
            if (cursorObject != null)
            {
                cursorObject.transform.position = new(LevelCreateManager.Singleton.gridPos.x + GetOffset(selectedItem,state).x, LevelCreateManager.Singleton.gridPos.y + GetOffset(selectedItem, state).y, 0);
            }
            if (selectedItem.extra.changeable)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    state = (state == 1)? -1 : 1;
                    cursorObject.transform.localScale = new(state, 1, 1);
                }
            }
            if (!TileValidation(gridPos,selectedItem) && enemyPropertyDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos)))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DestroyEnemy(LevelCreateManager.GetTileKey(gridPos));
                }
                return false;
            }
            else if (TileValidation(gridPos, selectedItem))
            {
                if (selectedItem.extra.checkType == GroundValidationType.GroundCheck && GroundValidation(gridPos, selectedItem))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        ObjectProperty objectProperty = CreateObjectProperty(selectedItem,gridPos,ItemCategory.Enemy);
                        PlaceEnemy(objectProperty);
                    }
                    return true;
                }
                else if (selectedItem.extra.checkType == GroundValidationType.None)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        ObjectProperty objectProperty = CreateObjectProperty(selectedItem, gridPos, ItemCategory.Enemy);
                        PlaceEnemy(objectProperty);
                    }
                    return true;
                }
            }
            return false;
        }
        Vector2 GetOffset(LevelEditorItem item, int state)
        {
            if (state == -1)
            {
                return new(item.extra.leftPos.x, item.extra.leftPos.y);
            }
            else
            {
                return new(item.objectOffset.x, item.objectOffset.y);
            }
        }
        bool TileValidation(Vector3Int startPos,LevelEditorItem selectedItem)
        {
            for (int x = startPos.x; x < startPos.x + selectedItem.gridDimension.x; x++)
            {
                for (int y = startPos.y; y < startPos.y + selectedItem.gridDimension.y; y++)
                {
                    string tileKey = LevelCreateManager.GetTileKey(x, y);
                    if (LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) ||
                       LevelCreateManager.Singleton.PlatformHandler.childDictionary.ContainsKey(tileKey) ||
                       LevelCreateManager.Singleton.ClimbableHandler.climbableDictionary.ContainsKey(tileKey) ||
                       enemySpawnTileKey.Contains(tileKey))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        bool GroundValidation(Vector3Int gridPos, LevelEditorItem selectedItem)
        {
            for (int x = gridPos.x; x < gridPos.x + selectedItem.gridDimension.x; x++)
            {
                if (!LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(LevelCreateManager.GetTileKey(x, gridPos.y - 1)))
                {
                    return false;
                }
            }
            return true;
        }

        public ObjectProperty CreateObjectProperty(LevelEditorItem selectedItem, Vector3Int gridPos,ItemCategory category)
        {
            ObjectProperty objectProperty = new()
            {
                id = selectedItem.id,
                category = category,
                position = new(gridPos.x, gridPos.y),
                state = state,
                valueStrings = new(),
                valueFloats = new()
            };
            return objectProperty;
        }

        public void PlaceEnemy(ObjectProperty enemyProperty)
        {
            LevelEditorItem selectedItem = ItemManager.Singleton.GetItemDetails(enemyProperty.id,ItemCategory.Enemy);
            GameObject enemyObject = Instantiate(selectedItem.editorPrefab, new(enemyProperty.position.x+ GetOffset(selectedItem, enemyProperty.state).x, enemyProperty.position.y+ GetOffset(selectedItem, enemyProperty.state).y), Quaternion.identity);
            enemyObject.transform.localScale = new(enemyProperty.state, 1, 1);
            string parentKey = LevelCreateManager.GetTileKey(enemyProperty.position.x, enemyProperty.position.y);
            enemyObjectDictionary.Add(parentKey, enemyObject);

            Vector2Int startPos = new(enemyProperty.position.x, enemyProperty.position.y);
            for (int x = startPos.x; x < startPos.x + selectedItem.gridDimension.x; x++)
            {
                for (int y = startPos.y; y < startPos.y + selectedItem.gridDimension.y; y++)
                {
                    string tileKey = LevelCreateManager.GetTileKey(x, y);
                    enemySpawnTileKey.Add(tileKey);
                }
            }
            enemyPropertyDictionary.Add(LevelCreateManager.GetTileKey(startPos.x,startPos.y), enemyProperty);
        }
        

        public void DestroyEnemy(string parentKey)
        {
            enemyPropertyDictionary.TryGetValue(parentKey, out var enemyProperty);
            Vector2Int startPos = new(enemyProperty.position.x, enemyProperty.position.y);
            LevelEditorItem selectedItem = ItemManager.Singleton.GetItemDetails(enemyProperty.id, ItemCategory.Enemy);
            for (int x = startPos.x; x < startPos.x + selectedItem.gridDimension.x; x++)
            {
                for (int y = startPos.y; y < startPos.y + selectedItem.gridDimension.y; y++)
                {
                    string tileKey = LevelCreateManager.GetTileKey(x, y);
                    enemySpawnTileKey.Remove(tileKey);
                }
            }
            enemyPropertyDictionary.Remove(parentKey);
            if (enemyObjectDictionary.TryGetValue(parentKey,out var Obj))
            {
                Destroy(Obj);
                enemyObjectDictionary.Remove(parentKey);
            }
        }
        public LevelSave Save()
        {
            LevelSave levelSave = new()
            {
                EnemyDictionary = new()
            };
            levelSave.EnemyDictionary = enemyPropertyDictionary;
            return levelSave;
        }

        public void Load(LevelSave levelSave)
        {
            if(levelSave == null || levelSave.EnemyDictionary == null) { return; }
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

