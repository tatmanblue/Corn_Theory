using CornTheory.Scriptables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CornTheory.Inventory
{
    public class InventoryItemGiverHandler : MonoBehaviour
    {
        [SerializeField] InventoryDescriptionObject Item;
        [SerializeField] int GiveQuanity;
        [SerializeField] bool HasGivenItem;

        private void OnTriggerEnter(Collider other)
        {
            if (true == HasGivenItem)
                return;

            if (0 == other.gameObject.name.CompareTo(Constants.Player))
            {
                GiveItem(other.gameObject.transform.GetComponent<Player.Player>());
            }
        }

        private void GiveItem(Player.Player player)
        {
            HasGivenItem = true;
            player.AddInventoryItem(Item, GiveQuanity);
        }
    }
}
