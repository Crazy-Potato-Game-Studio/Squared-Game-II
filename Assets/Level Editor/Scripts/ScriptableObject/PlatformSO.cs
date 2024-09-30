using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelBuilder
{
    [CreateAssetMenu(fileName = "PlatformSO", menuName = "Scriptable Object/Level Editor/PlatformSO")]
    public class PlatformSO : ScriptableObject
    {
        public List<PlatformDetails> properties;
    }
    [System.Serializable]
    public struct PlatformParentProperty
    {
        public int itemId;
        public List<PlatformChildProperty> childs;
        public string parentKey;
    }
}