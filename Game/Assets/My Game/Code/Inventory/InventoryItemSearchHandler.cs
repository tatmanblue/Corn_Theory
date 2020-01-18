using UnityEngine;
using UnityEngine.EventSystems;
using CornTheory.Scriptables;


namespace CornTheory.Inventory
{
    public class InventoryItemSearchHandler : MonoBehaviour, IPointerClickHandler
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
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    print(string.Format("HIT DATA: {0}", hit.transform.name));
                    if (0 == hit.transform.name.CompareTo(transform.name))
                    {
                        print(string.Format("InventoryItemSearchHandler mouse down for {0}", transform.name));
                        startTime = Time.time;
                        endTime = InventoryItemSearchHandler.NOT_SET;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                // only care about this if startTime was set
                if (InventoryItemSearchHandler.NOT_SET < startTime)
                {
                    print(string.Format("InventoryItemSearchHandler mouse up for {0}", transform.name));
                    endTime = Time.time;
                    print(string.Format("duration {0}", endTime - startTime));
                }
            }

            if (endTime - startTime > (clickDurationMS / 1000))
            {
                HasGivenItem = true;
                endTime = InventoryItemSearchHandler.NOT_SET;
                startTime = InventoryItemSearchHandler.NOT_SET;

                Player.Player player = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player.Player>();
                GiveItem(player);
                print("given inventory");
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            print(string.Format("I got the click {0}", transform.name));
        }
    }
}
