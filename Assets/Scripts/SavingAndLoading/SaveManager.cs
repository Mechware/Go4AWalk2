using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Serialization = Sirenix.Serialization.SerializationUtility;

namespace G4AW2
{
    [CreateAssetMenu(menuName = "Managers/SaveGame")]
    public class SaveManager : ScriptableObject
    {
        private Dictionary<string, Func<object>> _getSaveDataFunctions = new Dictionary<string, Func<object>>();
        private Dictionary<string, Action<object>> _loadDataFunctions = new Dictionary<string, Action<object>>();

        public void RegisterSaveFunction(string saveFile, Func<object> getSaveDataFunction)
        {
            _getSaveDataFunctions[saveFile] = getSaveDataFunction;
        }

        public void RegisterLoadFunction(string name, Action<object> onDataLoaded)
        {
            _loadDataFunctions[name] = onDataLoaded;
        }

        public void Save(string savePath)
        {
            Dictionary<string, object> saveData = new Dictionary<string, object>();
            foreach(var kvp in _getSaveDataFunctions)
            {
                saveData.Add(kvp.Key, kvp.Value());
            }

            var bytes = Serialization.SerializeValue(saveData, DataFormat.JSON);
            File.WriteAllBytes(savePath, bytes);
        }

        public void Load(string savePath)
        {
            List<string> calledBack = new List<string>();

            if (File.Exists(savePath))
            {
                var bytes = File.ReadAllBytes(savePath);
                var saveDict = Serialization.DeserializeValue<Dictionary<string, object>>(bytes, DataFormat.JSON);

                foreach (var kvp in saveDict)
                {
                    if (!_loadDataFunctions.ContainsKey(kvp.Key))
                    {
                        Debug.LogError($"Trying to load data {kvp.Key} but that has no matching load function");
                        continue;
                    }

                    _loadDataFunctions[kvp.Key](kvp.Value);
                    calledBack.Add(kvp.Key);
                }
            }
            
            foreach(var kvp in _loadDataFunctions)
            {
                if (calledBack.Contains(kvp.Key)) continue;

                _loadDataFunctions[kvp.Key](null);
            }
        }

#if UNITY_EDITOR
        [Serializable]
        private class SaveDataTest
        {
            public bool Field0 = false;
            public int Field1 = 1;
            public string Field2 = "2";
            public NestedTest Nest = new NestedTest();
        }

        [Serializable]
        private class NestedTest
        {
            public string NestedValue3 = "3";
        }

        [ContextMenu("Run Test")]
        private void RunTest()
        {
            _getSaveDataFunctions.Clear();
            var sd = new SaveDataTest();
            sd.Field0 = true;
            sd.Field1 = 10;
            sd.Field2 = "not 2 lol";
            sd.Nest.NestedValue3 = "4";
            RegisterSaveFunction("Hello", () => sd);
            
            string saveFilePath = Application.persistentDataPath + "Test.json";
            Debug.Log(saveFilePath);
            Save(saveFilePath);
            _getSaveDataFunctions.Clear();


            SaveDataTest sd2 = null;
            RegisterLoadFunction("Hello", (o) =>
            {
                sd2 = (SaveDataTest)o;
            });
            Load(saveFilePath);

            Debug.Assert(sd.Field0 == sd2.Field0);
            Debug.Assert(sd.Field1 == sd2.Field1);
            Debug.Assert(sd.Field2 == sd2.Field2);
            Debug.Assert(sd.Nest.NestedValue3 == sd2.Nest.NestedValue3);
            Debug.Log("Pass!");
        }
#endif
    }

    
}


