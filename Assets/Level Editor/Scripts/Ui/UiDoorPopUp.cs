using System.Collections;
using TMPro;
using UnityEngine;

public class UiDoorPopUp : MonoBehaviour
{
    public TextMeshProUGUI text;
    public void ShowDoorName(string name)
    {
        gameObject.SetActive(true);
        text.text = name;
    }
}
