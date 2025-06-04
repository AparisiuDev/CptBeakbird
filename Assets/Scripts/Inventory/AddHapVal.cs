using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHapVal : MonoBehaviour
{
    public LevelGoals levelGoals;
    public void AddValues(float happiness, float value)
    {
        levelGoals.happiness += happiness/100;
        levelGoals.lootValue += value / 100;
    }
}
