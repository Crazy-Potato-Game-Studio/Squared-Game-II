using System.Collections;
using TMPro;
using UnityEngine;

public class UiDoorPopUp : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject canvasObject;
    public void ShowDoorName(string name)
    {
        canvasObject.SetActive(true);
        text.text = name;
    }
}
