using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class DemoScript : MonoBehaviour
{
    public InventoryManagerScript inventoryManagerScript;
    public CharacterManagerScript characterManagerScript;

    private bool hit = true;
    
    
    public void SpawnRound()
    {
        
        inventoryManagerScript.AddItemToEmptySlot(inventoryManagerScript.items[Random.value < 0.5f ? 0 : 1]);
    
    }

    
    public void SpawnHelmet()
    {
        
    inventoryManagerScript.AddItemToEmptySlot(inventoryManagerScript.items[Random.value < 0.5f ? 2 : 3]);
    
    }
    
    public void SpawnArmor()
    {
        
        inventoryManagerScript.AddItemToEmptySlot(inventoryManagerScript.items[Random.value < 0.5f ? 4 : 5]);
    
    }
    
    public void SpawnHeal()
    {
        
        inventoryManagerScript.AddItemToEmptySlot(inventoryManagerScript.items[6]);
    
    }

    public void SpawnRandomItem()
    { 
        inventoryManagerScript.AddItemToEmptySlot(inventoryManagerScript.items[Random.Range(0, inventoryManagerScript.items.Length)]);
    }

    public void hitHero()
    {
        if (hit)
        {
            characterManagerScript.HitToHero( "Head");
            hit = false;
        }
        else
        {
            characterManagerScript.HitToHero("Chest");
            hit = true;
        }
    }
    
}
