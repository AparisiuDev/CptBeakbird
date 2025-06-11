using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelLocker;

public class LevelUnlocker : MonoBehaviour { 
    void Update()
    {
        if (LevelLocker.VariablesGlobales._leaveTut == true) LevelLocker.VariablesGlobales._lvl1 = true;
        if (LevelLocker.VariablesGlobales._leave1 == true) LevelLocker.VariablesGlobales._lvl2 = true;
        if (LevelLocker.VariablesGlobales._leave2 == true) LevelLocker.VariablesGlobales._tapon = true;
    }
}
