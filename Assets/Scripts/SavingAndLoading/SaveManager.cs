using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Questing;
using G4AW2.Utils;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Saving {
	[CreateAssetMenu(menuName = "Save Manager")]
	public class SaveManager : ScriptableObject {

		private readonly string saveString = "Save";

		public List<VariableBase> ObjectsToSave;
		public List<RuntimeSetBase> RuntimeSetsToSave;


		public PersistentSetFollowerData AllFollowers; // For ID look ups
		public PersistentSetQuest AllQuests; // For ID look ups.
		public PersistentSetItem AllItems;

		public RuntimeSetFollowerData CurrentFollowers;
		public RuntimeSetQuest OpenQuests;

		public RuntimeSetItem Consumables; 
		public RuntimeSetItem Equipment; 
		public RuntimeSetItem Materials;

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

			LoadIID(CurrentFollowers, AllFollowers.ToArray(), saveData.Followers);
			LoadIID(OpenQuests, AllQuests.ToArray(), saveData.Quests);
			LoadIID(Consumables, AllItems.ToArray(), saveData.Consumables);
			LoadIID(Equipment, AllItems.ToArray(), saveData.Equipment);
			LoadIID(Materials, AllItems.ToArray(), saveData.Materials);

		}

		// If you don't know C# well, IGNORE THIS.
		private static void LoadIID<T, TEvent>(RuntimeSetGeneric<T, TEvent> seto, T[] allArray, List<int> ids) where T : IID where TEvent : UnityEvent<T> {
			var set = (RuntimeSetGeneric<T, TEvent>) seto;
			set.Clear();
			foreach (int id in ids) {
				T thing = allArray.FirstOrDefault(f => f.GetID() == id);
				if (thing == null) {
					Debug.LogWarning("Currently laoding a " + typeof(T).Name + " that doesn't have a valid id. ID: " + id);
					continue;
				}
				set.Add(thing, true);
			}
			set.OnChange.Invoke(default(T));

		}

		private SaveObject GetSaveData() {
			var saveDict = new List<KeyValuePairStringString>();
			foreach (var so in ObjectsToSave) {
				saveDict.Add(new KeyValuePairStringString(so.name, JsonUtility.ToJson(so)));
			}

			List<int> followers = CurrentFollowers.Value.Select(f => f.GetID()).ToList();
			List<int> quests = OpenQuests.Value.Select(q => q.GetID()).ToList();
			List<int> equipment = Equipment.Value.Select(q => q.GetID()).ToList();
			List<int> consumables = Consumables.Value.Select(q => q.GetID()).ToList();
			List<int> materials = Materials.Value.Select(q => q.GetID()).ToList();

			return new SaveObject {
				PretendDictionary = saveDict,
				Followers = followers,
				Quests = quests,
				Equipment = equipment,
				Consumables = consumables,
				Materials = materials
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

		private struct SaveObject {
			public List<KeyValuePairStringString> PretendDictionary;
			public List<int> Followers;
			public List<int> Quests;
			public List<int> Consumables;
			public List<int> Equipment;
			public List<int> Materials;
		}

#if UNITY_EDITOR
		[ContextMenu("Print Save String")]
		void PrintSaveString() {
			Debug.Log(JsonUtility.ToJson(GetSaveData()));
		}

		[ContextMenu("Clear all save data")]
		void ClearSaveData() {
			PlayerPrefs.DeleteAll();
			CurrentFollowers.Clear();
			OpenQuests.Clear();
		}
#endif
	}
}

