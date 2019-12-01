using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CornTheory
{
    /// <summary>
    /// implementations are called to check if the mission has completed
    /// </summary>
    public interface IMissionStateCheck
    {
        /// <summary>
        /// Checks to see if the conditions have been met to call the mission completed
        /// </summary>
        /// <returns>MissionState.Finished</returns>
        MissionState DoCheck();
    }

    /// <summary>
    /// implementations are expecting a converaation with an NPC to trigger event change 
    /// </summary>
    public interface IMissionStateChangesOnSpeakEvent : IMissionStateCheck
    {
        void HandleSpeakEvent(int eventId);
    }
}