using System;
using System.Collections;
using System.Collections.Generic;
using CornTheory.Missions;
using CornTheory.Utility;
using Language.Lua;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace CornTheory.Player
{
    /// <summary>
    /// TODO: do we replace PlayerState with ScriptableObject implementation?
    /// </summary>
    public class PlayerState
    {
        // TODO when/where does this get loaded from data
        private static PlayerState DATA_INSTANCE = new PlayerState();
        public static PlayerState Instance
        {
            get { return DATA_INSTANCE; }
        }

        private PlayerState()
        {
            When = DateTime.Now.ToShortTimeString();
            GameState = GameUIState.AtMain;
            MaxHealth = 100;
            CurrentHealth = 100;
            MaxStamina = 100;
            CurrentStamina = 100;
        }

        public string When { get; private set; }
        public bool IsReady { get; private set; }
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public int MaxStamina { get; private set; }
        public int CurrentStamina { get; private set; }
        public GameUIState GameState { get; set; }
        public string Stuff { get; set; }

        public void LoadMissions()
        {
            GameObject missionManager = GameObject.Find(Constants.MissionMananger);
            MissionManager instance = missionManager.GetComponent<MissionManager>();

            // TODO: if this fails, the system is no longer functional.  Why would if fail?
            // what should we do about it
            List<MissionIndex> items = JsonTools.LoadFromResource<List<MissionIndex>>("Missions/MissionMaster");

            foreach(MissionIndex index in items)
            {
                Mission mission = JsonTools.LoadFromResource<Mission>(string.Format("Missions/{0}", index.File));
                instance.AddNewMission(mission, mission.Id == 1.0M ? true : false);
            }

            IsReady = true;
        }
    }
}
