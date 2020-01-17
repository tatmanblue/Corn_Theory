using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace CornTheory.Scriptables
{
    /// <summary>
    /// Inventory for a player or an NPC.  
    /// </summary>
    [CreateAssetMenu(fileName = "Inventory DB", menuName = "Corn Theory/Inventory/Inventory Database")]
    public class InventoryDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public InventoryDescriptionObject[] AllItems;                          // this is all of the items possible for a given inventory, configured in game dev
        public string CharacterInventoryFile;                                  // file name for loading the character inventory file
        public Dictionary<int, InventorySlot> CharactorInventory = new Dictionary<int, InventorySlot>();
        
        public void OnAfterDeserialize()
        {   
            if (null == CharactorInventory)
                CharactorInventory = new Dictionary<int, InventorySlot>();

            CharactorInventory.Clear();
        }

        /// <summary>
        /// unused, here for interface
        /// </summary>
        public void OnBeforeSerialize() { }

        public void AddInventoryItem(InventoryDescriptionObject item, int qty)
        {
            InventorySlot slot;
            if (CharactorInventory.ContainsKey(item.Id))
            {
                slot = CharactorInventory[item.Id];
            }
            else
            {
                slot = new InventorySlot(item.Id, 0, item);
                CharactorInventory.Add(item.Id, slot);
            }

            slot.Qty += qty;
        }

        public void RemoveInventoryItem(InventoryDescriptionObject item, int qty)
        {
            throw new System.NotImplementedException("RemoveInventoryItem");
        }

        /// <summary>
        /// TODO: later we need break this out into two different functions:  there is
        /// load new character inventory and load a previously saved charactor inventory.
        /// for now, just load the default
        /// </summary>
        [ContextMenu("Load Inventory")]
        public void LoadCharactorInventory()
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(System.IO.Path.Combine(Application.persistentDataPath, CharacterInventoryFile), FileMode.Open, FileAccess.Read))
            {
                CharactorInventory = (Dictionary<int, InventorySlot>)formatter.Deserialize(stream); ;
            }
        }

        [ContextMenu("Save Inventory")]
        public void SaveCharactoInventory()
        {
            IFormatter formatter = new BinaryFormatter();
            // path is data unique to the gamer since its running on their system.   such as "C:/Users/mattr/AppData/LocalLow/Tatman Game Studios/Corn Theory"
            using (Stream stream = new FileStream(System.IO.Path.Combine(Application.persistentDataPath, CharacterInventoryFile), FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, CharactorInventory);
            }
        }
    }

    /// <summary>
    /// NOT SURE WHY the data members are public (vs properties)
    /// https://www.youtube.com/watch?v=_IqTeruf3-s&list=PLJWSdH2kAe_Ij7d7ZFR2NIW8QCJE74CyT
    /// </summary>
    [System.Serializable]
    public class InventorySlot
    {
        public int ItemId;
        public InventoryDescriptionObject Item;
        public int Qty = 0;

        public InventorySlot(int itemId, int qty, InventoryDescriptionObject item)
        {
            ItemId = itemId;
            Qty = qty;
            Item = item;
        }
    }
}
