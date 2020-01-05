using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CornTheory
{
    public interface IMission
    {
        int Parent { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        MissionState State { get; set; }
        MissionType Type { get; set; }
        IMissionStateCheck Check { get; set; }
        int CoinReward { get; set; }
    }

    public interface IMissionManager
    {
        void SceneOpened();
        void AddNewMission(IMission mission, bool activate = false);

        // these methods are used by NPCS in the game to 
        // change mission states
        void ActivateMission(int missionId);
        void CompleteMission(int missionId);

    }
}
