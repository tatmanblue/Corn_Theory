using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBD
{
    public interface IPopupBehaviors
    {
        void ShowInventory();
        void ShowMissions();
        void ShowProfile();
        void ShowQuitConfirm();
        void ShowSettings();

        void ReturnControlBackToWorld();
    }
}

