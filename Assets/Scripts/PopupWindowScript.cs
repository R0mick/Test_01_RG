using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PopupWindowScript : MonoBehaviour
{
    public InventoryManagerScript inventoryManagerScript;
    public GameObject popupWindow;
    public TMP_Text itemName;
    public Image itemImage;
    public TMP_Text armorText;
    public TMP_Text weightText;
    public Button actionButton;
    public Button altActionButton;
    
    public Image darkBackground;
    public CharacterManagerScript characterManagerScript;
    
    public void ShowPopup()
    {
        this.gameObject.SetActive(true);
        darkBackground.gameObject.SetActive(true);
    }

    public void HidePopup()
    {
        actionButton.onClick.RemoveAllListeners();
        altActionButton.onClick.RemoveAllListeners();
        this.gameObject.SetActive(false);
        darkBackground.gameObject.SetActive(false);
        
    }
    
    
    public void OpenPopupWithStats(InventoryItemScript itemScriptInSlot) //open popup and set stats in it from item
    {
        itemName.SetText(itemScriptInSlot.item.itemName);
        itemImage.sprite = itemScriptInSlot.item.sprite;
        armorText.SetText(itemScriptInSlot.item.armor.ToString());
        weightText.SetText(itemScriptInSlot.item.weight.ToString());



        actionButton.GetComponentInChildren<TMP_Text>().SetText(itemScriptInSlot.item.actionType.ToString());
        altActionButton.GetComponentInChildren<TMP_Text>().SetText(itemScriptInSlot.item.alternativeActionType.ToString());

        actionButton.onClick.AddListener(delegate { action(itemScriptInSlot); });
        altActionButton.onClick.AddListener(delegate { alternativeAction(itemScriptInSlot); });
        
        
        ShowPopup();
    }

    private void action(InventoryItemScript itemScriptInSlot)
    {
        switch (itemScriptInSlot.item.actionType)
        {
            case ItemScript.ActionType.Buy:
            {
                itemScriptInSlot.currentCount = itemScriptInSlot.item.maxCount;
                itemScriptInSlot.countText.SetText(itemScriptInSlot.currentCount.ToString());
                break;
            }
            case ItemScript.ActionType.Equip:
            {
                string currentlyEquipped = characterManagerScript.EquipItem(itemScriptInSlot);
                //Debug.Log("item in slot = " + currentlyEquipped);
                HidePopup();
                inventoryManagerScript.AddItemFromListByName(currentlyEquipped,itemScriptInSlot.currentSlotNumber+1);
                break;
            }
            case ItemScript.ActionType.Use:
            {
                characterManagerScript.UseItem(itemScriptInSlot.item);
                itemScriptInSlot.currentCount -= 1;
                itemScriptInSlot.countText.SetText(itemScriptInSlot.currentCount.ToString());
                if (itemScriptInSlot.currentCount == 0)
                {
                    Destroy(itemScriptInSlot.gameObject);
                }
                break;
            }
        }
        
        HidePopup();

    }

    private void alternativeAction(InventoryItemScript itemScriptInSlot)
    {
        if (itemScriptInSlot.item.alternativeActionType == ItemScript.AlternativeActionType.Delete)
        {
            Destroy(itemScriptInSlot.gameObject);
            HidePopup();
        }
    }
}
