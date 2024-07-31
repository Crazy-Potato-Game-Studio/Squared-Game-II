using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int lastLevelNumber;
    public int lastLevelArrows;
    public int lastLevelPotions;

    public GameData(ItemsCounter itemsCounter){
        lastLevelNumber = ItemsCounter.lastPlayedLevel;
        lastLevelArrows = itemsCounter.levelArray[lastLevelNumber].arrowsNumber;
        lastLevelPotions = itemsCounter.levelArray[lastLevelNumber].potionsNumber;
    }
}
