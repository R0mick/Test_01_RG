using System;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public InventoryManagerScript inventoryManagerScript;
    public CharacterManagerScript characterManagerScript;
    public GameData gameData;
    public GameObject gameOverWindow;


   
    void Start()
    {
        if (gameData.IsSaveExist())
        {
            gameData.LoadData();
            gameData.DeleteSave();
        }
        else
        {
            SpawnStartItems();
        }
    }


    private void Update()
    {
        //Game Sequence
        if (characterManagerScript.enemyTurn)
        {
            EnemyHitsHero();
            
            characterManagerScript.enemyTurn = false;
        }

        if (characterManagerScript.enemyCurrentHealth <= 0)
        {
            inventoryManagerScript.AddRandomItem();
            characterManagerScript.UpdateEnemyHp(100);
        }
        
        if (characterManagerScript.heroCurrentHealth <= 0)
        {
            gameOverWindow.SetActive(true);
        }
    }


    private void SpawnStartItems()
    {
        inventoryManagerScript.AddItemFromListByName("Cap");
        inventoryManagerScript.AddItemFromListByName("Helmet");
        inventoryManagerScript.AddItemFromListByName("Jacket");
        inventoryManagerScript.AddItemFromListByName("BodyArmor");
        inventoryManagerScript.AddItemFromListByName("Heal_Pills");
        inventoryManagerScript.AddItemFromListByName("5.45x39_Rounds");
        inventoryManagerScript.AddItemFromListByName("9x18_Rounds");
    }
    
    public void EnemyHitsHero()
    {
        //Debug.Log(characterManagerScript.enemyAimHead);
        if (characterManagerScript.enemyAimHead)
        {
            characterManagerScript.HitToHero( "Head");
            characterManagerScript.enemyAimHead = false;
        }
        else
        {
            characterManagerScript.HitToHero( "Chest");
            characterManagerScript.enemyAimHead = true;
        }
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    
    
    private void OnApplicationQuit()
    {
       
            Debug.Log("autosave on exit");
            gameData.SaveData();
        
    }
}
