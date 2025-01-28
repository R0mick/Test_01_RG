using System;
using DefaultNamespace;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterManagerScript : MonoBehaviour
{
    public InventoryManagerScript inventoryManagerScript;
    
    [Header("Hero")]
    [Header("Technical")]
    public Image headImage;
    public Image chestImage;
    
    public TMP_Text headArmorCountText;
    public TMP_Text chestArmorCountText;

    public Image heroHealthBar;
    public TMP_Text heroCurrentHealthText;
    public GameObject gunHighliteBox;
    public GameObject machineGunHighliteBox;
    
    [Header("Stats")]
    public float heroMaxHealth=100f;
    public float heroCurrentHealth;
    public int headArmorCount;
    public int chestArmorCount;
    public int heroWeightCount;
    public string itemInHeadSlotName;
    public string itemInChestSlotName;
    public Weapon weapon;
    public int gunDamage=5;
    public int machineGunDamage=9;
    public int gunAmmoConsumption=1;
    public int machineGunAmmoConsumption=3;
   

    [Header("Enemy")]
    [Header("Technical")]
    public Image enemyHealthBar;
    public TMP_Text enemyCurrentHealthText;
    public float enemyMaxHealth = 100f;
    public float enemyCurrentHealth;
    public bool enemyTurn;
    public bool enemyAimHead=true;
    public int enemyDamage = 15;


    public enum Weapon
    {
        Gun,
        MachineGun
    }
    void Start()
    {
        heroCurrentHealth = heroMaxHealth;
        enemyCurrentHealth = enemyMaxHealth;
        SelectGun();
    }
    

    public void HitToHero(string bodyPart)
    {
        int calculatedDamage=0;
        if (bodyPart == "Head")
        {
            calculatedDamage = enemyDamage-headArmorCount;
            Debug.LogFormat("Hit for {0} to {1}",calculatedDamage,bodyPart);
        }

        if (bodyPart == "Chest")
        {
            calculatedDamage = enemyDamage-chestArmorCount;
            Debug.LogFormat("Hit for {0} to {1}",calculatedDamage,bodyPart);
        }
        
        heroCurrentHealth -= calculatedDamage;
        heroHealthBar.fillAmount = heroCurrentHealth / heroMaxHealth;
        heroCurrentHealthText.SetText(heroCurrentHealth.ToString());
        enemyTurn = false;
    }

    public String EquipItem(InventoryItemScript inventoryItemScript)
    {
        if (inventoryItemScript.item.itemType == ItemScript.ItemType.Head)
        {
            string currentlyEquipped = itemInHeadSlotName;
            
            itemInHeadSlotName = inventoryItemScript.item.itemName;
            headImage.sprite = inventoryItemScript.item.sprite;
            headImage.SetNativeSize();
            headArmorCountText.SetText(inventoryItemScript.item.armor.ToString());
            UpdateArmor();
            inventoryItemScript.gameObject.SetActive(false);
            Destroy(inventoryItemScript.gameObject);
            return currentlyEquipped;
            
        }
        
        if (inventoryItemScript.item.itemType == ItemScript.ItemType.Chest)
        {
            string currentlyEquipped = itemInChestSlotName;
            
            
            itemInChestSlotName = inventoryItemScript.item.itemName;
            chestImage.sprite = inventoryItemScript.item.sprite;
            chestImage.SetNativeSize();
            chestArmorCountText.SetText(inventoryItemScript.item.armor.ToString());
            UpdateArmor();
            inventoryItemScript.gameObject.SetActive(false);
            Destroy(inventoryItemScript.gameObject);
            return currentlyEquipped;
        }
        return null;
    }
    
    private void UpdateArmor()
    {
        headArmorCount = int.Parse(headArmorCountText.text);
        chestArmorCount = int.Parse(chestArmorCountText.text);
        //Debug.Log(headArmor);
        //Debug.Log(chestArmor);
    }

    public void UseItem(ItemScript item)
    {
        heroCurrentHealth += item.regeneration;
        if (heroCurrentHealth>=heroMaxHealth)
        {
            heroCurrentHealth = heroMaxHealth;
        }
        heroHealthBar.fillAmount = heroCurrentHealth / heroMaxHealth;
        heroCurrentHealthText.SetText(heroCurrentHealth.ToString());
    }

    public void SelectGun()
    {
        weapon = Weapon.Gun;
        gunHighliteBox.SetActive(true);
        machineGunHighliteBox.SetActive(false);
    }

    public void SelectMachineGun()
    {
        weapon = Weapon.MachineGun;
        machineGunHighliteBox.SetActive(true);
        gunHighliteBox.SetActive(false);
    }

    public void HitToEnemy()
    {
        if (weapon == Weapon.Gun)
        { 
            if(ConsumeAmmo(weapon)){
                enemyCurrentHealth -= gunDamage;
                enemyHealthBar.fillAmount = enemyCurrentHealth / enemyMaxHealth;
                enemyCurrentHealthText.SetText(enemyCurrentHealth.ToString());
                Debug.LogFormat("Shoot using {0} for {1} dmg", weapon, gunDamage);
            }
        }
        if (weapon == Weapon.MachineGun)
        {
            if (ConsumeAmmo(weapon))
            {
                enemyCurrentHealth -= machineGunDamage;
                enemyHealthBar.fillAmount = enemyCurrentHealth / enemyMaxHealth;
                enemyCurrentHealthText.SetText(enemyCurrentHealth.ToString());
                Debug.LogFormat("Shoot using {0} for {1} dmg", weapon, machineGunDamage);
            }
        }

        enemyTurn = true;
    }

    private bool ConsumeAmmo(Weapon weapon)
    {
        if (weapon == Weapon.Gun)
        {
            foreach (var slot in inventoryManagerScript.inventorySlots)
            {
                InventoryItemScript itemScriptInSlot = slot.GetComponentInChildren<InventoryItemScript>();
                if (itemScriptInSlot != null)
                {
                    if (itemScriptInSlot.item.itemName == "9x18_Rounds"&& itemScriptInSlot.currentCount>=1)
                    {
                        itemScriptInSlot.currentCount-= 1;
                        itemScriptInSlot.countText.SetText(itemScriptInSlot.currentCount.ToString());
                        return true;
                    }
                }
            }
            Debug.Log("No Ammo!");
            return false;
        }
        if (weapon == Weapon.MachineGun)
        {
            foreach (var slot in inventoryManagerScript.inventorySlots)
            {
                InventoryItemScript itemScriptInSlot = slot.GetComponentInChildren<InventoryItemScript>();
                if (itemScriptInSlot != null)
                {
                    if (itemScriptInSlot.item.itemName == "5.45x39_Rounds"&& itemScriptInSlot.currentCount>=3)
                    {
                        itemScriptInSlot.currentCount-= 3;
                        itemScriptInSlot.countText.SetText(itemScriptInSlot.currentCount.ToString());
                        return true;
                    }
                }
            }
        }
        Debug.Log("No Ammo!");
        return false;
    }

    public void UpdateEnemyHp(float newHp)
    {
        enemyCurrentHealth = newHp;
        enemyCurrentHealthText.SetText(newHp.ToString());
        enemyHealthBar.fillAmount = newHp / enemyMaxHealth;
    }
    public void UpdateHeroHp(float newHp)
    {
        heroCurrentHealth = newHp;
        heroCurrentHealthText.SetText(newHp.ToString());
        heroHealthBar.fillAmount = newHp / enemyMaxHealth;
    }

    public void CleanEquippedItems()
    {
    itemInHeadSlotName="";
    itemInChestSlotName="";
    }
}
