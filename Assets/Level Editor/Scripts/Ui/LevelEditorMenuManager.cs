using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace LevelBuilder
{
    public class LevelEditorMenuManager : MonoBehaviour
    {
        public GameObject levelEditMenuUi;
        public GameObject menuUi;
        [Header("Saved Level")]
        public UiLevelDetails savedLevelDetailsUiprefab;
        public Transform savedLevelsDetailsUiParent;
        public Button backToMenuButton;
        [Header("Create Level")]
        public TMP_InputField levelNameField;
        public Image levelNameBorderImage;
        public Color invalidLevelNameColor;
        public Color validLevelNameColor;
        public Button createButton;
        List<SaveData> saveDatas;
        public string levelEditorSceneName = "Scene0_CreateScene";
        public string levelPlaySceneName = "Scene1_Playable";
        private void OnEnable()
        {
            backToMenuButton.onClick.AddListener(() => {
                levelEditMenuUi.SetActive(false);
                menuUi.gameObject.SetActive(true);
            });
            levelNameField.onValueChanged.AddListener(LevelNameValidation);
            createButton.onClick.AddListener(CreateButtonPress);
        }
        private void Start()
        {
            PopulateSaveLevel();
            LevelNameValidation(levelNameField.text);
        }
        private void PopulateSaveLevel()
        {
            saveDatas = SaveLoadManager.Singleton.GetSavesInfo();
            foreach (SaveData saveData in saveDatas)
            {
                UiLevelDetails saveLevelUi = Instantiate(savedLevelDetailsUiprefab, savedLevelsDetailsUiParent);
                saveLevelUi.SetLevelInfo(saveData, this);
            }
        }
        public void LevelNameValidation(string levelName)
        {
            SaveData saveData = saveDatas.Find(x => x.saveInfo.saveDataFileName == levelName);
            if (saveData != null || LevelNameIsInvalid(levelName))
            {
                levelNameBorderImage.color = invalidLevelNameColor;
                createButton.interactable = false;
            }
            else
            {
                levelNameBorderImage.color = validLevelNameColor;
                createButton.interactable = true;
            }
        }
        public void CreateButtonPress()
        {
            SaveLoadManager.Singleton.SetSaveToLoad(levelNameField.text.ToString());
            SceneManager.LoadScene(levelEditorSceneName);
        }

        public void EditLevel(SaveData saveData)
        {
            SaveLoadManager.Singleton.SetSaveToLoad(saveData.saveInfo.saveDataFileName);
            SceneManager.LoadScene(levelEditorSceneName);
        }
        public void PlayLevel(SaveData saveData)
        {
            PlayableLevelData playableLevel = PlayableMaker.Singleton.MakePlayable(saveData);
            if (playableLevel == null)
            {
                Debug.Log("Not Playable!");
            }
            else
            {
                PlayableMaker.Singleton.playableToBuild = playableLevel;
                SceneManager.LoadScene(levelPlaySceneName);
            }
        }
        public void DeleteSave(SaveData saveData)
        {
            SaveLoadManager.Singleton.DeleteSave(saveData.saveInfo.saveDataFileName);
            saveDatas.Remove(saveData);
            LevelNameValidation(levelNameField.text);
        }
        public void UploadToSteam(SaveData saveData)
        {
            PlayableLevelData playableLevel = PlayableMaker.Singleton.MakePlayable(saveData);
            if (playableLevel == null)
            {
                Debug.Log("Not Playable!");
            }
            Debug.Log("Steam Upload Code Missing");
            //steam uploadCode here
        }
        public bool LevelNameIsInvalid(string levelName)
        {
            return levelName == null ||
                    levelName == "" ||
                    levelName.Contains("/") ||
                    levelName.Contains(".");
            
        }
    }
}
   



        //private void Awake()
        //{
        //    FileValidation();
        //    levelNameField.onValueChanged.AddListener(LevelNameValidation);
        //    createButton.onClick.AddListener(CreateButtonPress);
        //    createButton.onClick.AddListener(() => { levelEditMenuUi.SetActive(false); });
        //}
        //private void Start()
        //{
        //    LoadLevelsFromFile();
        //    ResetUi();
        //}
        //public void LoadLevelsFromFile()
        //{
        //    foreach (Transform child in savedLevelsDetailsUiParent)
        //    {
        //        Destroy(child.gameObject);
        //    }
        //    if (File.Exists(Application.persistentDataPath + "/Level Editor/SavesInfo.dat"))
        //    {
        //        BinaryFormatter bf = new();
        //        saveInfos = new();
        //        FileStream file = File.Open(Application.persistentDataPath + "/Level Editor/Levels.dat", FileMode.Open);
        //        savedLevels = (List<LevelDetails>)bf.Deserialize(file);
        //        file.Close();
        //    }
        //}


        //public void PopulateLevelsUI()
        //{
        //   foreach (var level in savedLevels)
        //   {
        //        UiLevelDetails levelDetails = Instantiate(savedLevelDetailsUiprefab.gameObject, savedLevelsDetailsUiParent).GetComponent<UiLevelDetails>();
        //        levelDetails.SetLevelDetails(level,this);
        //   }
        //}

        //private void FileValidation()
        //{
        //    bool levelEditorFolderExists = Directory.Exists(Application.persistentDataPath+"/Level Editor");
        //    if (!levelEditorFolderExists) { Directory.CreateDirectory(Application.persistentDataPath+"/Level Editor"); }
        //    bool levelsFolderExists = Directory.Exists(Application.persistentDataPath + "/Level Editor/Levels");
        //    if (!levelsFolderExists) { Directory.CreateDirectory(Application.persistentDataPath + "/Level Editor/Levels"); }
        //}
        //public void CreateButtonPress()
        //{
        //    CreateNewLevel();
        //    ResetUi();
        //    PopulateLevelsUI();
        //}
        //public void NewLevelCreate()
        //{
        //    levelNameField.text = "Level " + (savedLevels.Count+1).ToString("00");
        //    LevelNameValidation(levelNameField.text);
        //    levelCreateUi.SetActive(true);
        //    levelDetailsUi.SetActive(false);
        //}
        //public void LevelNameValidation(string levelName)
        //{
        //    LevelDetails levelDetails = savedLevels.Find(x => x.levelName == levelName);
        //    levelNameBorderImage.color = new Color(0.25f, 0.25f, 0.25f);
        //    createButton.interactable = true;
        //    if (levelDetails != null || levelName == "" || !NameIsCorrect(levelName))
        //    {
        //        levelNameBorderImage.color = invalidLevelNameColor;
        //        createButton.interactable = false;
        //    }

        //}
        //public bool NameIsCorrect(string levelName){
        //    if(levelName.Contains("<") || levelName.Contains(">") || levelName.Contains(":") || levelName.Contains("\"") || levelName.Contains("/") || levelName.Contains("\\")
        //    || levelName.Contains("|") || levelName.Contains("?") || levelName.Contains("*") || levelName.Contains(".") || levelName.Contains(",") || levelName.Contains("COM") || levelName.Contains("LPT")){
        //        return false;
        //    }else{
        //        if(levelName == "CON" || levelName == "PRN" || levelName == "AUX" || levelName == "NUL"){
        //            return false;
        //        }else{
        //            return true;
        //        }
        //    }
        //}
        //public void CreateNewLevel()
        //{
        //    string levelName = levelNameField.text;

        //    LevelDetails levelDetails = new()
        //    {
        //        levelName = levelName,
        //        levelDataFileName = levelName,
        //        lastEditedDay = 0
        //    };
        //    savedLevels.Add(levelDetails);
        //    SaveDataToFile();

        //    levelNameField.text = "Level " + (savedLevels.Count+1).ToString("00");
        //    LevelNameValidation(levelNameField.text);

        //}
        //public void SaveDataToFile()
        //{
        //    BinaryFormatter bf = new();
        //    FileStream file = File.Open(Application.persistentDataPath + "/Level Editor/Levels.dat", FileMode.Create);
        //    bf.Serialize(file, savedLevels);
        //    file.Close();
        //}



        //public void ShowLevelDetails(LevelDetails levelDetails, UiLevelDetails uiLevelDetails)
        //{
        //    selectedLevelDetails = uiLevelDetails;
        //    //levelCreateUi.SetActive(false);
        //    //levelDetailsUi.SetActive(true);
        //    this.levelDetails = levelDetails;
        //    levelName.text = this.levelDetails.levelName;
        //    levelImage.sprite = uiLevelDetails.levelSprite;
        //}
        //public void Play()
        //{
        //    PlayerPrefs.SetString("CURRENT_LEVEL", this.levelDetails.levelName);
        //    SceneManager.LoadScene("Scene1_Playable");
        //}
        //public void Edit()
        //{
        //    for (int i = 0; i < savedLevels.Count; i++)
        //    {
        //        if (savedLevels[i] == this.levelDetails)
        //        {
        //            savedLevels[i].lastEditedDay = DateTime.Now.Day;
        //        }
        //    }
        //    PlayerPrefs.SetString("CURRENT_LEVEL", this.levelDetails.levelName);
        //    SceneManager.LoadScene("Scene0_CreateScene");
        //}
        //public void Upload()
        //{
        //    Debug.Log("Upload to Steam workshop");
        //}

        //public void Delete()
        //{
        //    if (File.Exists(Application.persistentDataPath + "/Level Editor/Levels/"+selectedLevelDetails.levelDetails.levelName+ ".dat"))
        //    {
        //        File.Delete(Application.persistentDataPath + "/Level Editor/Levels/" + selectedLevelDetails.levelDetails.levelName + ".dat");
        //    }
        //    if (PlayerPrefs.GetString("CURRENT_LEVEL") == selectedLevelDetails.levelDetails.levelName) { PlayerPrefs.DeleteKey("CURRENT_LEVEL"); }
        //    savedLevels.Remove(selectedLevelDetails.levelDetails);
        //    NewLevelCreate();
        //    Destroy(selectedLevelDetails.gameObject);
        //    SaveDataToFile();

        //    Debug.Log(PlayerPrefs.GetString("CURRENT_LEVEL"));
        //}