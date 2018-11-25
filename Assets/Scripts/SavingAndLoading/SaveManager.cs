using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Utils;
using Sirenix.Serialization;
using UnityEngine;

namespace G4AW2.Saving {
	[CreateAssetMenu(menuName = "Save Manager")]
	public class SaveManager : ScriptableObject {

		private readonly string saveString = "Save";

		public List<VariableBase> ObjectsToSave;

		[ContextMenu("Save")]
		public void Save() {
			var saveData = GetSaveData();
			PlayerPrefs.SetString(saveString + name, JsonUtility.ToJson(saveData));
		}

		[ContextMenu("Load")]
		public void Load() {
			if (!PlayerPrefs.HasKey(saveString + name))
				return;

			var saveData = JsonUtility.FromJson<SaveObject>(PlayerPrefs.GetString(saveString + name));
			foreach (var kvp in saveData.PretendDictionary) {
				VariableBase soToOverwrite = ObjectsToSave.First(so => so.name.Equals(kvp.Key));

				if (soToOverwrite == null) {
					// Removed a variable?
					Debug.LogWarning("Could not find scriptable object matching name: " + kvp.Key);
					continue;
				}

				VariableBase emptySO = (VariableBase) ScriptableObject.CreateInstance(soToOverwrite.GetType());
				JsonUtility.FromJsonOverwrite(kvp.Value, emptySO);

				soToOverwrite.CopyValue(emptySO);
			}
		}

		private SaveObject GetSaveData() {
			var saveDict = new List<KeyValuePairStringString>();
			foreach (var so in ObjectsToSave) {
				saveDict.Add(new KeyValuePairStringString(so.name, JsonUtility.ToJson(so)));
			}

			return new SaveObject {PretendDictionary = saveDict};
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

		private struct SaveObject {
			public List<KeyValuePairStringString> PretendDictionary;
		}

#if UNITY_EDITOR
		[ContextMenu("Print Save String")]
		void PrintSaveString() {
			Debug.Log(JsonUtility.ToJson(GetSaveData()));
		}
#endif
	}
}

