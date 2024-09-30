using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "DetailsSO", menuName = "Scriptable Object/Level Editor/DetailsSO")]
public class DetailsSO : ScriptableObject
{
    public List<DetailsProperty> properties;
    private void OnValidate()
    {
        for (int i = 0; i < properties.Count; i++)
        {
            properties[i].id = i;
        }
    }
}
[System.Serializable]
public class DetailsProperty
{
    [HideInInspector] public string name;
    public int id;
    public Tile[] tiles;
    public List<Vector2Int> groundCheckDirection;
    public List<float> zRotation;
}