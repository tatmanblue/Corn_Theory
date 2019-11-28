using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBD.Player;
using UnityEngine;

namespace TBD
{
    /// <summary>
    /// Contains code for instantiating popup windows.  These windows stop interaction
    /// with the world.
    /// </summary>
    public class PopupInstanceHandler : MonoBehaviour, IPopupBehaviors
    {
        [SerializeField] private GameObject inventory;
        [SerializeField] private GameObject missions;
        [SerializeField] private GameObject profile;
        [SerializeField] private GameObject quitConfirm;
        [SerializeField] private GameObject settings;

        private GameObject canvas;

        public void ShowInventory()
        {
            ShowPopup(inventory);
        }

        public void ShowMissions()
        {
            ShowPopup(missions);
        }

        public void ShowProfile()
        {
            ShowPopup(profile);
        }

        public void ShowQuitConfirm()
        {
            ShowPopup(quitConfirm);
        }

        public void ShowSettings()
        {
            ShowPopup(settings);
        }

        public void ReturnControlBackToWorld()
        {
            PlayerState.Instance.GameState = GameUIState.InWorld;
        }

        private void Start()
        {
            canvas = GameObject.Find(Constants.Canvas);
        }

        private void ShowPopup(GameObject which)
        {
            if (null == which)
                return;

            var popup = Instantiate(which) as GameObject;
            popup.SetActive(true);
            popup.transform.localScale = Vector3.zero;

            if (null != canvas)
                popup.transform.SetParent(canvas.transform, false);

            popup.GetComponent<TBD.Popup2>().Open();
        }
    }
}
