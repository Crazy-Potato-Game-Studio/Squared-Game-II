using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower
{
    private TowerModel model;
    private int level;

    public Tower(TowerModel mod, int lev) {
        model = mod;
        level = lev;
    }
}
