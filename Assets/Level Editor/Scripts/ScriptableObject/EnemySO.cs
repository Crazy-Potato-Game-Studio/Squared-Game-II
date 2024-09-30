using System.Collections.Generic;
using UnityEngine;
namespace LevelBuilder
{
    [CreateAssetMenu(fileName = "Enemy SO", menuName = "Scriptable Object/Level Editor/Enemy SO")]
    public class EnemySO : ScriptableObject
    {
        public List<EnemyDetails> enemyDetails;

        private void OnValidate()
        {
            for (int i = 0; i < enemyDetails.Count; i++)
            {
                if(enemyDetails[i].min > enemyDetails[i].max) { enemyDetails[i].max = enemyDetails[i].min; }
                if(enemyDetails[i].max < enemyDetails[i].min) { enemyDetails[i].min = enemyDetails[i].max; }
            }
        }
    }

    [System.Serializable]
    public class EnemyDetails
    {
        public string name;
        [EnemyItemCodeDescription]
        public int id;
        public EnemyType enemyType;
        [Space]
        [Header("Item Drop")]
        [ItemCodeDescription]
        public int defaultDropItem1Id;
        [ItemCodeDescription]
        public int defaultDropItem2Id;
        [Header("Default Drop Quantity")]
        [Range(0, 10)] public int min;
        [Range(0, 10)] public int max;
        public bool fliped;
    }
}