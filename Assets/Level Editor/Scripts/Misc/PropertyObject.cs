using NativeSerializableDictionary;
using System.Collections.Generic;
using UnityEngine;

namespace LevelBuilder
{
    public class PropertyObject : MonoBehaviour
    {
        public ObjectProperty property;
    }
    [System.Serializable]
    public class ObjectProperty
    {
        public int id;
        public ItemCategory category;
        public int state;
        public string key;
        public Vector2IntSerializable position;
        public Dictionary<string, string> valueStrings;
        public Dictionary<string, float> valueFloats;
    }
}