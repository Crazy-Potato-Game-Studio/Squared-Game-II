using LevelBuilder;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCapturer : MonoBehaviour
{
    public GameObject[] uiToHireDuringScreenShot;
    public void SaveToLevelDetails()
    {
        foreach (var ui in uiToHireDuringScreenShot)
        {
            ui.SetActive(false);
        }
        StartCoroutine(TakeScreenshotAndShow());
    }
    private IEnumerator TakeScreenshotAndShow()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        Texture2D newScreenshot = new Texture2D(screenshot.width, screenshot.height, TextureFormat.RGB24, false);
        newScreenshot.SetPixels(screenshot.GetPixels());
        newScreenshot.Apply();
        Destroy(screenshot);
        SaveData(newScreenshot);
        EnableUi();
    }
    void SaveData(Texture2D newScreenshot)
    {
        byte[] bytes = newScreenshot.EncodeToPNG();
        ScreenShotData screenShotData = new()
        {
            height = newScreenshot.height,
            width = newScreenshot.width,
            data = bytes
        };

        if (File.Exists(Application.persistentDataPath + "/Level Editor/Levels.dat"))
        {
            BinaryFormatter lbf = new();
            FileStream lfile = File.Open(Application.persistentDataPath + "/Level Editor/Levels.dat", FileMode.Open);
            List<LevelDetails> savedLevels = (List<LevelDetails>)lbf.Deserialize(lfile);
            lfile.Close();

            foreach (LevelDetails level in savedLevels)
            {
                if (level.levelDataFileName == PlayerPrefs.GetString("CURRENT_LEVEL"))
                {
                    level.screenShotData = screenShotData;
                }
            }

            BinaryFormatter nbf = new();
            FileStream nfile = File.Open(Application.persistentDataPath + "/Level Editor/Levels.dat", FileMode.Create);
            nbf.Serialize(nfile, savedLevels);
            nfile.Close();
        }
    }
    void EnableUi()
    {
        foreach (var ui in uiToHireDuringScreenShot)
        {
            ui.SetActive(true);
        }
    }
}
[System.Serializable]
public class ScreenShotData
{
    public int height; public int width; public byte[] data;
}