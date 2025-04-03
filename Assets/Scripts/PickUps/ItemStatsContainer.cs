using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatsContainer : MonoBehaviour
{
    public ItemStats ItemStats;

   
    [Header("Names and Description")]
    public string Name;
    public string Description;
    [Header("Price and Satisfaction")]
    public float Price;
    public float Satisfaction;

    private void Start()
    {
        ItemStats = new ItemStats
        {
            Name = Name,
            Description = Description,
            Price = Price,
            Satisfaction = Satisfaction
        };
    }
}
