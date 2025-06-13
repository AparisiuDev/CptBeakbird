using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelLocker;

public class LevelUnlocker : MonoBehaviour {
    public GameObject player;
    public GameObject spawn1;
    public GameObject spawn2;
    public GameObject spawn3;
    void Start()
    {
        if (LevelLocker.VariablesGlobales._leaveTut == true) { 
            LevelLocker.VariablesGlobales._lvl1 = true;
            player.transform.position = spawn1.transform.position;
        }
        if (LevelLocker.VariablesGlobales._leave1 == true) { 
            LevelLocker.VariablesGlobales._lvl2 = true;
            player.transform.position = spawn2.transform.position;
        }
        if (LevelLocker.VariablesGlobales._leave2 == true) { 
            LevelLocker.VariablesGlobales._tapon = true; 
            player.transform.position = spawn3.transform.position;
        }
    }
}
