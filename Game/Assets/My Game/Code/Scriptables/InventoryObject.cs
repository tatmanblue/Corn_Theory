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
        /// this prefab is how the item appears in the player inventory
        /// </summary>
        public GameObject InventoryDisplay;
        public InventoryObjectTypes InventoryType = InventoryObjectTypes.NotConfigured;
        [TextArea(15, 20)]
        public string Description;

        public void Awake()
        {
            OnAwake();
        }

        public abstract void OnAwake();
    }
}
