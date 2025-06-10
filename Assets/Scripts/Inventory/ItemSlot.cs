using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using Unity.VisualScripting;


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
    public VideoPlayer MODEL3D_PLAYER;
    public TMP_Text ItemDescriptionName;
    public TMP_Text ItemDescriptionDescription;

    public Image itemModel;
    public VideoClip itemModel3D;
    public GameObject selectedShader;
    public bool thisItemSelected;

    public RawImage rawImage;
    public RenderTexture renderTexture;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }
    public void AddItem(string Name, string Description, Sprite Model, VideoClip Model3D)
    {
        this.itemName = Name;
        this.description = Description;
        this.model = Model;
        itemModel3D = Model3D;
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
        rawImage.gameObject.SetActive(true);
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
        MODEL3D_PLAYER.clip = itemModel3D;
        rawImage.texture = renderTexture;
        ItemDescriptionName.text = itemName;
        ItemDescriptionDescription.text = description;
    }
    public void OnRightClick()
    {
        inventoryManager.DeselectAllSlots();
        rawImage.gameObject.SetActive(false);
        //MODEL3D_PLAYER.clip = null;
        //MODEL3D_PLAYER.targetTexture = null; // Opcional, si usas RenderTexture
        //rawImage = null;
        //renderTexture = null;
        ItemDescriptionName.text = null;
        ItemDescriptionDescription.text = null;
    }
}