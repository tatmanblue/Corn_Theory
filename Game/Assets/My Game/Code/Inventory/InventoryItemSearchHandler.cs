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
        private bool isMouseDown = false;
        private SearchProgressBarProgress progressHandler = null;

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

            bool mouseIsOnSelf = IsMouseOnSelf();

            if (Input.GetMouseButtonDown(0))
            {
                if (true == mouseIsOnSelf)
                {
                    startTime = Time.time;
                    endTime = InventoryItemSearchHandler.NOT_SET;
                    isMouseDown = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                // only care about this if startTime was set
                if (InventoryItemSearchHandler.NOT_SET < startTime)
                {
                    isMouseDown = false;
                    // the rule is in order for the mouse hold down to count has holding the mouse down
                    // for the duration it has to be released over this object.  if it is
                    // released elsewhere, then it doesn't count and the attempt
                    // has to start over
                    if (true == mouseIsOnSelf)
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

            if (endTime - startTime > (clickDurationMS / 1000.0F))
            {
                HasGivenItem = true;
                endTime = InventoryItemSearchHandler.NOT_SET;
                startTime = InventoryItemSearchHandler.NOT_SET;

                Player.Player player = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player.Player>();
                GiveItem(player);
                print(string.Format("{0} gave inventory item {1}", transform.name, Item.name));
            }
            else if (true == isMouseDown && true == mouseIsOnSelf)
            {                
                if (null == progressHandler)
                {
                    progressHandler = GetComponentInChildren<SearchProgressBarProgress>();
                }

                if (progressHandler.CurrentProgress < progressHandler.MaxProgress)
                {
                    float stepsPerMS = progressHandler.MaxProgress / clickDurationMS;
                    float duration = (Time.time * 1000F) - (startTime * 100F);
                    int amount = (int)(stepsPerMS * duration);
                    progressHandler.ModifyProgress(amount);
                }
            }
        }

    }
}
