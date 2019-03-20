using System;
using CustomEvents;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using G4AW2.Dialogue;
using UnityEngine;

namespace G4AW2.Saving {
	[CreateAssetMenu(menuName = "Misc/Save Manager")]
	public class SaveManager : ScriptableObject {

		private string saveFile;
		private string backUpFile;

        
		public List<ListObject> ObjectsToSave; 

        [System.Serializable]
	    public class ListObject {
	        public ScriptableObject ObjectToSave; // These objects must inherit from ISaveable. There's no way to enforce that though.
            public List<ScriptableObject> OtherData;
	    }

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

		    string directoryName = Path.GetDirectoryName(saveFile);

            if(!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

            string saveString = GetSaveString();

            File.WriteAllText(saveFile, saveString);
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

		    try {
		        LoadFromString(File.ReadAllText(saveFilePath));
		    }
		    catch (Exception e) {
		        PopUp.SetPopUp("Could not load save data, would you like to clear all progress?", new [] {"Yes", "No"},
		            new Action[] {
		                () => { ClearSaveData(); },
                        () =>{}
		            });
		    }

        }

	    private void LoadFromString(string loadText) {
	        SaveObject saveData = JsonUtility.FromJson<SaveObject>(loadText);
	        foreach(KeyValuePairStringString kvp in saveData.VariableDictionary) {
	            ListObject soToOverwrite = ObjectsToSave.First(so => so.ObjectToSave.name.Equals(kvp.Key));

	            if(soToOverwrite == null) {
	                // Removed a variable?
	                Debug.LogWarning("Could not find scriptable object matching name: " + kvp.Key);
	                continue;
	            }

	            if (soToOverwrite.OtherData.Count == 0) {
	                ((ISaveable) soToOverwrite.ObjectToSave).SetData(kvp.Value);
	            }
	            else {
	                ((ISaveable)soToOverwrite.ObjectToSave).SetData(kvp.Value, soToOverwrite.OtherData[0]);
                }
            }
        }

		private string GetSaveString() {
			return JsonUtility.ToJson(GetSaveData());
		}

		private SaveObject GetSaveData() {
			List<KeyValuePairStringString> saveDictForVariables = new List<KeyValuePairStringString>();// = 
                //ObjectsToSave.Select(so => new KeyValuePairStringString(so.ObjectToSave.name, ((ISaveable)so.ObjectToSave).GetSaveString())).ToList();

		    foreach (var saveObjs in ObjectsToSave) {
		        string key = saveObjs.ObjectToSave.name;
		        string value = ((ISaveable) saveObjs.ObjectToSave).GetSaveString();


                saveDictForVariables.Add(new KeyValuePairStringString(key, value));
		    }

			return new SaveObject {
				VariableDictionary = saveDictForVariables,
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
			public List<KeyValuePairStringString> VariableDictionary;
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

