using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder
{
    public static class TileHandler
    {
        public static void ConnectFloorTile(FloorSO floorTileDetailsSo,Tilemap floorTileMap,Dictionary<string, FloorProperty> floorDictionary,int tileId, Vector3Int tileSetPos)
        {
            Tile tile;
            if (!floorDictionary.ContainsKey(GetTileKey(tileSetPos.x, tileSetPos.y)))
            {
                tile = null;
            }
            else
            {
                tile = floorTileDetailsSo.floorTileDetails[tileId].ruleTile ?
                    SelectFloorTile(floorTileDetailsSo,floorDictionary, tileSetPos, tileId) :
                    floorTileDetailsSo.floorTileDetails[tileId].tiles[0];
            }
            floorTileMap.SetTile(new Vector3Int(tileSetPos.x, tileSetPos.y, 0), tile);

            string rightTileKey = GetTileKey(tileSetPos.x + 1, tileSetPos.y);
            if (floorDictionary.TryGetValue(rightTileKey, out FloorProperty rightTileId))
            {
                Tile rightTile = SelectFloorTile(floorTileDetailsSo,floorDictionary, new Vector3Int(tileSetPos.x + 1, tileSetPos.y), rightTileId.id);
                floorTileMap.SetTile(new Vector3Int(tileSetPos.x + 1, tileSetPos.y, 0), rightTile);
            }
            string leftTileKey = GetTileKey(tileSetPos.x - 1, tileSetPos.y);
            if (floorDictionary.TryGetValue(leftTileKey, out FloorProperty leftTileId))
            {
                Tile leftTile = SelectFloorTile(floorTileDetailsSo, floorDictionary,new Vector3Int(tileSetPos.x - 1, tileSetPos.y), leftTileId.id);
                floorTileMap.SetTile(new Vector3Int(tileSetPos.x - 1, tileSetPos.y, 0), leftTile);
            }
            string upTileKey = GetTileKey(tileSetPos.x, tileSetPos.y + 1);
            if (floorDictionary.TryGetValue(upTileKey, out FloorProperty upTileId))
            {
                Tile upTile = SelectFloorTile(floorTileDetailsSo, floorDictionary, new Vector3Int(tileSetPos.x, tileSetPos.y + 1), upTileId.id);
                floorTileMap.SetTile(new Vector3Int(tileSetPos.x, tileSetPos.y + 1, 0), upTile);
            }
            string downTileKey = GetTileKey(tileSetPos.x, tileSetPos.y - 1);
            if (floorDictionary.TryGetValue(downTileKey, out FloorProperty downTileId))
            {
                Tile downTile = SelectFloorTile(floorTileDetailsSo, floorDictionary, new Vector3Int(tileSetPos.x, tileSetPos.y - 1), downTileId.id);
                floorTileMap.SetTile(new Vector3Int(tileSetPos.x, tileSetPos.y - 1, 0), downTile);
            }
        }
        private static Tile SelectFloorTile(FloorSO floorTileDetailsSo, Dictionary<string, FloorProperty> floorDictionary,Vector3Int gridPos, int ruleTileIndex)
        {
            if (!floorTileDetailsSo.floorTileDetails[ruleTileIndex].ruleTile) { return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[0]; }
            bool hasUpTile = floorDictionary.ContainsKey(GetTileKey(gridPos.x, gridPos.y + 1));
            bool hasDownTile = floorDictionary.ContainsKey(GetTileKey(gridPos.x, gridPos.y - 1));
            bool HasRightTile = floorDictionary.ContainsKey(GetTileKey(gridPos.x + 1, gridPos.y));
            bool hasLeftTile = floorDictionary.ContainsKey(GetTileKey(gridPos.x - 1, gridPos.y));

            if (!hasUpTile && hasDownTile && HasRightTile && !hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[0];
            }
            else if (!hasUpTile && hasDownTile && HasRightTile && hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[1];
            }
            else if (!hasUpTile && hasDownTile && !HasRightTile && hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[2];
            }
            else if (hasUpTile && hasDownTile && HasRightTile && !hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[3];
            }
            else if (hasUpTile && hasDownTile && HasRightTile && hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[4];
            }
            else if (hasUpTile && hasDownTile && !HasRightTile && hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[5];
            }
            else if (hasUpTile && !hasDownTile && HasRightTile && !hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[6];
            }
            else if (hasUpTile && !hasDownTile && HasRightTile && hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[7];
            }
            else if (hasUpTile && !hasDownTile && !HasRightTile && hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[8];
            }
            else if (!hasUpTile && !hasDownTile && !HasRightTile && !hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[9];
            }
            else if (!hasUpTile && hasDownTile && !HasRightTile && !hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[10];
            }
            else if (hasUpTile && hasDownTile && !HasRightTile && !hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[11];
            }
            else if (hasUpTile && !hasDownTile && !HasRightTile && !hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[12];
            }
            else if (!hasUpTile && !hasDownTile && HasRightTile && !hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[13];
            }
            else if (!hasUpTile && !hasDownTile && HasRightTile && hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[14];
            }
            else if (!hasUpTile && !hasDownTile && !HasRightTile && hasLeftTile)
            {
                return floorTileDetailsSo.floorTileDetails[ruleTileIndex].tiles[15];
            }
            return null;
        }
        public static void ConnectPlatformTile(int id, Vector3Int currentTilePos, Dictionary<string, FloorProperty> floorDictionary, Tilemap platformTileMap,PlatformSO platformDetails)
        {
            Tile tile = platformDetails.properties[id - 11].tiles[1];
            if (floorDictionary.ContainsKey(GetTileKey(currentTilePos.x - 1, currentTilePos.y)))
            {
                tile = platformDetails.properties[id - 11].tiles[0];
            }
            if (floorDictionary.ContainsKey(GetTileKey(currentTilePos.x + 1, currentTilePos.y)))
            {
                tile = platformDetails.properties[id - 11].tiles[2];
            }
            if (floorDictionary.ContainsKey(
                GetTileKey(currentTilePos.x - 1, currentTilePos.y)) &&
                floorDictionary.ContainsKey(
                GetTileKey(currentTilePos.x + 1, currentTilePos.y)))
            {
                tile = platformDetails.properties[id - 11].tiles[1];
            }
            if(platformTileMap == null) { return; }
            if(tile == null) {return; }
            platformTileMap.SetTile(currentTilePos, tile);
        }
        public static string GetTileKey(int x, int y) => "X" + x + "Y" + y;
    }
}