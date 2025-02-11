using System;
using DefaultNamespace;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 Physically placed in inventory grid. Item prefabs used as item library. Consumable/ammo items count using from here.  
 **/

public class ItemContainerScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerClickHandler
{
   
    public Image image;
    public TMP_Text countText;
    
    [HideInInspector] public Transform parentAfterDrag;
    public int currentSlotNumber;
    public ItemScript item;
    public int currentCount;

    
    public void InitializeItem(ItemScript newItem, InventorySlotScript slotScript, int count)
    {
        item = newItem;
        image.sprite = newItem.sprite;
        if (count != 0)
        {
            currentCount = count;
        }
        else
        {
            currentCount = newItem.maxCount;
        }
        currentSlotNumber = slotScript.slotNumber;
        RefreshCount();
    }

    public void RefreshCount()
    {
        if (item.stackable)
        {
            countText.text = currentCount.ToString();
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //item sets as last sibling in grid when start dragging (to display image above slots)
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false; //disable visibility for mouse after start drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        currentSlotNumber = parentAfterDrag.gameObject.GetComponent<InventorySlotScript>().slotNumber;
        image.raycastTarget = true; //enable  visibility for mouse after start drag
    }

    public static Action<ItemContainerScript> OnUseItem;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnUseItem?.Invoke(this);
        //Debug.LogFormat("Item {0} ins slot {1} is Clicked",item.name, currentSlotNumber);
        
    }
    
}