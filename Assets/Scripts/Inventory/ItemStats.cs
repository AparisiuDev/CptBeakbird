using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public struct ItemStats
{
    [Header("Names and Description")]
    public string Name;
    public string Description;
    [Header("Price and Satisfaction")]
    public float Price;
    public float Satisfaction;

    [Header("Size SMALL/MID/BIG")]
    public string size;

    [Header("Model")]
    public Sprite Model;
    public VideoClip Model3D;

}
