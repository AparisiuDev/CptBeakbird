using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    // =========== ITEM DATA ================ //
    public string itemName;
    public string description;
    public Sprite model;
    public bool isFull;
    // =========== ITEM SLOT ================ //
    //[SerializeField] private TMP_Text itemNameText;
    //[SerializeField] private TMP_Text descriptionText;

    [SerializeField] Image itemModel;

    public void AddItem(string Description, Sprite Model)
    {
        this.description = Description;
        this.model = Model;
        isFull = true;

        itemModel.sprite = Model;
    }
}
