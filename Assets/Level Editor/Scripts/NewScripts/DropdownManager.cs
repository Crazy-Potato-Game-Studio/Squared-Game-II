using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownManager : MonoBehaviour
{
    [SerializeField] private GameObject[] dropdownElements;

    public void SetElements(){
        foreach (var element in dropdownElements)
        {
            element.GetComponent<DropdownElement>().SetPosition();
        }
    } 
}
