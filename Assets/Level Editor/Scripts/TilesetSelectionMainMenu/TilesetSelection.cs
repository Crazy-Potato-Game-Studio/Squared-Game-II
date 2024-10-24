using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TilesetSelection : MonoBehaviour
{
    public int selectedTileset = 0;
    public Image[] tilesetBorderImages;
    [SerializeField] private Image tilesetImage;
    [SerializeField] private Sprite[] tilesetSprites;
    [SerializeField] private TextMeshProUGUI tilesetText;
    [SerializeField] private TextMeshProUGUI tilesetLongText;

    private void Awake() {
        ChangeSelectedTileset(selectedTileset);
        tilesetBorderImages[selectedTileset].GetComponent<TilesetImage>().selected = true;
    }

    public void IncrementOrDecrementSelectedTilesetIndex(int number){
        if(number == 1){
            if(selectedTileset == 3){
                ChangeSelectedTileset(0);
            }else{
                ChangeSelectedTileset(selectedTileset + 1);
            }
        }else if(number == -1){
            if(selectedTileset == 0){
                ChangeSelectedTileset(3);
            }else{
                ChangeSelectedTileset(selectedTileset - 1);
            }
        }
    }

    public void ChangeSelectedTileset(int number){
        tilesetBorderImages[selectedTileset].color = new Color(0.25f, 0.25f, 0.25f);
        tilesetBorderImages[selectedTileset].gameObject.GetComponent<TilesetImage>().selected = false;
        selectedTileset = number;
        tilesetBorderImages[selectedTileset].gameObject.GetComponent<TilesetImage>().selected = true;
        AddGreenBorder();
        switch (selectedTileset)
        {
            case 0:
                tilesetImage.sprite = tilesetSprites[0];
                tilesetText.text = "Grasslands";
                tilesetLongText.text = "A green and peaceful place, charming, full of vegetation and harmless slimes. Great location for starting your adventure.";
                break;
            case 1:
                tilesetImage.sprite  = tilesetSprites[1];
                tilesetText.text = "Tunnels";
                tilesetLongText.text = "Dark and narrow place, abandoned by an advanced civilisation, full of bats, lava and tricky puzzles. Let the electricity light your way.";
                break;
            case 2:
                tilesetImage.sprite  = tilesetSprites[2];
                tilesetText.text = "Frozen World";
                tilesetLongText.text = "A frozen and cold biome, inhabited by creatures as cold and vile as ice. Defended by a slime queen with an army of snowman and deadly lasers.";
                break;
            case 3:
                tilesetImage.sprite  = tilesetSprites[3];
                tilesetText.text = "Ancient Temple";
                tilesetLongText.text = "The creatures living here may not pose much of a threat, but be careful and avoid traps! The puzzles here can be quite a challenge.";
                break;
            default:
                tilesetImage.sprite  = tilesetSprites[0];
                tilesetText.text = "Grasslands";
                tilesetLongText.text = "Grasslands";
                break;
        }
    }

    private void AddGreenBorder(){
        tilesetBorderImages[selectedTileset].color = new Color(0.6f , 1, 0.6f);
    }

    private void RemoveGreenBorder(){
        tilesetBorderImages[selectedTileset].color = new Color(0.25f, 0.25f, 0.25f);
    }
}
