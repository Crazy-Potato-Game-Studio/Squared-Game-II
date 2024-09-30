using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder
{
    [CreateAssetMenu(fileName = "FloorSO", menuName = "Scriptable Object/Level Editor/FloorSO")]
    public class FloorSO : ScriptableObject
    {
        public List<FloorTileDetails> floorTileDetails;
    }
    [System.Serializable]
    public struct FloorTileDetails
    {
        public int id;
        public bool ruleTile;
        public Tile[] tiles;
    }
}