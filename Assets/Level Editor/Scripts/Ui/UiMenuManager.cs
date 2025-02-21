using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelBuilder
{
    public class UiMenuManager : MonoBehaviour
    {
        public string menuSceneName = "MainMenu";
        public GameObject levelEditorUi;
        
        public void Menu()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            levelEditorUi.SetActive(!gameObject.activeSelf);
        }
        public void MainMenu() => SceneManager.LoadScene(menuSceneName);

        public void Back()
        {
            levelEditorUi.SetActive(gameObject.activeSelf);
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}