using System.Drawing.Drawing2D;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace LevelBuilder
{
    public class UiLevelDetails : MonoBehaviour
    {
        public Image levelImage;
        public TextMeshProUGUI levelName;
        public TextMeshProUGUI dateAccessed;
        public Button playButton;
        public Button editButton;
        public Button deleteButton;
        public Button uploadButton;
        private SaveData saveData;
        private LevelEditorMenuManager levelEditor;
        public void SetLevelInfo(SaveData saveData, LevelEditorMenuManager levelEditor)
        {
            this.saveData = saveData;
            this.levelEditor = levelEditor;
            levelName.text = saveData.saveInfo.saveDataFileName;
            dateAccessed.text = "Last Edit: "+saveData.saveInfo.dateAccessed;
            levelImage.sprite = SaveLoadManager.Singleton.GetThumbnailSprite(saveData.saveInfo.thumbnail);
            
            editButton.onClick.AddListener(() => {
                levelEditor.EditLevel(saveData);
            });

            playButton.onClick.AddListener(() =>
            {
                levelEditor.PlayLevel(saveData);
            });

            deleteButton.onClick.AddListener(() =>
            {
                levelEditor.DeleteSave(saveData);
                Destroy(gameObject);
            });
            uploadButton.onClick.AddListener(() =>
            {
                levelEditor.UploadToSteam(saveData);
            });
        }
    }
}

