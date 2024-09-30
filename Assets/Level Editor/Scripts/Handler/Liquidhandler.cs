using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder
{
    public class Liquidhandler : MonoBehaviour
    {
        public Tilemap liquidTileMap;
        public LiquidTileDetails[] liquidTileDetails;
        public Dictionary<string,LiquidTileProperty> liquidTilesPropertyDictionary;

        public bool ValidationCheckForPlatform(LevelEditorItem selectedItem, Vector3Int gridPos)
        {
            if (LevelCreateManager.Singleton.FloorHandler.floorDictionary.TryGetValue(LevelCreateManager.GetTileKey(gridPos),out FloorProperty floorProperty) &&
                !LevelCreateManager.Singleton.ClimbableHandler.climbableDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x, gridPos.y)) &&
                !LevelCreateManager.Singleton.PlatformHandler.childDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x, gridPos.y)) &&
                !LevelCreateManager.Singleton.DecorationHandler.decorationChildDictionary.ContainsKey(LevelCreateManager.GetTileKey(gridPos.x, gridPos.y)))
            {
                if (liquidTilesPropertyDictionary.TryGetValue(LevelCreateManager.GetTileKey(gridPos), out LiquidTileProperty property))
                {
                    DestroyLiquid(property);
                    return false;
                }
                if(Input.GetMouseButtonDown(0))
                {
                    LiquidTileProperty newTileProperty = new()
                    {
                        id = selectedItem.id,
                        floorId = floorProperty.id,
                        gridpos = new(gridPos.x,gridPos.y)
                    };
                    PlaceLiquidTile(newTileProperty);
                }
                return true;
            }
            return false;
        }

        void DestroyLiquid(LiquidTileProperty liquidProperty)
        {

        }

        void PlaceLiquidTile(LiquidTileProperty liquidProperty)
        {

        }
    }
    [System.Serializable]
    public struct LiquidTileProperty
    {
        public int id;
        public int floorId;
        public Vector2IntSerializable gridpos;
    }
    [System.Serializable]
    public struct LiquidTileDetails
    {
        public int id;
        public Tile[] tiles;
    }
}

