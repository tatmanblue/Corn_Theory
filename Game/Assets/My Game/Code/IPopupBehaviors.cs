using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CornTheory
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

