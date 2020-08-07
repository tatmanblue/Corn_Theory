using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CornTheory.Missions
{
    [Serializable]
    public class MissionIndex
    {
        public decimal Id { get; set; }

        public string File { get; set; }
    }

    [Serializable]
    public class Mission : IMission
    {
        public Mission()
        {
            Parent = -1;
        }

        public decimal Parent { get; set; }
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public MissionState State { get; set; }
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public MissionType Type { get; set; }
        public IMissionStateCheck Check { get; set; }
        public int CoinReward { get; set; }
    }


    [Serializable]
    public class MissionList : List<IMission> { }
}
