using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CornTheory.Missions
{
    [Serializable]
    public class Mission : IMission
    {
        public Mission()
        {
            Parent = -1;
        }

        public int Parent { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public MissionState State { get; set; }
        // [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public MissionType Type { get; set; }
        public IMissionStateCheck Check { get; set; }
    }


    [Serializable]
    public class MissionList : List<IMission> { }
}
