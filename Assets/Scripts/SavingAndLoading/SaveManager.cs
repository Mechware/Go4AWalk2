using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Utils;
using UnityEngine;

namespace G4AW2.Saving {
	[CreateAssetMenu(menuName = "Save Manager")]
	public class SaveManager : ScriptableObject {

		private readonly string saveString = "Save";

		public List<ScriptableObject> ObjectsToSave;

		public void Save() {
			var saveData = GetSaveData();
			PlayerPrefs.SetString(saveString, JsonUtility.ToJson(saveData));
		}

		public void Load() {
			if (!PlayerPrefs.HasKey(saveString))
				return;

			Dictionary<string, string> saveData = JsonUtility.FromJson<Dictionary<string, string>>(PlayerPrefs.GetString(saveString));
			foreach (var kvp in saveData) {
				var soToOverwrite = ObjectsToSave.First(so => so.name.Equals(kvp.Key));
				if (soToOverwrite == null) {
					// Removed a variable?
					Debug.LogWarning("Could not find scriptable object matching name: " + kvp.Key);
					continue;
				}

				JsonUtility.FromJsonOverwrite(kvp.Value, soToOverwrite);
			}
		}

		private Dictionary<string, string> GetSaveData() {
			Dictionary<string, string> saveDict = new Dictionary<string, string>();
			foreach (var so in ObjectsToSave) {
				saveDict.Add(so.name, JsonUtility.ToJson(so));
			}
			return saveDict;
		}

#if UNITY_EDITOR
		[ContextMenu("Print Save String")]
		void PrintSaveString() {
			DebugUtils.PrintDictionary(GetSaveData());
		}
#endif
	}
}

