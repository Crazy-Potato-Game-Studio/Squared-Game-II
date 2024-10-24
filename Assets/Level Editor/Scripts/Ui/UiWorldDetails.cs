using System.Drawing.Drawing2D;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace LevelBuilder
{
    public class UiLevelDetails : MonoBehaviour,IPointerClickHandler
    {
        public TextMeshProUGUI levelName;
        public TextMeshProUGUI days;
        public LevelDetails levelDetails { get; private set; }
        [HideInInspector] public Sprite levelSprite;
        LevelEditorMenuManager levelEditorMenuManager;
        public void SetLevelDetails(LevelDetails levelDetails, LevelEditorMenuManager levelEditorMenuManager)
        {
            this.levelEditorMenuManager = levelEditorMenuManager;
            this.levelDetails = levelDetails;
            levelName.text = this.levelDetails.levelName;
            days.text = this.levelDetails.lastEditedDay.ToString() + " Days Ago";

            Debug.Log(levelDetails.screenShotData);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(levelDetails.screenShotData.data); // Automatically resizes the texture
            //Texture2D screenShotTexture = new(levelDetails.screenShotData.width, levelDetails.screenShotData.height, TextureFormat.RGBA32, false);
            //screenShotTexture.LoadRawTextureData(levelDetails.screenShotData.data);
            //screenShotTexture.Apply();
            texture.Apply();
            levelSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            levelEditorMenuManager.ShowLevelDetails(levelDetails,this);
        }
    }
}

