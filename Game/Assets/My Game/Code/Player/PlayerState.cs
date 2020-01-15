using System;
using System.Collections;
using System.Collections.Generic;
using CornTheory.Missions;
using UnityEngine;

namespace CornTheory.Player
{
    /// <summary>
    /// TODO: do we replace PlayerState with ScriptableObject implementation?
    /// </summary>
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
        }

        public string When { get; private set; }
        public bool IsReady { get; private set; }
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public int MaxStamina { get; private set; }
        public int CurrentStamina { get; private set; }
        public GameUIState GameState { get; set; }

        // TODO this should be data driven
        public void LoadMissions()
        {
            GameObject missionManager = GameObject.Find(Constants.MissionMananger);
            MissionManager instance = missionManager.GetComponent<MissionManager>();

            instance.AddNewMission(new Mission()
            {
                Id = 1,
                Name = "This is your first mission.",
                Description = "Missions earn you coin and other items that help you.   Missions also help you learn new information and skills so that you can succeed. \r\n\r\nFind the farmer working in the corn field and speak to him.",
                State = MissionState.OnGoing,
                Parent = -1,
                Type = MissionType.Main,
                CoinReward = 10
            }, true);

            instance.AddNewMission(new Mission()
            {
                Id = 2,
                Name = "Find the bus stop",
                Description = "The farmer suggested to get to the bus stop at the top of the hill.  Find the bus stop and look around.  Maybe you will meet someone that can help you.",
                State = MissionState.NotStarted,
                Parent = -1,
                Type = MissionType.Main,
                CoinReward = 10
            });

            instance.AddNewMission(new Mission()
            {
                Id = 3,
                Name = "Walk to town.",
                Description = "Walk across the bridge into town.",
                State = MissionState.NotStarted,
                Parent = -1,
                Type = MissionType.Main,
                CoinReward = 10
            });



            IsReady = true;
        }

    }
}
