using System;
using UnityEngine;

namespace CornTheory.Utility
{
    /// <summary>
    /// help with using Unity JsonUtility
    /// its very picky,  files have to be exactly as types are defined etc...
    /// no properties
    /// see https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
    /// </summary>
    public static class JsonTools
    {
        public static T[] ArrayFromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
