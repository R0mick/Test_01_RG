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
    
    
    public void OpenPopupWithStats(ItemContainerScript itemContainerScriptInSlot) //open popup and set stats in it from item
    {
        itemName.SetText(itemContainerScriptInSlot.item.itemName);
        itemImage.sprite = itemContainerScriptInSlot.item.sprite;
        armorText.SetText(itemContainerScriptInSlot.item.armor.ToString());
        weightText.SetText(itemContainerScriptInSlot.item.weight.ToString());



        actionButton.GetComponentInChildren<TMP_Text>().SetText(itemContainerScriptInSlot.item.actionType.ToString());
        altActionButton.GetComponentInChildren<TMP_Text>().SetText(itemContainerScriptInSlot.item.alternativeActionType.ToString());

        actionButton.onClick.AddListener(delegate { action(itemContainerScriptInSlot); });
        altActionButton.onClick.AddListener(delegate { alternativeAction(itemContainerScriptInSlot); });
        
        
        ShowPopup();
    }

    private void action(ItemContainerScript itemContainerScriptInSlot)
    {
        switch (itemContainerScriptInSlot.item.actionType)
        {
            case ItemScript.ActionType.Buy:
            {
                itemContainerScriptInSlot.currentCount = itemContainerScriptInSlot.item.maxCount;
                itemContainerScriptInSlot.countText.SetText(itemContainerScriptInSlot.currentCount.ToString());
                break;
            }
            case ItemScript.ActionType.Equip:
            {
                string currentlyEquipped = characterManagerScript.EquipItem(itemContainerScriptInSlot);
                //Debug.Log("item in slot = " + currentlyEquipped);
                HidePopup();
                inventoryManagerScript.AddItemFromListByName(currentlyEquipped,itemContainerScriptInSlot.currentSlotNumber+1);
                break;
            }
            case ItemScript.ActionType.Use:
            {
                characterManagerScript.UseItem(itemContainerScriptInSlot.item);
                itemContainerScriptInSlot.currentCount -= 1;
                itemContainerScriptInSlot.countText.SetText(itemContainerScriptInSlot.currentCount.ToString());
                if (itemContainerScriptInSlot.currentCount == 0)
                {
                    Destroy(itemContainerScriptInSlot.gameObject);
                }
                break;
            }
        }
        
        HidePopup();

    }

    private void alternativeAction(ItemContainerScript itemContainerScriptInSlot)
    {
        if (itemContainerScriptInSlot.item.alternativeActionType == ItemScript.AlternativeActionType.Delete)
        {
            Destroy(itemContainerScriptInSlot.gameObject);
            HidePopup();
        }
    }
}
