using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CornTheory.Inventory
{
    public enum InventoryObjectTypes
    {
        NotConfigured,  // This is an error and should never be a value seen at runtime
        General,        // Sellable but no other value
    }
}
