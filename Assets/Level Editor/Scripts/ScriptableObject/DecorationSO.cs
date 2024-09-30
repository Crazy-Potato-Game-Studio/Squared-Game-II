using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelBuilder
{
    [CreateAssetMenu(fileName = "DecorationSO", menuName = "Scriptable Object/Level Editor/DecorationSO")]
    public class DecorationSO : ScriptableObject
    {
        public List<DecorationDetails> details;
        private void OnValidate()
        {
            for (int i = 0; i < details.Count; i++)
            {
                details[i].id = i;
            }
        }
    }
    [System.Serializable]
    public class DecorationDetails
    {
        public int id;
        public Tile[] tiles;
        public GameObject prefab;
    }
}