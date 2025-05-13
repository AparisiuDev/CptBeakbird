using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // =========== ITEM DATA ================ //
    public string itemName;
    public string description;
    public Sprite model;
    public bool isFull;
    // =========== ITEM SLOT ================ //
    //[SerializeField] private TMP_Text itemNameText;
    //[SerializeField] private TMP_Text descriptionText;

    // =========== ITEM SLOT DESCRIPTION ================ //
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionName;
    public TMP_Text ItemDescriptionDescription;

    [SerializeField] Image itemModel;
    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }
    public void AddItem(string Name, string Description, Sprite Model)
    {
        this.itemName = Name;
        this.description = Description;
        this.model = Model;
        isFull = true;

        itemModel.sprite = Model;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
        itemDescriptionImage.sprite = model;
        ItemDescriptionName.text = itemName;
        ItemDescriptionDescription.text = description;
    }
    public void OnRightClick()
    {
        inventoryManager.DeselectAllSlots();
        itemDescriptionImage.sprite = null;
        ItemDescriptionName.text = null;
        ItemDescriptionDescription.text = null;
    }
}