using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder
{
    public class OtherHandler : MonoBehaviour,ISaveLoad
    {
        public Tilemap spikeTileMap;
        int state;
        GameObject cursorObject;
        public Dictionary<string, StateTileProperty> spikeDictionary = new();
        public Dictionary<string, TileProperty> trampolinDictionary = new();
        public Dictionary<string, GameObject> trampolineObjects = new();
        public List<string> trampolineChilds = new();

        private string uniqueId = "Other";
        public string UniqueSaveID { get => uniqueId; set => uniqueId = value; }

        private void OnEnable()
        {
            if (SaveLoadManager.Singleton != null) { SaveLoadManager.Singleton.saveLoadList.Add(this); }
            LevelCreateManager.ItemSelectEvent += OnItemSelect;
            LevelCreateManager.OverUiEvent += OverUi;
        }
        private void Start()
        {
            if (!SaveLoadManager.Singleton.saveLoadList.Contains(this)) { SaveLoadManager.Singleton.saveLoadList.Add(this); }
        }
        void OverUi()
        {
            if (cursorObject != null)
            {
                cursorObject.transform.position = new(cursorObject.transform.position.x, cursorObject.transform.position.y, -20);
            }
        }
        private void OnItemSelect(LevelEditorItem item)
        {
            if(cursorObject != null) { Destroy(cursorObject); }
            if(item == null || item.validationType != ValidationType.Other) { return; }
            state = (int)item.extra.standingBlock;
            cursorObject = Instantiate(item.editorPrefab);
        }

        public bool ValidationCheckForOthers(LevelEditorItem selectedItem, Vector3Int gridPos)
        {
            if(selectedItem.id == 2)
            {
                return SpikeValidation(selectedItem,gridPos);
            }
            else if(selectedItem.id == 3)
            {
                return TrampolineValidation(selectedItem,gridPos);
            }
            return false;
        }

        private bool SpikeValidation(LevelEditorItem selectedItem, Vector3Int gridPos)
        {
            Vector2Int groundValidationPos = new();
            string tileKey = TileHandler.GetTileKey(gridPos.x, gridPos.y);
            if(Input.GetKeyDown(KeyCode.R))
            {
                state++;
                if(state > 3) state = 0;
                float rotationZ = 0;
                if (state == 0)
                {
                    rotationZ = 0;
                }
                else if (state == 1)
                {
                    rotationZ = -90;
                }
                else if (state == 2)
                {
                    rotationZ = -180;
                }
                else if (state == 3)
                {
                    rotationZ = 90;
                }
                cursorObject.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
            }

            if (state == 0)
            {
                groundValidationPos = new(gridPos.x, gridPos.y - 1);
            }
            else if (state == 1)
            {
                groundValidationPos = new(gridPos.x - 1, gridPos.y);
            }
            else if (state == 2)
            {
                groundValidationPos = new(gridPos.x, gridPos.y + 1);
            }
            else if (state == 3)
            {
                groundValidationPos = new(gridPos.x + 1, gridPos.y);
            }


            if (cursorObject != null)
            {
                cursorObject.transform.position = new(gridPos.x+selectedItem.objectOffset.x,gridPos.y+selectedItem.objectOffset.y);
            }
            if (spikeDictionary.ContainsKey(tileKey))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    spikeTileMap.SetTile(gridPos, null);
                    spikeDictionary.Remove(tileKey);
                }
                return false;
            }

            if (!LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKey) &&
                LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(TileHandler.GetTileKey(groundValidationPos.x, groundValidationPos.y))&&
                !spikeDictionary.ContainsKey(tileKey) &&
                !trampolineChilds.Contains(tileKey) &&
                !LevelCreateManager.Singleton.InteractableHandler.childDictionary.ContainsKey(tileKey))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StateTileProperty spikeTileProperty = new()
                    {
                        Id = selectedItem.id,
                        state = state,
                        position = new(gridPos.x, gridPos.y),
                    };
                    PlaceSpike(spikeTileProperty, selectedItem);
                }
                return true;
            }
            return false;
        }

        public void PlaceSpike(StateTileProperty spikeTileProperty,LevelEditorItem item)
        {
            spikeTileMap.SetTile(new(spikeTileProperty.position.x, spikeTileProperty.position.y, 0), item.tile);
            spikeDictionary.Add(TileHandler.GetTileKey(spikeTileProperty.position.x, spikeTileProperty.position.y), spikeTileProperty);
            float rotationZ = 0;
            if (spikeTileProperty.state == 0)
            {
                rotationZ = 0;
            }
            else if (spikeTileProperty.state == 1)
            {
                rotationZ = -90;
            }
            else if (spikeTileProperty.state == 2)
            {
                rotationZ = -180;
            }
            else if (spikeTileProperty.state == 3)
            {
                rotationZ = 90;
            }
            Quaternion rotation = Quaternion.Euler(0, 0, rotationZ);
            Matrix4x4 matrix = Matrix4x4.Rotate(rotation);
            spikeTileMap.SetTransformMatrix(new(spikeTileProperty.position.x, spikeTileProperty.position.y, 0), matrix);
        }

        bool TrampolineValidation(LevelEditorItem selectedItem, Vector3Int gridPos)
        {
            if(cursorObject != null)
            {
                cursorObject.transform.position = new(gridPos.x + selectedItem.objectOffset.x, gridPos.y + selectedItem.objectOffset.y);
            }
            GridCursor.Singleton.DisplayMultiGridCursor(gridPos, new(2, 1));
            string tileKey = TileHandler.GetTileKey(gridPos.x, gridPos.y);
            if(trampolinDictionary.ContainsKey(tileKey))
            {
                if(Input.GetMouseButtonDown(0))
                {
                    DestroyTrampoline(gridPos);
                }
                return false;
            }

            for (int i = gridPos.x; i < gridPos.x+2; i++)
            {
                string tileKeyi = TileHandler.GetTileKey(i, gridPos.y);
                if (LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(tileKeyi) ||
                LevelCreateManager.Singleton.InteractableHandler.childDictionary.ContainsKey(tileKeyi) ||
                spikeDictionary.ContainsKey(tileKeyi) || 
                trampolineChilds.Contains(tileKeyi) ||
                !LevelCreateManager.Singleton.FloorHandler.floorDictionary.ContainsKey(TileHandler.GetTileKey(i,gridPos.y-1)))
                {
                    return false;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                TileProperty tileProperty = new(selectedItem.id,gridPos.x,gridPos.y);
                PlaceTrampoline(tileProperty);
            }
            return true;
        }
        void PlaceTrampoline(TileProperty tileProperty)
        {
            LevelEditorItem item = ItemManager.Singleton.GetItemDetails(tileProperty.id, ItemCategory.Other);
            GameObject trampoline = Instantiate(item.editorPrefab, new(tileProperty.position.x + item.objectOffset.x, tileProperty.position.y + item.objectOffset.y), Quaternion.identity);
            trampolineObjects.Add(TileHandler.GetTileKey(tileProperty.position.x, tileProperty.position.y), trampoline);
            trampolinDictionary.Add(TileHandler.GetTileKey(tileProperty.position.x, tileProperty.position.y), tileProperty);
            for (int i = tileProperty.position.x; i < tileProperty.position.x + 2; i++)
            {
                string tileKeyi = TileHandler.GetTileKey(i, tileProperty.position.y);
                trampolineChilds.Add(tileKeyi);
            }
        }
        void DestroyTrampoline(Vector3Int gridPos)
        {
            string tileKey = TileHandler.GetTileKey(gridPos.x, gridPos.y);
            trampolinDictionary.Remove(tileKey);
            for (int i = gridPos.x; i < gridPos.x + 2; i++)
            {
                string tileKeyi = TileHandler.GetTileKey(i, gridPos.y);
                trampolineChilds.Remove(tileKeyi);
            }
            if (trampolineObjects.ContainsKey(tileKey))
            {
                Destroy(trampolineObjects[tileKey]);
                trampolineObjects.Remove(tileKey);
            }
        }
        public LevelSave Save()
        {
            LevelSave levelSave = new();
            OtherProperty otherProperty = new();

            otherProperty.spikeTileProperty = new();
            otherProperty.tramploingProperty = new();
            foreach (var item in spikeDictionary.Values)
            {
                otherProperty.spikeTileProperty.Add(item);
            }
            foreach (var item in trampolinDictionary.Values)
            {
                otherProperty.tramploingProperty.Add(item);
            }
            levelSave.OtherProperty = otherProperty;
            return levelSave;
        }

        public void Load(LevelSave mapSave)
        {
            foreach (var spike in mapSave.OtherProperty.spikeTileProperty)
            {
                LevelEditorItem item = ItemManager.Singleton.GetItemDetails(spike.Id, ItemCategory.Other);
                PlaceSpike(spike, item);
            }
            foreach (var trampoline in mapSave.OtherProperty.tramploingProperty)
            {
                PlaceTrampoline(trampoline);
            }
        }
    }
    [System.Serializable]
    public class StateTileProperty
    {
        public int Id;
        public int state;
        public Vector2IntSerializable position;
    }
    [System.Serializable]
    public class OtherProperty
    {
        public List<StateTileProperty> spikeTileProperty;
        public List<TileProperty> tramploingProperty;
    }
}