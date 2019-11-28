using System.Collections;
using System.Collections.Generic;
using TBD.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TBD.Missions
{
    public class MissionManager : MonoBehaviour, IMissionManager
    {
        private PopupInstanceHandler popupHandler;

        public MissionList Missions { get; private set; }

        public void ActivateMission(int missionId)
        {
            lock (Missions)
            {
                Mission mission = Missions.Find(x => x.Id == missionId) as Mission;
                if (null == mission)
                    return;

                print("activating mission " + missionId);
                mission.State = MissionState.OnGoing;
                popupHandler.Invoke("ShowMissions", Constants.PopupInvokeDelay);
            }
        }

        public void AddNewMission(IMission mission)
        {
            Missions.Add(mission);
        }

        public void CompleteMission(int missionId)
        {
            lock (Missions)
            {
                Mission completedMission = Missions.Find(x => x.Id == missionId) as Mission;
                if (null == completedMission)
                    return;

                print("completing mission " + missionId);
                completedMission.State = MissionState.Finished;
            }
        }

        public void SceneOpened()
        {
            var scene = SceneManager.GetActiveScene();
            print("MissionManager.SceneOpened() called in scene " + scene.name);
            if (scene.name == Constants.MainWorldScene || scene.name == Constants.DevPlayArenaScene)
            {
                PlayerState.Instance.LoadMissions();
                popupHandler.Invoke("ShowMissions", 0f);
            }
        }

        private void Awake()
        {
            if (null == Missions)
                Missions = new MissionList();

            popupHandler = FindObjectOfType<PopupInstanceHandler>();
        }

        // Start is called before the first frame update
        private void Start() { }

        // Update is called once per frame
        private void Update() { }
    }
}