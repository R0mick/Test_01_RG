using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ItemScript:MonoBehaviour
    {
        public string itemName;
        public ItemType itemType;
        public ActionType actionType;
        public AlternativeActionType alternativeActionType;
        public Sprite sprite;
        public bool stackable = false;
    
        [Header("Stats")]
        public int maxCount;
        public int armor;
        public int regeneration;
        public float weight;
    
    
        public enum ActionType
        {
            Buy,
            Use,
            Equip
        }
    
        public enum AlternativeActionType
        {
            Delete
        }
    
        public enum ItemType
        {
            Head,
            Chest,
            Consumable,
            Ammo
        }
    }
}