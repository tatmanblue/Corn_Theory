
using CornTheory.Scriptables;
using UnityEngine;

namespace CornTheory.Inventory
{
    /// <summary>
    /// This class is responsible for Interacting with Player Inventory and displaying
    /// the inventory on the "Inventory Dlg" screen
    /// </summary>
    public class InventoryViewLoader : MonoBehaviour
    {
        private static Color SELECTED_BORDER_COLOR = new Color(255, 214, 11, 205);
        private static Color DEFAULT_BORDER_COLOR = new Color(255, 255, 255, 255);

        private static int COLUMNS = 8;
        private static int START_X = -421;
        private static int START_Y = 184;
        private static int HORZ_BUFFER = 40;
        private static int VERT_BUFFER = 40;

        private int selectedItemIndex = 0;

        public InventoryDatabaseObject Inventory;
        public GameObject ItemDisplay;

        private void Start()
        {
            CreateDisplay();
        }

        private void CreateDisplay()
        {
            if (null == Inventory)
            {
                print("THERE IS NO INVENTORY DB");
                return;
            }

            if (null == ItemDisplay)
            {
                print("INVENTORY ITEM DISPLAY PREFAB MISSING");
                return;
            }

            for (int index = 0; index < Inventory.Container.Length; index ++)
            {
                var itemPrefab = Instantiate(ItemDisplay, Vector3.zero, Quaternion.identity, transform);
            }
        }
    }
}
