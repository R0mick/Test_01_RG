using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class InventoryManagerScript : MonoBehaviour
{
  public GameObject ItemContainerPrefab;
  public InventorySlotScript[] inventorySlots;
  public GameObject popupWindow;
  
  [SerializeField] public ItemScript[] items;

  public bool AddItemToEmptySlot(ItemScript item)
  {
    //Find empty slot
    for (int i = 0; i < inventorySlots.Length; i++)
    {
      InventorySlotScript slotScript = inventorySlots[i];
      ItemContainerScript itemContainerScriptInSlot = slotScript.GetComponentInChildren<ItemContainerScript>();
      if (itemContainerScriptInSlot == null)
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
    ItemContainerScript itemContainerScriptInSlot = slotScript.GetComponentInChildren<ItemContainerScript>();
    if (itemContainerScriptInSlot == null)
    {
      //Debug.Log(item.name + " added in slotScript "+ slotScript.name);
      SpawnNewItem(item, slotScript, itemCount);
      return slotScript;
    }

    Debug.LogFormat("Slot is Occupied by {0}", itemContainerScriptInSlot.item.name);
    return null;
  }

  void SpawnNewItem(ItemScript item, InventorySlotScript slotScript,int itemCount)
  {
    GameObject newItemGameObject = Instantiate(ItemContainerPrefab, slotScript.transform);
    ItemContainerScript itemContainerScript = newItemGameObject.GetComponent<ItemContainerScript>();
    itemContainerScript.InitializeItem(item, slotScript,itemCount);
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
  
  
  public ItemContainerScript getItemFromSlot(int slotNumber)
  {
    ItemContainerScript itemContainerScriptInSlot = inventorySlots[slotNumber - 1].GetComponentInChildren<ItemContainerScript>();
    if (itemContainerScriptInSlot != null)
    {
      return itemContainerScriptInSlot;
    }
    Debug.LogFormat("No item found in slot {0}", slotNumber);
    return null;
  }

  public List<ItemContainerScript> GetItemsFromInventory()
  {
    List<ItemContainerScript> itemsList = new List<ItemContainerScript>();
    ItemContainerScript itemContainerScriptInSlot;
    foreach (var slot in inventorySlots)
    {
      itemContainerScriptInSlot = slot.GetComponentInChildren<ItemContainerScript>();
      
      if (itemContainerScriptInSlot != null)
      {
       itemsList.Add(itemContainerScriptInSlot); 
      }
    }
    return itemsList;
  }

  public void CleanInventory()
  {
    foreach (var slot in inventorySlots)
    {
      ItemContainerScript itemContainerScriptInSlot = slot.GetComponentInChildren<ItemContainerScript>();
      if (itemContainerScriptInSlot != null)
      {
        itemContainerScriptInSlot.gameObject.SetActive(false);
        Destroy(itemContainerScriptInSlot.gameObject);
      }
    }
  }

  private void OpenPopupWindow(ItemContainerScript containerScript) //Check Inventory Items For Usage, get stats and translate id to opened popup window
  {
    popupWindow.GetComponent<PopupWindowScript>().OpenPopupWithStats(containerScript);
  }


  private void OnEnable()
  {
    ItemContainerScript.OnUseItem += OpenPopupWindow;
  }

  private void OnDisable()
  {
    ItemContainerScript.OnUseItem -= OpenPopupWindow;
  }
  
}
