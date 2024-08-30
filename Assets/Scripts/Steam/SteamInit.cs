using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamInit : MonoBehaviour
{
    void Start()
    {
        try{
            SteamClient.Init(2960090);
            PrintSteamName();
        }catch(System.Exception e){
            Debug.Log(e);
        }
    }

    void Update()
    {
        
    }

    private void PrintSteamName(){
        Debug.Log(SteamClient.Name);
    }
}
