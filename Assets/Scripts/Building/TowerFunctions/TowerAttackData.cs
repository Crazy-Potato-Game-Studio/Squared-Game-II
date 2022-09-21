using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TowerData", menuName = "TowerDataCreator", order = 1)]
public class TowerAttackData: ScriptableObject
{
    public int id;

    public int range;
    public int spotRange;

    public int damage;
    public int rateOfFire;
}
