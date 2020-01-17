using CornTheory.Inventory;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CornTheory.Scriptables
{

    /// <summary>
    /// TODO: should public data members be properties?
    /// 
    /// InventoryDescriptionObject describes an object that can be held in player or NPC inventories
    /// It does not describe what the player or NPC has (aka qty) as that is an "Inventory Slot"
    /// Why?  You can have 0 to many in the inventory but you only need one description of the item
    /// </summary>
    public abstract class InventoryDescriptionObject : ScriptableObject
    {
        /// <summary>
        /// this prefab is how the item appears in rectangle of the the player inventory list
        /// </summary>
        public Sprite InventoryDisplay;
        public InventoryObjectTypes InventoryType = InventoryObjectTypes.NotConfigured;
        [TextArea(15, 20)]
        public string Description;
        // TODO: make this system generated
        public int Id = 0;


        /// <summary>
        /// by calling the abstract OnAwake in the Awake method, we force derived classes
        /// to implement OnAwake.
        /// </summary>
        public void Awake()
        {
            OnAwake();
        }

        /// <summary>
        /// the expection is derived classes implement OnAwake to self initialize
        /// </summary>
        public abstract void OnAwake();
    }
}
