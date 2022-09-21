using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TowerModel", menuName = "TowerModelCreator", order = 1)]
public class TowerModel : ScriptableObject
{
    public int id;
    public string towerName;

    public Vector2 modelSize;

    public List<Sprite> modelSprites;
    public Sprite UISprite;

    public GameObject scriptHolder;
}
