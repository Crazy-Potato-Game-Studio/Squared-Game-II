using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemsCounter : MonoBehaviour
{
    public static int lastPlayedLevel = 0;

    public class Level{
        public int arrowsNumber;
        public int potionsNumber;

        public Level(int arrows, int potions){
            arrowsNumber = arrows;
            potionsNumber = potions;
        }
    }

    public Level[] levelArray;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        levelArray = new Level[SceneManager.sceneCountInBuildSettings]; 
    }

    public void SaveItems(int arrows, int potions){
        Level newLevel = new Level(arrows, potions);
        levelArray[SceneManager.GetActiveScene().buildIndex] = newLevel;

        SaveSystem.SaveProgress(this);
    }
}
