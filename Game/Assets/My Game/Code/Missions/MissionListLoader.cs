using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CornTheory.Missions
{
    /// <summary>
    /// This class assists UI with building the missions list
    /// </summary>
    public class MissionListLoader : MonoBehaviour
    {
        [SerializeField] GameObject container;
        [SerializeField] GameObject listItem;

        // Start is called before the first frame update
        void Start()
        {
            if (null == container || null == listItem)
                return;

            var missionManager = FindObjectOfType<MissionManager>();
            if (null == missionManager)
                return;

            foreach (Mission mission in missionManager.Missions)
            {
                if (mission.State != MissionState.OnGoing)
                    continue;

                var missionUI = Instantiate(listItem) as GameObject;
                var missionText = missionUI.GetComponentInChildren<Text>();
                missionText.text = mission.Name;
                missionUI.transform.SetParent(container.transform, false);
            }
        }

        // Update is called once per frame
        void Update() { }
    }
}