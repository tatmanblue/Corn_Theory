using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CornTheory
{
    /// <summary>
    /// identifies where in the UI the game is
    /// </summary>
    public enum GameUIState
    {
        /// <summary>
        /// Main screen
        /// </summary>
        AtMain,
        /// <summary>
        /// In world, playing game
        /// </summary>
        InWorld,
        /// <summary>
        /// In world but interacting with POPUP UI like the missions screen
        /// </summary>
        InWorldUI
    }

    [Serializable]
    public enum MissionState
    {
        NotStarted,
        OnGoing,
        Finished
    }

    [Serializable]
    public enum MissionType
    {
        Main,
        Sideline
    }
}
