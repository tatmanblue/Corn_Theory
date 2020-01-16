
using CornTheory.Inventory;
using UnityEngine;

namespace CornTheory.Scriptables
{
    [CreateAssetMenu(fileName = "New General Item", menuName = "Corn Theory/Inventory/General Object")]
    public class GeneralInventoryObject : InventoryDescriptionObject
    {
        public override void OnAwake()
        {
            this.InventoryType = InventoryObjectTypes.General;
        }
    }
}
