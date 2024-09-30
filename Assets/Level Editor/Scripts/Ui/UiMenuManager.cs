using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelBuilder
{
    public class UiMenuManager : MonoBehaviour
    {
        public string menuSceneName = "MainMenu";
        public string playableSceneName = "Scene1_Playable";
        public GameObject levelEditorUi;
        
        public void Menu()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            levelEditorUi.SetActive(!gameObject.activeSelf);
        }
        public void Play()
        {
            SaveLoadManager.Singleton.SaveDataToFile();
            PlayableMaker.MakePlayable();
            SceneManager.LoadScene(playableSceneName);
        }

        public void Save()
        {
            SaveLoadManager.Singleton.SaveDataToFile();
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(menuSceneName);
        }

        public void Back()
        {
            levelEditorUi.SetActive(gameObject.activeSelf);
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}