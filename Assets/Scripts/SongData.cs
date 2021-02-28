using G4AW2;
using G4AW2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Song")]
public class SongData : ScriptableObject {
	public string DisplayName;
	
	public float PixelsPerSecond;
	public List<NoteData> TopNotes;
	public List<NoteData> MiddleNotes;
	public List<NoteData> BottomNotes;


	public QuestConfig QuestConfigToUnlock;

	public bool IsUnlocked => QuestConfigToUnlock == null || SaveGame.SaveData.CompletedQuests.Any(id => id.Id == QuestConfigToUnlock.Id);

	[Header("Buff")]
	public float BuffDuration;
	public List<Drop> DropChances;
	public float MinDropTime;
	public float MaxDropTime;


	public float GetSpawnTime(float acc) {
		
		return MinDropTime + (1f - acc) * (MaxDropTime - MinDropTime) +
				UnityEngine.Random.Range(-0.5f, 0.5f) * (MaxDropTime - MinDropTime);
	}
	
}


[Serializable]
public class NoteData {
	public float AppearTime;
}