using CornTheory.Scriptables;
using System;
using UnityEngine;

namespace CornTheory.Inventory
{
    public class InventoryItemSearchHandler : MonoBehaviour
    {
        [SerializeField] InventoryDescriptionObject Item;
        [SerializeField] int GiveQuanity;
        [SerializeField] float clickDurationMS;
        [SerializeField] bool HasGivenItem;

        private float startTime = 0F;
        private float endTime = 0F;

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
                print("InventoryItemSearchHandler mouse down");
                startTime = Time.time;
            }

            if (Input.GetMouseButtonUp(0))
            {
                print("InventoryItemSearchHandler mouse up");
                endTime = Time.time;
                print(string.Format("duration {0}", endTime - startTime));
            }

            if (endTime - startTime > (clickDurationMS / 1000))
            {
                HasGivenItem = true;
                endTime = 0F;
                startTime = 0F;

                Player.Player player = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<Player.Player>();
                GiveItem(player);
                print("given inventory");
            }
        }


        private void GiveItem(Player.Player player)
        {
            if (null == player)
                print("player is null!!!!");

            HasGivenItem = true;
            player.AddInventoryItem(Item, GiveQuanity);
        }
    }
}
