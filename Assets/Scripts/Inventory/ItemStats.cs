using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemStats
{
    [Header("Names and Description")]
    public string Name;
    public string Description;
    [Header("Price and Satisfaction")]
    public float Price;
    public float Satisfaction;
    [Header("Time to grab")]
    public float timeToGrab;
    [Header("Model")]
    public Sprite Model;

}
