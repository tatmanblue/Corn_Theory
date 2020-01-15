using UnityEngine;

namespace CornTheory.Missions
{
    /// <summary>
    /// This class is responsible for interactiing with the MissionManager and showing
    /// the active (and completed) missions on the "Single Mission Dlg" screen.
    /// </summary>
    public class SingleViewMissionListLoader : MonoBehaviour
    {
        private MissionManager missionManager = null;
        private IMission activeMission = null;

        private static Color SUCCESS_COLOR = new Color(106, 203, 93, 205);
        private static Color NOT_SUCCESS_COLOR = new Color(168, 168, 168, 168);

        // Start is called before the first frame update
        void Start()
        {
            missionManager = FindObjectOfType<MissionManager>();
            if (null == missionManager)
            {
                print("MISSING MissionManager!");
                return;
            }

            if (null == missionManager.ActiveMission)
            {
                print("NO ACTIVE Mission to display");
                return;
            }

            activeMission = missionManager.ActiveMission;
        }

        // Update is called once per frame
        void Update() 
        {
            SetAll();
        }

        public void ShowNextMission()
        {
            print("ShowNextMission()");
            IMission mission = missionManager.Missions.Find(x => x.Id == activeMission.Id + 1) as IMission;
            if (null == mission)
                return;

            if (mission.State == MissionState.NotStarted)
                return;

            activeMission = mission;
            SetAll();
        }

        public void ShowPreviousMission()
        {
            print("ShowPreviousMission()");
            IMission mission = missionManager.Missions.Find(x => x.Id == activeMission.Id - 1) as IMission;
            if (null == mission)
                return;

            if (mission.State == MissionState.NotStarted)
                return;

            activeMission = mission;
            SetAll();
        }

        /// <summary>
        /// assigns the Mission data to the assigned Ui elements
        /// </summary>
        private void SetAll()
        {
            SetMissionTitle();
            SetMissionDescription();
            SetMissionReward();
            SetIndicators();
        }

        private void SetMissionTitle()
        {
            GameObject uiElement = GameObject.Find("Mission Header Text");
            uiElement.GetComponent<UnityEngine.UI.Text>().text = activeMission.Name;
        }

        private void SetMissionDescription()
        {
            GameObject uiElement = GameObject.Find("Descriptive Text");
            uiElement.GetComponent<UnityEngine.UI.Text>().text = activeMission.Description;

        }

        private void SetMissionReward()
        {
            GameObject uiElement = GameObject.Find("Coin Text");
            uiElement.GetComponent<UnityEngine.UI.Text>().text = activeMission.CoinReward.ToString();

        }

        private void SetIndicators()
        {
            print("SetIndicators()");
            GameObject uiElement = GameObject.Find("Complete Image");

            // TODO: setting color does not work
            if (activeMission.State == MissionState.Finished)
                uiElement.GetComponent<UnityEngine.UI.Image>().color = SUCCESS_COLOR;
            else
                uiElement.GetComponent<UnityEngine.UI.Image>().color = NOT_SUCCESS_COLOR;

            // TODO: this doesnt work
            // uiElement = GameObject.Find("Prev Mission Button");
            // uiElement.SetActive(activeMission.Id == 1 ? false : true);

        }
    }
}
