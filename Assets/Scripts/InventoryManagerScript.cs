using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryManagerScript : MonoBehaviour
{
  public GameObject InventoryItemPrefab;
  public InventorySlotScript[] inventorySlots;
  public GameObject popupWindow;
  
  [SerializeField] public ItemScript[] items;

  public bool AddItemToEmptySlot(ItemScript item)
  {
    //Find empty slot
    for (int i = 0; i < inventorySlots.Length; i++)
    {
      InventorySlotScript slotScript = inventorySlots[i];
      InventoryItemScript itemScriptInSlot = slotScript.GetComponentInChildren<InventoryItemScript>();
      if (itemScriptInSlot == null)
      {
        //Debug.Log(item.name + " added in slotScript "+ slotScript.name);
        SpawnNewItem(item, slotScript,0);
        return true;
      }
    }
    return false;
  }

  private InventorySlotScript AddItemToCurrentSlot (ItemScript item, int slotNumber,int itemCount)
  {
    
    InventorySlotScript slotScript = inventorySlots[slotNumber-1]; // fix for array system
    InventoryItemScript itemScriptInSlot = slotScript.GetComponentInChildren<InventoryItemScript>();
    if (itemScriptInSlot == null)
    {
      //Debug.Log(item.name + " added in slotScript "+ slotScript.name);
      SpawnNewItem(item, slotScript, itemCount);
      return slotScript;
    }

    Debug.LogFormat("Slot is Occupied by {0}", itemScriptInSlot.item.name);
    return null;
  }

  void SpawnNewItem(ItemScript item, InventorySlotScript slotScript,int itemCount)
  {
    GameObject newItemGameObject = Instantiate(InventoryItemPrefab, slotScript.transform);
    InventoryItemScript inventoryItemScript = newItemGameObject.GetComponent<InventoryItemScript>();
    inventoryItemScript.InitializeItem(item, slotScript,itemCount);
  }

  private void OpenPopupWindow() //Check Inventory Items For Usage, get stats and translate id to opened popup window
  {
    foreach (var slot  in inventorySlots)
    {
      InventoryItemScript itemScriptInSlot = slot.GetComponentInChildren<InventoryItemScript>();
      if (itemScriptInSlot != null)
      {
        if(itemScriptInSlot.isClicked)
        {
          
          itemScriptInSlot.isClicked = false;
          
          popupWindow.GetComponent<PopupWindowScript>().OpenPopupWithStats(itemScriptInSlot);
          
          return;
        }
      }
    }
  }

  public void AddItemFromListByName(string itemName)
  {
    foreach (var item in items)
    {
      if (itemName.Equals(item.itemName))
      {
        AddItemToEmptySlot(item);
      }
    }
  }

  public bool AddItemFromListByName(string itemName, int slot)
  {
    foreach (var item in items)
    {
      if (itemName.Equals(item.itemName))
      {
         AddItemToCurrentSlot(item, slot, 0);
         return true;
      }
    }
    return false;
  }
  
  public bool AddItemFromListByName(int slotNumber, string itemName, int count )
  {
    foreach (var item in items)
    {
      if (itemName.Equals(item.itemName))
      { 
        AddItemToCurrentSlot(item, slotNumber,count);
        
        return true;
      }
    }
    return false;
  }

  public void AddRandomItem()
  {
    int itemIndex = Random.Range(0, items.Length);
    AddItemToEmptySlot(items[itemIndex]);
  }
  
  

  public InventoryItemScript getItemFromSlot(int slotNumber)
  {
    InventoryItemScript itemScriptInSlot = inventorySlots[slotNumber - 1].GetComponentInChildren<InventoryItemScript>();
    if (itemScriptInSlot != null)
    {
      return itemScriptInSlot;
    }
    Debug.LogFormat("No item found in slot {0}", slotNumber);
    return null;
  }

  public List<InventoryItemScript> GetItemsFromInventory()
  {
    List<InventoryItemScript> itemsList = new List<InventoryItemScript>();
    InventoryItemScript itemScriptInSlot;
    foreach (var slot in inventorySlots)
    {
      itemScriptInSlot = slot.GetComponentInChildren<InventoryItemScript>();
      
      if (itemScriptInSlot != null)
      {
       itemsList.Add(itemScriptInSlot); 
      }
    }
    return itemsList;
  }

  public void CleanInventory()
  {
    foreach (var slot in inventorySlots)
    {
      InventoryItemScript itemScriptInSlot = slot.GetComponentInChildren<InventoryItemScript>();
      if (itemScriptInSlot != null)
      {
        itemScriptInSlot.gameObject.SetActive(false);
        Destroy(itemScriptInSlot.gameObject);
      }
    }
  }
  
  
  private void  Update()
  {
    OpenPopupWindow();//check if item isClicked then open popUp
  }
}
