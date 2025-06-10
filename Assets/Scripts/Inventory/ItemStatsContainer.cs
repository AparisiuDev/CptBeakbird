using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ItemStatsContainer : MonoBehaviour
{
    public ItemStats ItemStats;
    public Vector3 og_scale;
    public bool notGrabbable;

   
    [Header("Names and Description")]
    public string Name;
    public string Description;
    [Header("Price and Satisfaction")]
    public float Price;
    public float Satisfaction;
    [Header("Time to grab")]
    public float timeToGrab;
    [Header("Size SMALL/MID/BIG")]
    public string size;
    [Header("Model")]
    public Sprite Model;
    public VideoClip Model3D;


    private void Start()
    {
        og_scale = this.gameObject.transform.localScale;

        ItemStats = new ItemStats
        {
            Name = Name,
            Description = Description,
            Price = Price,
            Satisfaction = Satisfaction,
            size = size,
            Model = Model,
            Model3D = Model3D

        };

    }
}
