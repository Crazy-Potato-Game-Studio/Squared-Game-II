using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace LevelBuilder
{
    public class LevelEditorMenuManager : MonoBehaviour
    {
        public UiLevelDetails savedLevelDetailsUiprefab;
        public Transform savedLevelsDetailsUiParent;
        [Header("Top Bar")]
        public Image quickLoadBorderImage;
        public Button quickLoadButton;
        [Header("LevelCreate")]
        public GameObject levelCreateUi;
        public TMP_InputField worldNameField;
        public Image validationImage;
        public Color errorColor;
        public Button createButton;
        public Button playButton;
        public Button deleteButton;

        [Header("Level Details")]
        public GameObject levelDetailsUi;
        public LevelDetails levelDetails;
        public TextMeshProUGUI levelName;
        public Image levelImage;
        public List<LevelDetails> savedLevels;
        UiLevelDetails selectedLevelDetails;
        private void Awake()
        {
            FileValidation();
            worldNameField.onValueChanged.AddListener(LevelNameValidation);
            createButton.onClick.AddListener(CreateButtonPress);
        }
        private void Start()
        {
            LoadDataFromFile();
            DeleteLevelsUI();
            worldNameField.text = "Level "+ (savedLevels.Count + 1).ToString("00");
            LevelNameValidation(worldNameField.text);
            if (!PlayerPrefs.HasKey("CURRENT_LEVEL")) 
            {
                quickLoadButton.interactable = false;
                quickLoadBorderImage.enabled = false; 
            }
        }
        public void DeleteLevelsUI(){
            foreach (Transform child in savedLevelsDetailsUiParent)
            {
                Destroy(child.gameObject);
            }
        }

        public void PopulateLevelsUI()
        {
           foreach (var level in savedLevels)
           {
                UiLevelDetails levelDetails = Instantiate(savedLevelDetailsUiprefab.gameObject, savedLevelsDetailsUiParent).GetComponent<UiLevelDetails>();
                levelDetails.SetLevelDetails(level,this);
           }
        }

        private void FileValidation()
        {
            bool levelEditorFolderExists = System.IO.Directory.Exists(Application.persistentDataPath+"/Level Editor");
            if (!levelEditorFolderExists) { System.IO.Directory.CreateDirectory(Application.persistentDataPath+"/Level Editor"); }
            bool levelsFolderExists = System.IO.Directory.Exists(Application.persistentDataPath + "/Level Editor/Levels");
            if (!levelsFolderExists) { System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Level Editor/Levels"); }
        }
        public void CreateButtonPress()
        {
            CreateNewLevel();
            DeleteLevelsUI();
            PopulateLevelsUI();
        }
        public void NewLevelCreate()
        {
            worldNameField.text = "Level " + (savedLevels.Count+1).ToString("00");
            LevelNameValidation(worldNameField.text);
            levelCreateUi.SetActive(true);
            levelDetailsUi.SetActive(false);
        }
        public void LevelNameValidation(string levelName)
        {
            LevelDetails levelDetails = savedLevels.Find(x => x.levelName == levelName);
            validationImage.color = new Color(0.25f, 0.25f, 0.25f);
            createButton.interactable = true;
            if (levelDetails != null || levelName == "" || !NameIsCorrect(levelName))
            {
                validationImage.color = errorColor;
                createButton.interactable = false;
            }

        }
        public bool NameIsCorrect(string levelName){
            if(levelName.Contains("<") || levelName.Contains(">") || levelName.Contains(":") || levelName.Contains("\"") || levelName.Contains("/") || levelName.Contains("\\")
            || levelName.Contains("|") || levelName.Contains("?") || levelName.Contains("*") || levelName.Contains(".") || levelName.Contains(",") || levelName.Contains("COM") || levelName.Contains("LPT")){
                return false;
            }else{
                if(levelName == "CON" || levelName == "PRN" || levelName == "AUX" || levelName == "NUL"){
                    return false;
                }else{
                    return true;
                }
            }
        }
        public void CreateNewLevel()
        {
            string levelName = worldNameField.text;

            LevelDetails levelDetails = new()
            {
                levelName = levelName,
                levelDataFileName = levelName,
                lastEditedDay = 0
            };
            savedLevels.Add(levelDetails);
            SaveDataToFile();
            
            worldNameField.text = "Level " + (savedLevels.Count+1).ToString("00");
            LevelNameValidation(worldNameField.text);

        }
        public void SaveDataToFile()
        {
            BinaryFormatter bf = new();
            FileStream file = File.Open(Application.persistentDataPath + "/Level Editor/Levels.dat", FileMode.Create);
            bf.Serialize(file, savedLevels);
            file.Close();
        }

        public void LoadDataFromFile()
        {
            if (File.Exists(Application.persistentDataPath + "/Level Editor/Levels.dat"))
            {
                BinaryFormatter bf = new();
                savedLevels = new();
                FileStream file = File.Open(Application.persistentDataPath + "/Level Editor/Levels.dat", FileMode.Open);
                savedLevels = (List<LevelDetails>)bf.Deserialize(file);
                file.Close();
            }
        }

        public void ShowLevelDetails(LevelDetails levelDetails, UiLevelDetails uiLevelDetails)
        {
            selectedLevelDetails = uiLevelDetails;
            levelCreateUi.SetActive(false);
            levelDetailsUi.SetActive(true);
            this.levelDetails = levelDetails;
            levelName.text = this.levelDetails.levelName;
            levelImage.sprite = uiLevelDetails.levelSprite;
        }

        public void QuickLoad()
        {
            if (PlayerPrefs.HasKey("CURRENT_LEVEL")) { SceneManager.LoadScene("Scene0_CreateScene"); }
        }
        public void Play()
        {
            PlayerPrefs.SetString("CURRENT_LEVEL", this.levelDetails.levelName);
            SceneManager.LoadScene("Scene1_Playable");
        }
        public void Edit()
        {
            for (int i = 0; i < savedLevels.Count; i++)
            {
                if (savedLevels[i] == this.levelDetails)
                {
                    savedLevels[i].lastEditedDay = DateTime.Now.Day;
                }
            }
            PlayerPrefs.SetString("CURRENT_LEVEL", this.levelDetails.levelName);
            SceneManager.LoadScene("Scene0_CreateScene");
        }
        public void Upload()
        {
            Debug.Log("Upload to Steam workshop");
        }

        public void Delete()
        {
            if (File.Exists(Application.persistentDataPath + "/Level Editor/Levels/"+selectedLevelDetails.levelDetails.levelName+ ".dat"))
            {
                File.Delete(Application.persistentDataPath + "/Level Editor/Levels/" + selectedLevelDetails.levelDetails.levelName + ".dat");
            }
            if (PlayerPrefs.GetString("CURRENT_LEVEL") == selectedLevelDetails.levelDetails.levelName) { PlayerPrefs.DeleteKey("CURRENT_LEVEL"); }
            if (!PlayerPrefs.HasKey("CURRENT_LEVEL"))
            {
                quickLoadButton.interactable = false;
                quickLoadBorderImage.enabled = false;
            }
            savedLevels.Remove(selectedLevelDetails.levelDetails);
            NewLevelCreate();
            Destroy(selectedLevelDetails.gameObject);
            SaveDataToFile();

            Debug.Log(PlayerPrefs.GetString("CURRENT_LEVEL"));
        }
    }
    [System.Serializable]
    public class LevelDetails
    {
        public string levelName;
        public int lastEditedDay;
        public string levelDataFileName;
        public ScreenShotData screenShotData;
    }
}
   

