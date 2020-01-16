using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CornTheory.Scriptables
{
    public enum InventoryObjectTypes
    {
        NotConfigured,  // This is an error and should never be a value seen at runtime
        General,        // Sellable but no other value
    }

    /// <summary>
    /// TODO: should public data members be properties
    /// </summary>
    public abstract class InventoryObject : ScriptableObject
    {
        /// <summary>
        /// this prefab is how the item appears in rectangle of the the player inventory list
        /// </summary>
        public Sprite InventoryDisplay;
        public InventoryObjectTypes InventoryType = InventoryObjectTypes.NotConfigured;
        [TextArea(15, 20)]
        public string Description;
        public int Qty = 1;

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
