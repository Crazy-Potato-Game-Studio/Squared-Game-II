using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StructuresSelector : MonoBehaviour
{
    public bool isPlayerBuilding;


    [SerializeField]
    private List<TowerModel> selectedTowersForBattle;

    [SerializeField]
    private TowerModel nullTower;

    public List<Tower> listOfPossibleTowers;
    public Transform StructuresMenuUI;


    private void InitializeTowers() {
        int size = selectedTowersForBattle.Count;
        isPlayerBuilding = false;


        loadSprites(size);
    }

    private void loadSprites(int s) {
        int index = 0;

        foreach (var model in selectedTowersForBattle)
        {
            Image towerImage = StructuresMenuUI.GetChild(index).GetChild(0).GetComponent<Image>();
            towerImage.sprite = model.UISprite;

            index++;
        }

        while (index < s)
        {
            Image towerImage = StructuresMenuUI.GetChild(index).GetChild(0).GetComponent<Image>();
            towerImage.sprite = nullTower.UISprite;

            index++;
        }
    }



    private void Awake()
    {
        InitializeTowers();
    }


    public void handleUIClick(GameObject UITile) {
        Debug.Log(UITile.name);
    }

}
