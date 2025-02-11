using System;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;


public class GameData : MonoBehaviour
{

    public CharacterManagerScript characterManagerScript;
    public InventoryManagerScript inventoryManagerScript;

    private readonly string _saveDataPath = Application.dataPath + "/Test_01_RG_SaveData.dat";
    private readonly string _saveDataMetaPath = Application.dataPath + "/Test_01_RG_SaveData.dat.meta";




    [Serializable]
    public class DataToSave
    {
        //Hero data
        public float heroHp;
        public string heroWeapon;
        public string headArmor;
        public string chestArmor;

        //Inventory
        //get InventoryItem's list
        //get info about slot_number, itemName, count from InventoryItem
        //write to ItemContainerScript_alt List
        //string format "||slot_number|itemName|count"

        public string itemsString;

        //Hero data
        public float enemyHp;
        public bool enemyAimHead;
    }

   
    public void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Create(_saveDataPath);
        DataToSave data = new DataToSave();

        //saving here all data we have
        data.heroHp = characterManagerScript.heroCurrentHealth;

        data.heroWeapon = characterManagerScript.weapon.ToString();
        data.headArmor = characterManagerScript.itemInHeadSlotName;
        data.chestArmor = characterManagerScript.itemInChestSlotName;
        data.itemsString = ConvertInventoryItemsToString();

        data.enemyHp = characterManagerScript.enemyCurrentHealth;
        data.enemyAimHead = characterManagerScript.enemyAimHead;
        
        //--------------------------------

        formatter.Serialize(stream, data);

        //Debug.LogFormat("File saved to {0}", path);

        stream.Close();

    }


    private DataToSave LoadFile()
    {
        if (File.Exists(_saveDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(_saveDataPath, FileMode.Open);
            DataToSave data = (DataToSave)formatter.Deserialize(stream);

            stream.Close();
            return data;
        }

        return null;
    }


    public void LoadData()
    {
        DataToSave data = LoadFile();

        if (data == null)
        {
            Debug.Log("No data to load!");
            return;
        }

        //loading data/stats from file
        //heroHP
        characterManagerScript.UpdateHeroHp(data.heroHp);
        //heroWeapon
        if (data.heroWeapon.Contains("Gun"))
        {
            characterManagerScript.SelectGun();
        }

        if (data.heroWeapon.Contains("MachineGun"))
        {
            characterManagerScript.SelectMachineGun();
        }

        //heroHeadArmor
        if (data.headArmor != "")
        {
            inventoryManagerScript.AddItemFromListByName(data.headArmor);
            characterManagerScript.EquipItem(inventoryManagerScript.getItemFromSlot(1));
        }

        //chestArmor
        if (data.chestArmor != "")
        {
            inventoryManagerScript.AddItemFromListByName(data.chestArmor);
            characterManagerScript.EquipItem(inventoryManagerScript.getItemFromSlot(1));
        }
        
        //heroInventory
        
        ConvertStringToInventoryItems(data.itemsString);


        //enemyHP
        characterManagerScript.UpdateEnemyHp(data.enemyHp);
        characterManagerScript.enemyAimHead = data.enemyAimHead;

    }

    public void DeleteSave()
    {
        File.Delete(_saveDataPath);
        File.Delete(_saveDataMetaPath);
    }

    string ConvertInventoryItemsToString()
    {
        string itemsString = string.Empty;
        List<ItemContainerScript> items = inventoryManagerScript.GetItemsFromInventory();

        foreach (var item in items)
        {
            itemsString += string.Format("|{0}|{1}|{2}", (item.currentSlotNumber+1).ToString(), item.item.itemName,
                item.currentCount.ToString());
            //Debug.Log(itemsString);

        }

        return itemsString;
    }

    void ConvertStringToInventoryItems(string itemsString)
    {
        inventoryManagerScript.CleanInventory();
        var elements = itemsString.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        for (int i = 0; i < elements.Length; i += 3)
        {
            if (i + 2 < elements.Length)
            {
                //Debug.LogFormat(" item: {0},{1},{2}", int.Parse(elements[i]), elements[i + 1], int.Parse(elements[i + 2]));
                inventoryManagerScript.AddItemFromListByName(int.Parse(elements[i]), elements[i + 1], int.Parse(elements[i + 2]));
            }
        }
    }

    public bool IsSaveExist()
    {
        return File.Exists(_saveDataPath);
    }
}

