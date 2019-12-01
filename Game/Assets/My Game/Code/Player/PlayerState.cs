using System;
using System.Collections;
using System.Collections.Generic;
using CornTheory.Missions;
using UnityEngine;

namespace CornTheory.Player
{
    public class PlayerState
    {
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
            Missions = new MissionList();
        }

        public string When { get; private set; }
        public bool IsReady { get; private set; }
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public int MaxStamina { get; private set; }
        public int CurrentStamina { get; private set; }
        public GameUIState GameState { get; set; }
        public MissionList Missions { get; private set; }

        // TODO this should be data driven
        public void LoadMissions()
        {
            GameObject missionManager = GameObject.Find(Constants.MissionMananger);
            MissionManager instance = missionManager.GetComponent<MissionManager>();

            instance.Missions.Add(new Mission()
            {
                Id = 1,
                Name = "Speak to the Farmer",
                Description = "There is a farmer nearby.  Speak to him.  He might have information to help you.",
                State = MissionState.OnGoing,
                Parent = -1,
                Type = MissionType.Main
            });

            instance.Missions.Add(new Mission()
            {
                Id = 2,
                Name = "Find the bus stop",
                Description = "The farmer suggested to get to the bus stop at the top of the hill.  Find the bus stop.",
                State = MissionState.NotStarted,
                Parent = -1,
                Type = MissionType.Main
            });

            instance.Missions.Add(new Mission()
            {
                Id = 3,
                Name = "Walk to town.",
                Description = "Walk across the bridge into town.",
                State = MissionState.NotStarted,
                Parent = -1,
                Type = MissionType.Main
            });

            IsReady = true;
        }

    }
}
