using UnityEngine;
using CornTheory.Scriptables;


namespace CornTheory.Inventory
{
    public class InventoryItemSearchHandler : MonoBehaviour
    {
        [SerializeField] InventoryDescriptionObject Item;
        [SerializeField] int GiveQuanity;
        [SerializeField] float clickDurationMS;
        [SerializeField] bool HasGivenItem;

        private readonly static float NOT_SET = -1F;

        private float startTime = InventoryItemSearchHandler.NOT_SET;
        private float endTime = InventoryItemSearchHandler.NOT_SET;

        private void GiveItem(Player.Player player)
        {
            if (null == player)
                print("player is null!!!!");

            HasGivenItem = true;
            player.AddInventoryItem(Item, GiveQuanity);
        }

        /// <summary>
        /// checks to see if the mouse position is over the object this script is attached to
        /// </summary>
        /// <returns></returns>
        private bool IsMouseOnSelf()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
                if (0 == hit.transform.name.CompareTo(transform.name))
                    return true;

            return false;
        }

        private void Update()
        {
            if (true == HasGivenItem)
                return;

            if (0 >= clickDurationMS)
                return;

            if (null == Item)
                return;

            if (0 == GiveQuanity)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                if (true == IsMouseOnSelf())
                {
                    startTime = Time.time;
                    endTime = InventoryItemSearchHandler.NOT_SET;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                // only care about this if startTime was set
                if (InventoryItemSearchHandler.NOT_SET < startTime)
                {
                    // the rule is in order for this to count has holding the mouse down
                    // for the duration it has to be released over this object.  if it is
                    // released elsewhere, then it doesnt count and the attempt
                    // has to start over
                    if (true == IsMouseOnSelf())
                    {
                        endTime = Time.time;
                    }
                    else
                    {
                        startTime = InventoryItemSearchHandler.NOT_SET;
                        endTime = InventoryItemSearchHandler.NOT_SET;
                    }
                }
            }

            if (endTime - startTime > (clickDurationMS / 1000))
            {
                HasGivenItem = true;
                endTime = InventoryItemSearchHandler.NOT_SET;
                startTime = InventoryItemSearchHandler.NOT_SET;

                Player.Player player = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player.Player>();
                GiveItem(player);
                print(string.Format("{0} gave inventory item {1}", transform.name, Item.name));
            }
        }

    }
}
