
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
        private static int START_Y = 167;
        private static int X_SPACE_BETWEEN_ITEMS = 120;
        private static int Y_SPACE_BETWEEN_ITEMS = 120;

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

            var parentTransform = GameObject.Find("Items Panel").transform;

            for (int index = 0; index < Inventory.Container.Length; index ++)
            {
                var itemPrefab = Instantiate(ItemDisplay, Vector3.zero, Quaternion.identity, parentTransform);
                itemPrefab.GetComponent<RectTransform>().localPosition = ComputeItemPosition(index);
            }
        }

        private Vector3 ComputeItemPosition(int index)
        {
            float x = START_X + (X_SPACE_BETWEEN_ITEMS * (index % COLUMNS));
            float y = START_Y + ((Y_SPACE_BETWEEN_ITEMS * -1) * (index / COLUMNS));
            print(string.Format("{2} places at {0},{1}", x, y, index));
            return new Vector3(x, y, 0F);
        }
    }
}
