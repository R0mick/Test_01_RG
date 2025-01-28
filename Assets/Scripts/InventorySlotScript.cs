using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotScript : MonoBehaviour, IDropHandler
{
    public int slotNumber;
    public void OnDrop(PointerEventData eventData)
    {
        //links item to item slot after drop if empty or swap items if not 
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItemScript inventoryItemScript = dropped.GetComponent<InventoryItemScript>();
            inventoryItemScript.parentAfterDrag = transform;
        }else
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItemScript inventoryItemScript = dropped.GetComponent<InventoryItemScript>();
            
            GameObject current = transform.GetChild(0).gameObject;
            InventoryItemScript currentInventory = current.GetComponent<InventoryItemScript>();
            
            currentInventory.transform.SetParent(inventoryItemScript.parentAfterDrag);
            inventoryItemScript.parentAfterDrag = transform;

        }

    }
}
