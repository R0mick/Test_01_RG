using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotScript : MonoBehaviour, IDropHandler
{
    public int slotNumber;
    public void OnDrop(PointerEventData eventData)
    {
        //links container to slot after drop if empty or swap items if not 
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            ItemContainerScript itemContainerScriptDropped = dropped.GetComponent<ItemContainerScript>();
            itemContainerScriptDropped.parentAfterDrag = transform;
        }else
        {
            GameObject dropped = eventData.pointerDrag;
            ItemContainerScript itemContainerScriptDropped = dropped.GetComponent<ItemContainerScript>();
            
            GameObject current = transform.GetChild(0).gameObject;
            ItemContainerScript itemContainerScriptCurrent = current.GetComponent<ItemContainerScript>();
            
            itemContainerScriptCurrent.transform.SetParent(itemContainerScriptDropped.parentAfterDrag);
            itemContainerScriptDropped.parentAfterDrag = transform;

        }

    }
}
