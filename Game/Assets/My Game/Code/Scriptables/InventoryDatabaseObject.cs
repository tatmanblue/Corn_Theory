using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CornTheory.Scriptables
{
    /// <summary>
    /// Inventory for a player or an NPC.  
    /// </summary>
    [CreateAssetMenu(fileName = "Inventory DB", menuName = "Corn Theory/Inventory/Inventory Database")]
    public class InventoryDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public InventoryObject[] Container;
        public Dictionary<int, InventoryObject> AllItems = new Dictionary<int, InventoryObject>();
        public string Database;

        public void OnAfterDeserialize()
        {   
            if (null == AllItems)
                AllItems = new Dictionary<int, InventoryObject>();

            AllItems.Clear();

            if (null == Container)
                return;

            for (int index = 0; index < Container.Length; index ++)
            {
                AllItems.Add(index, Container[index]);
            }
        }

        /// <summary>
        /// unused, here for interface
        /// </summary>
        public void OnBeforeSerialize() { }
    }

    /// <summary>
    /// NOT SURE WHY the data members are public (vs properties)
    /// https://www.youtube.com/watch?v=_IqTeruf3-s&list=PLJWSdH2kAe_Ij7d7ZFR2NIW8QCJE74CyT
    /// </summary>
    public class InventorySlot
    {
        public int ItemId;
        public InventoryObject Item;
        public int Qty = 0;

        public InventorySlot(int itemId, int qty, InventoryObject item)
        {
            ItemId = itemId;
            Qty = qty;
            Item = item;
        }
    }
}
