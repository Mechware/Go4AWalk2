using CustomEvents;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace G4AW2.Saving {
	[CreateAssetMenu(menuName = "Save Manager")]
	public class SaveManager : ScriptableObject {

		private string saveFile;
		private string backUpFile;

		public List<SaveableScriptableObject> ObjectsToSave;
		public List<KeyValuePairSaveableSetAndAllIdsList> RuntimeSetsAndAllIdsSets;

		void OnEnable() {
			saveFile = Path.Combine(Application.persistentDataPath, "Saves", "G4AW2_" + name + ".save");
			backUpFile = saveFile + "_Backup";
		}

		[ContextMenu("Save")]
		public void Save() {
			if (File.Exists(saveFile)) {
				if (File.Exists(backUpFile)) {
					File.Delete(backUpFile);
				}

				File.Copy(saveFile, backUpFile);
				File.Delete(saveFile);
			}

			Directory.CreateDirectory(Path.GetDirectoryName(saveFile));

			File.WriteAllText(saveFile, GetSaveString());
		}

		[ContextMenu("Load")]
		public void Load() {
            string saveFilePath;
			if (!File.Exists(saveFile)) {
                
                Debug.LogWarning("Could not find save file. Attempting to load back up");
                if (!File.Exists(backUpFile)) {
                    Debug.LogWarning("Could not find back up file.");
                    return;
                } else {
                    saveFilePath = backUpFile;
                }
            } else {
                saveFilePath = saveFile;
            }

            SaveObject saveData = JsonUtility.FromJson<SaveObject>(File.ReadAllText(saveFilePath));
			foreach (KeyValuePairStringString kvp in saveData.VariableDictionary) {
				SaveableScriptableObject soToOverwrite = ObjectsToSave.First(so => so.name.Equals(kvp.Key));

				if (soToOverwrite == null) {
					// Removed a variable?
					Debug.LogWarning("Could not find scriptable object matching name: " + kvp.Key);
					continue;
				}

				soToOverwrite.SetData(kvp.Value);
			}

			foreach (KeyValuePairStringString kvp in saveData.RuntimeSetDictionary) {
				KeyValuePairSaveableSetAndAllIdsList set2 = RuntimeSetsAndAllIdsSets.FirstOrDefault(set => set.Key.name.Equals(kvp.Key));
				if (set2.Equals(default(KeyValuePairSaveableSetAndAllIdsList))) {
					Debug.LogWarning("Could not find runtime set matching name: " + kvp.Key);
					continue;
				}
				set2.Key.SetData(kvp.Value, set2.Value);
			}
		}

		private string GetSaveString() {
			return JsonUtility.ToJson(GetSaveData());
		}

		private SaveObject GetSaveData() {
			List<KeyValuePairStringString> saveDictForVariables = ObjectsToSave.Select(so => new KeyValuePairStringString(so.name, so.GetSaveString())).ToList();

			List<KeyValuePairStringString> saveDictForRuntimeSets = RuntimeSetsAndAllIdsSets.Select(kvp => new KeyValuePairStringString(kvp.Key.name, kvp.Key.GetSaveString())).ToList();

			return new SaveObject {
				VariableDictionary = saveDictForVariables,
				RuntimeSetDictionary = saveDictForRuntimeSets
			};
		}

		[System.Serializable]
		private struct KeyValuePairStringString {
			public string Key;
			public string Value;
			public KeyValuePairStringString(string key, string value) {
				Key = key;
				Value = value;
			}
		}

		[System.Serializable]
		public struct KeyValuePairSaveableSetAndAllIdsList {
			public SaveableScriptableObject Key;
			public ScriptableObject Value; // Persistent Runtime Set
		}

		private struct SaveObject {
			public List<KeyValuePairStringString> VariableDictionary;
			public List<KeyValuePairStringString> RuntimeSetDictionary;
		}

		[ContextMenu("Clear all save data")]
		void ClearSaveData() {
			File.Delete(saveFile);
            File.Delete(backUpFile);
		}

#if UNITY_EDITOR
		[ContextMenu("Print Save String")]
		void PrintSaveString() {
			Debug.Log(GetSaveString());
		}
#endif
	}
}

