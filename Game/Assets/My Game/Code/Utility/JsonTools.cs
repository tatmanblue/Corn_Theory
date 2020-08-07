using Newtonsoft.Json;
using System;
using UnityEngine;

namespace CornTheory.Utility
{
    /// <summary>
    /// Helper methods for loading json resources
    /// </summary>
    public static class JsonTools
    {
        public static T LoadFromResource<T>(string resourceName)
        {
            string resource = Resources.Load<TextAsset>(resourceName).ToString();
            return JsonTools.LoadFromJson<T>(resource);
        }
        public static T LoadFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
